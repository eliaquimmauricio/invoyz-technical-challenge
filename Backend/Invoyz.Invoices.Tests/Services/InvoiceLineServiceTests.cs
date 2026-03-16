using Bogus;
using FluentAssertions;
using Invoyz.Invoices.Domain.Dtos.InvoiceLines;
using Invoyz.Invoices.Domain.Entities;
using Invoyz.Invoices.Domain.Interfaces.Data;
using Invoyz.Invoices.Domain.Services;
using Moq;

namespace Invoyz.Invoices.Tests.Services
{
    public class InvoiceLineServiceTests
    {
        private readonly Mock<IRepository<InvoiceLine, InvoiceLineReadDto, InvoiceLineWriteDto>> _repositoryMock;
        private readonly InvoiceLineService _sut;
        private readonly Faker<InvoiceLineWriteDto> _invoiceLineWriteDtoFaker;
        private readonly Faker<InvoiceLine> _invoiceLineFaker;

        public InvoiceLineServiceTests()
        {
            _repositoryMock = new Mock<IRepository<InvoiceLine, InvoiceLineReadDto, InvoiceLineWriteDto>>();
            _sut = new InvoiceLineService(_repositoryMock.Object);
            
            _invoiceLineWriteDtoFaker = new Faker<InvoiceLineWriteDto>()
                .RuleFor(l => l.ProductId, f => Guid.NewGuid())
                .RuleFor(l => l.Quantity, f => f.Random.Int(1, 100))
                .RuleFor(l => l.UnitPrice, f => f.Finance.Amount(1, 1000))
                .RuleFor(l => l.TaxRate, f => f.Finance.Amount(0, 0.25m));

            _invoiceLineFaker = new Faker<InvoiceLine>()
                .CustomInstantiator(f => InvoiceLine.FromWriteDto(new InvoiceLineWriteDto
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = f.Random.Int(1, 100),
                    UnitPrice = f.Finance.Amount(1, 1000),
                    TaxRate = f.Finance.Amount(0, 0.25m)
                }, Guid.NewGuid()));
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllInvoiceLines_WhenInvoiceLinesExist()
        {
            // Arrange
            var invoiceLines = _invoiceLineFaker.Generate(5);
            _repositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(invoiceLines);

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(5);
            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoInvoiceLinesExist()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<InvoiceLine>());

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnInvoiceLine_WhenInvoiceLineExists()
        {
            // Arrange
            var invoiceLineId = Guid.NewGuid();
            var invoiceLine = _invoiceLineFaker.Generate();
            _repositoryMock.Setup(r => r.GetByIdAsync(invoiceLineId))
                .ReturnsAsync(invoiceLine);

            // Act
            var result = await _sut.GetByIdAsync(invoiceLineId);

            // Assert
            result.Should().NotBeNull();
            result!.Quantity.Should().Be(invoiceLine.Quantity);
            result.UnitPrice.Should().Be(invoiceLine.UnitPrice);
            _repositoryMock.Verify(r => r.GetByIdAsync(invoiceLineId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenInvoiceLineDoesNotExist()
        {
            // Arrange
            var invoiceLineId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetByIdAsync(invoiceLineId))
                .ReturnsAsync((InvoiceLine?)null);

            // Act
            var result = await _sut.GetByIdAsync(invoiceLineId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByInvoiceIdAsync_ShouldReturnInvoiceLinesForSpecificInvoice()
        {
            // Arrange
            var invoiceId = Guid.NewGuid();
            var invoiceLines = _invoiceLineFaker.Generate(10);
            
            // Set specific invoice IDs
            for (int i = 0; i < 5; i++)
            {
                invoiceLines[i].InvoiceId = invoiceId;
            }
            for (int i = 5; i < 10; i++)
            {
                invoiceLines[i].InvoiceId = Guid.NewGuid();
            }

            _repositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(invoiceLines);

            // Act
            var result = await _sut.GetByInvoiceIdAsync(invoiceId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(5);
            result.Should().OnlyContain(l => l.InvoiceId == invoiceId);
            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByInvoiceIdAsync_ShouldReturnEmptyList_WhenNoInvoiceLinesForInvoice()
        {
            // Arrange
            var invoiceId = Guid.NewGuid();
            var invoiceLines = _invoiceLineFaker.Generate(5);
            
            // Set all invoice lines to different invoice IDs
            foreach (var line in invoiceLines)
            {
                line.InvoiceId = Guid.NewGuid();
            }

            _repositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(invoiceLines);

            // Act
            var result = await _sut.GetByInvoiceIdAsync(invoiceId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateInvoiceLine_WhenValidDtoProvided()
        {
            // Arrange
            var writeDto = _invoiceLineWriteDtoFaker.Generate();
            var capturedInvoiceLine = (InvoiceLine?)null;

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<InvoiceLine>()))
                .Callback<InvoiceLine>(l => capturedInvoiceLine = l)
                .Returns(Task.CompletedTask);

            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.CreateAsync(writeDto);

            // Assert
            result.Should().NotBeNull();
            result.ProductId.Should().Be(writeDto.ProductId);
            result.Quantity.Should().Be(writeDto.Quantity);
            result.UnitPrice.Should().Be(writeDto.UnitPrice);
            result.TaxRate.Should().Be(writeDto.TaxRate);
            
            capturedInvoiceLine.Should().NotBeNull();
            capturedInvoiceLine!.Quantity.Should().Be(writeDto.Quantity);
            
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<InvoiceLine>()), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateInvoiceLine_WhenInvoiceLineExists()
        {
            // Arrange
            var invoiceLineId = Guid.NewGuid();
            var existingInvoiceLine = _invoiceLineFaker.Generate();
            var writeDto = _invoiceLineWriteDtoFaker.Generate();

            _repositoryMock.Setup(r => r.GetByIdAsync(invoiceLineId))
                .ReturnsAsync(existingInvoiceLine);

            _repositoryMock.Setup(r => r.Update(It.IsAny<InvoiceLine>()));

            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _sut.UpdateAsync(invoiceLineId, writeDto);

            // Assert
            existingInvoiceLine.ProductId.Should().Be(writeDto.ProductId);
            existingInvoiceLine.Quantity.Should().Be(writeDto.Quantity);
            existingInvoiceLine.UnitPrice.Should().Be(writeDto.UnitPrice);
            existingInvoiceLine.TaxRate.Should().Be(writeDto.TaxRate);
            
            _repositoryMock.Verify(r => r.GetByIdAsync(invoiceLineId), Times.Once);
            _repositoryMock.Verify(r => r.Update(existingInvoiceLine), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenInvoiceLineDoesNotExist()
        {
            // Arrange
            var invoiceLineId = Guid.NewGuid();
            var writeDto = _invoiceLineWriteDtoFaker.Generate();

            _repositoryMock.Setup(r => r.GetByIdAsync(invoiceLineId))
                .ReturnsAsync((InvoiceLine?)null);

            // Act
            var act = async () => await _sut.UpdateAsync(invoiceLineId, writeDto);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Entity with id {invoiceLineId} not found.");
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteInvoiceLine_WhenInvoiceLineExists()
        {
            // Arrange
            var invoiceLineId = Guid.NewGuid();
            var invoiceLine = _invoiceLineFaker.Generate();

            _repositoryMock.Setup(r => r.GetByIdAsync(invoiceLineId))
                .ReturnsAsync(invoiceLine);

            _repositoryMock.Setup(r => r.Delete(invoiceLine));

            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _sut.DeleteAsync(invoiceLineId);

            // Assert
            _repositoryMock.Verify(r => r.GetByIdAsync(invoiceLineId), Times.Once);
            _repositoryMock.Verify(r => r.Delete(invoiceLine), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowKeyNotFoundException_WhenInvoiceLineDoesNotExist()
        {
            // Arrange
            var invoiceLineId = Guid.NewGuid();

            _repositoryMock.Setup(r => r.GetByIdAsync(invoiceLineId))
                .ReturnsAsync((InvoiceLine?)null);

            // Act
            var act = async () => await _sut.DeleteAsync(invoiceLineId);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Entity with id {invoiceLineId} not found.");
        }
    }
}
