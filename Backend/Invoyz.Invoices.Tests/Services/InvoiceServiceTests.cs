using Bogus;
using FluentAssertions;
using Invoyz.Invoices.Domain.Dtos.Invoices;
using Invoyz.Invoices.Domain.Dtos.InvoiceLines;
using Invoyz.Invoices.Domain.Entities;
using Invoyz.Invoices.Domain.Enums;
using Invoyz.Invoices.Domain.Helpers;
using Invoyz.Invoices.Domain.Interfaces.Data;
using Invoyz.Invoices.Domain.Interfaces.Services;
using Invoyz.Invoices.Domain.Services;
using Moq;

namespace Invoyz.Invoices.Tests.Services
{
    public class InvoiceServiceTests
    {
        private readonly Mock<IRepository<Invoice, InvoiceReadDto, InvoiceWriteDto>> _repositoryMock;
        private readonly Mock<IPdfGeneratorService> _pdfGeneratorServiceMock;
        private readonly Mock<IFileUtility> _fileUtilityMock;
        private readonly InvoiceService _sut;
        private readonly Faker<InvoiceWriteDto> _invoiceWriteDtoFaker;
        private readonly Faker<Invoice> _invoiceFaker;

        public InvoiceServiceTests()
        {
            _repositoryMock = new Mock<IRepository<Invoice, InvoiceReadDto, InvoiceWriteDto>>();
            _pdfGeneratorServiceMock = new Mock<IPdfGeneratorService>();
            _fileUtilityMock = new Mock<IFileUtility>();
            _sut = new InvoiceService(_repositoryMock.Object, _pdfGeneratorServiceMock.Object, _fileUtilityMock.Object);
            
            _invoiceWriteDtoFaker = new Faker<InvoiceWriteDto>()
                .RuleFor(i => i.InvoiceNumber, f => f.Finance.Account(8))
                .RuleFor(i => i.CustomerId, f => Guid.NewGuid())
                .RuleFor(i => i.IssueDate, f => f.Date.Past())
                .RuleFor(i => i.DueDate, f => f.Date.Future())
                .RuleFor(i => i.Status, f => f.PickRandom<InvoiceStatus>())
                .RuleFor(i => i.Lines, f => new List<InvoiceLineWriteDto>
                {
                    new InvoiceLineWriteDto
                    {
                        ProductId = Guid.NewGuid(),
                        Quantity = f.Random.Int(1, 10),
                        UnitPrice = f.Finance.Amount(1, 1000),
                        TaxRate = f.Finance.Amount(0, 0.25m)
                    }
                });

            _invoiceFaker = new Faker<Invoice>()
                .CustomInstantiator(f =>
                {
                    var invoice = Invoice.FromWriteDto(new InvoiceWriteDto
                    {
                        InvoiceNumber = f.Finance.Account(8),
                        CustomerId = Guid.NewGuid(),
                        IssueDate = f.Date.Past(),
                        DueDate = f.Date.Future(),
                        Status = f.PickRandom<InvoiceStatus>(),
                        Lines = new List<InvoiceLineWriteDto>()
                    });
                    return invoice;
                });
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllInvoicesWithLines_WhenInvoicesExist()
        {
            // Arrange
            var invoices = _invoiceFaker.Generate(3);
            _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Func<IQueryable<Invoice>, IQueryable<Invoice>>>()))
                .ReturnsAsync(invoices);

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            _repositoryMock.Verify(r => r.GetAllAsync(It.IsAny<Func<IQueryable<Invoice>, IQueryable<Invoice>>>()), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoInvoicesExist()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Func<IQueryable<Invoice>, IQueryable<Invoice>>>()))
                .ReturnsAsync(new List<Invoice>());

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnInvoiceWithLines_WhenInvoiceExists()
        {
            // Arrange
            var invoiceId = Guid.NewGuid();
            var invoice = _invoiceFaker.Generate();
            _repositoryMock.Setup(r => r.GetByIdAsync(invoiceId, It.IsAny<Func<IQueryable<Invoice>, IQueryable<Invoice>>>()))
                .ReturnsAsync(invoice);

            // Act
            var result = await _sut.GetByIdAsync(invoiceId);

            // Assert
            result.Should().NotBeNull();
            result!.InvoiceNumber.Should().Be(invoice.InvoiceNumber);
            _repositoryMock.Verify(r => r.GetByIdAsync(invoiceId, It.IsAny<Func<IQueryable<Invoice>, IQueryable<Invoice>>>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenInvoiceDoesNotExist()
        {
            // Arrange
            var invoiceId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetByIdAsync(invoiceId, It.IsAny<Func<IQueryable<Invoice>, IQueryable<Invoice>>>()))
                .ReturnsAsync((Invoice?)null);

            // Act
            var result = await _sut.GetByIdAsync(invoiceId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateInvoice_WhenValidDtoProvided()
        {
            // Arrange
            var writeDto = _invoiceWriteDtoFaker.Generate();
            var capturedInvoice = (Invoice?)null;

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Invoice>()))
                .Callback<Invoice>(i => capturedInvoice = i)
                .Returns(Task.CompletedTask);

            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.CreateAsync(writeDto);

            // Assert
            result.Should().NotBeNull();
            result.InvoiceNumber.Should().Be(writeDto.InvoiceNumber);
            result.CustomerId.Should().Be(writeDto.CustomerId);
            result.Status.Should().Be(writeDto.Status);
            
            capturedInvoice.Should().NotBeNull();
            capturedInvoice!.InvoiceNumber.Should().Be(writeDto.InvoiceNumber);
            
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Invoice>()), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateInvoice_WhenInvoiceExists()
        {
            // Arrange
            var invoiceId = Guid.NewGuid();
            var existingInvoice = _invoiceFaker.Generate();
            var writeDto = _invoiceWriteDtoFaker.Generate();

            _repositoryMock.Setup(r => r.GetByIdAsync(invoiceId))
                .ReturnsAsync(existingInvoice);

            _repositoryMock.Setup(r => r.Update(It.IsAny<Invoice>()));

            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _sut.UpdateAsync(invoiceId, writeDto);

            // Assert
            existingInvoice.InvoiceNumber.Should().Be(writeDto.InvoiceNumber);
            existingInvoice.CustomerId.Should().Be(writeDto.CustomerId);
            existingInvoice.Status.Should().Be(writeDto.Status);
            
            _repositoryMock.Verify(r => r.GetByIdAsync(invoiceId), Times.Once);
            _repositoryMock.Verify(r => r.Update(existingInvoice), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenInvoiceDoesNotExist()
        {
            // Arrange
            var invoiceId = Guid.NewGuid();
            var writeDto = _invoiceWriteDtoFaker.Generate();

            _repositoryMock.Setup(r => r.GetByIdAsync(invoiceId))
                .ReturnsAsync((Invoice?)null);

            // Act
            var act = async () => await _sut.UpdateAsync(invoiceId, writeDto);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Entity with id {invoiceId} not found.");
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteInvoice_WhenInvoiceExists()
        {
            // Arrange
            var invoiceId = Guid.NewGuid();
            var invoice = _invoiceFaker.Generate();

            _repositoryMock.Setup(r => r.GetByIdAsync(invoiceId))
                .ReturnsAsync(invoice);

            _repositoryMock.Setup(r => r.Delete(invoice));

            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _sut.DeleteAsync(invoiceId);

            // Assert
            _repositoryMock.Verify(r => r.GetByIdAsync(invoiceId), Times.Once);
            _repositoryMock.Verify(r => r.Delete(invoice), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowKeyNotFoundException_WhenInvoiceDoesNotExist()
        {
            // Arrange
            var invoiceId = Guid.NewGuid();

            _repositoryMock.Setup(r => r.GetByIdAsync(invoiceId))
                .ReturnsAsync((Invoice?)null);

            // Act
            var act = async () => await _sut.DeleteAsync(invoiceId);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Entity with id {invoiceId} not found.");
        }

        [Fact]
        public async Task GeneratePdfAndSendByEmail_ShouldGeneratePdfAndSaveFile_WhenInvoiceExists()
        {
            // Arrange
            var invoiceId = Guid.NewGuid();
            var invoice = _invoiceFaker.Generate();
            var pdfBytes = new byte[] { 1, 2, 3, 4, 5 };

            _repositoryMock.Setup(r => r.GetByIdAsync(invoiceId, It.IsAny<Func<IQueryable<Invoice>, IQueryable<Invoice>>>()))
                .ReturnsAsync(invoice);

            _pdfGeneratorServiceMock.Setup(p => p.GenerateInvoicePdfAsync(invoice))
                .ReturnsAsync(pdfBytes);

            _fileUtilityMock.Setup(f => f.SaveFile(pdfBytes, It.IsAny<string>()));

            // Act
            await _sut.GeneratePdfAndSendByEmail(invoiceId);

            // Assert
            _repositoryMock.Verify(r => r.GetByIdAsync(invoiceId, It.IsAny<Func<IQueryable<Invoice>, IQueryable<Invoice>>>()), Times.Once);
            _pdfGeneratorServiceMock.Verify(p => p.GenerateInvoicePdfAsync(invoice), Times.Once);
            _fileUtilityMock.Verify(f => f.SaveFile(pdfBytes, $"Invoice_{invoiceId}.pdf"), Times.Once);
        }

        [Fact]
        public async Task GeneratePdfAndSendByEmail_ShouldThrowKeyNotFoundException_WhenInvoiceDoesNotExist()
        {
            // Arrange
            var invoiceId = Guid.NewGuid();

            _repositoryMock.Setup(r => r.GetByIdAsync(invoiceId, It.IsAny<Func<IQueryable<Invoice>, IQueryable<Invoice>>>()))
                .ReturnsAsync((Invoice?)null);

            // Act
            var act = async () => await _sut.GeneratePdfAndSendByEmail(invoiceId);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Invoice with id {invoiceId} not found.");
            
            _pdfGeneratorServiceMock.Verify(p => p.GenerateInvoicePdfAsync(It.IsAny<Invoice>()), Times.Never);
            _fileUtilityMock.Verify(f => f.SaveFile(It.IsAny<byte[]>(), It.IsAny<string>()), Times.Never);
        }
    }
}
