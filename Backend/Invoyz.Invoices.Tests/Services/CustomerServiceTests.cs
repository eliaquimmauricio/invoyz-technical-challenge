using Bogus;
using FluentAssertions;
using Invoyz.Invoices.Domain.Dtos.Customers;
using Invoyz.Invoices.Domain.Entities;
using Invoyz.Invoices.Domain.Interfaces.Data;
using Invoyz.Invoices.Domain.Services;
using Moq;

namespace Invoyz.Invoices.Tests.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<IRepository<Customer, CustomerReadDto, CustomerWriteDto>> _repositoryMock;
        private readonly CustomerService _sut;
        private readonly Faker<CustomerWriteDto> _customerWriteDtoFaker;
        private readonly Faker<Customer> _customerFaker;

        public CustomerServiceTests()
        {
            _repositoryMock = new Mock<IRepository<Customer, CustomerReadDto, CustomerWriteDto>>();
            _sut = new CustomerService(_repositoryMock.Object);
            
            _customerWriteDtoFaker = new Faker<CustomerWriteDto>()
                .RuleFor(c => c.Name, f => f.Company.CompanyName())
                .RuleFor(c => c.VatNumber, f => f.Finance.Account(10))
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.Address, f => f.Address.FullAddress());

            _customerFaker = new Faker<Customer>()
                .CustomInstantiator(f => Customer.FromWriteDto(new CustomerWriteDto
                {
                    Name = f.Company.CompanyName(),
                    VatNumber = f.Finance.Account(10),
                    Email = f.Internet.Email(),
                    Address = f.Address.FullAddress()
                }));
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCustomers_WhenCustomersExist()
        {
            // Arrange
            var customers = _customerFaker.Generate(5);
            _repositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(customers);

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(5);
            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoCustomersExist()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Customer>());

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCustomer_WhenCustomerExists()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = _customerFaker.Generate();
            _repositoryMock.Setup(r => r.GetByIdAsync(customerId))
                .ReturnsAsync(customer);

            // Act
            var result = await _sut.GetByIdAsync(customerId);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be(customer.Name);
            result.Email.Should().Be(customer.Email);
            _repositoryMock.Verify(r => r.GetByIdAsync(customerId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetByIdAsync(customerId))
                .ReturnsAsync((Customer?)null);

            // Act
            var result = await _sut.GetByIdAsync(customerId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateCustomer_WhenValidDtoProvided()
        {
            // Arrange
            var writeDto = _customerWriteDtoFaker.Generate();
            var capturedCustomer = (Customer?)null;

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Customer>()))
                .Callback<Customer>(c => capturedCustomer = c)
                .Returns(Task.CompletedTask);

            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.CreateAsync(writeDto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(writeDto.Name);
            result.VatNumber.Should().Be(writeDto.VatNumber);
            result.Email.Should().Be(writeDto.Email);
            result.Address.Should().Be(writeDto.Address);
            
            capturedCustomer.Should().NotBeNull();
            capturedCustomer!.Name.Should().Be(writeDto.Name);
            
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateCustomer_WhenCustomerExists()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var existingCustomer = _customerFaker.Generate();
            var writeDto = _customerWriteDtoFaker.Generate();

            _repositoryMock.Setup(r => r.GetByIdAsync(customerId))
                .ReturnsAsync(existingCustomer);

            _repositoryMock.Setup(r => r.Update(It.IsAny<Customer>()));

            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _sut.UpdateAsync(customerId, writeDto);

            // Assert
            existingCustomer.Name.Should().Be(writeDto.Name);
            existingCustomer.VatNumber.Should().Be(writeDto.VatNumber);
            existingCustomer.Email.Should().Be(writeDto.Email);
            existingCustomer.Address.Should().Be(writeDto.Address);
            
            _repositoryMock.Verify(r => r.GetByIdAsync(customerId), Times.Once);
            _repositoryMock.Verify(r => r.Update(existingCustomer), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var writeDto = _customerWriteDtoFaker.Generate();

            _repositoryMock.Setup(r => r.GetByIdAsync(customerId))
                .ReturnsAsync((Customer?)null);

            // Act
            var act = async () => await _sut.UpdateAsync(customerId, writeDto);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Entity with id {customerId} not found.");
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteCustomer_WhenCustomerExists()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = _customerFaker.Generate();

            _repositoryMock.Setup(r => r.GetByIdAsync(customerId))
                .ReturnsAsync(customer);

            _repositoryMock.Setup(r => r.Delete(customer));

            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _sut.DeleteAsync(customerId);

            // Assert
            _repositoryMock.Verify(r => r.GetByIdAsync(customerId), Times.Once);
            _repositoryMock.Verify(r => r.Delete(customer), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowKeyNotFoundException_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            _repositoryMock.Setup(r => r.GetByIdAsync(customerId))
                .ReturnsAsync((Customer?)null);

            // Act
            var act = async () => await _sut.DeleteAsync(customerId);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Entity with id {customerId} not found.");
        }
    }
}
