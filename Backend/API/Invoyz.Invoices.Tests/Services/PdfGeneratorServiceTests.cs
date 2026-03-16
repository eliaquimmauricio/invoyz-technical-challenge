using Bogus;
using FluentAssertions;
using Invoyz.Invoices.Domain.Entities;
using Invoyz.Invoices.Domain.Enums;
using Invoyz.Invoices.Domain.Services;
using Invoyz.Invoices.Domain.Dtos.Invoices;
using Invoyz.Invoices.Domain.Dtos.InvoiceLines;
using Invoyz.Invoices.Domain.Dtos.Customers;
using Invoyz.Invoices.Domain.Dtos.Products;

namespace Invoyz.Invoices.Tests.Services
{
    public class PdfGeneratorServiceTests
    {
        private readonly PdfGeneratorService _sut;
        private readonly Faker _faker;

        public PdfGeneratorServiceTests()
        {
            _sut = new PdfGeneratorService();
            _faker = new Faker();
        }

        [Fact]
        public async Task GenerateInvoicePdfAsync_ShouldGeneratePdfBytes_WhenInvoiceProvided()
        {
            // Arrange
            var invoice = CreateSampleInvoice();

            // Act
            var result = await _sut.GenerateInvoicePdfAsync(invoice);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Length.Should().BeGreaterThan(0);
            
            // PDF files typically start with %PDF
            var pdfHeader = System.Text.Encoding.ASCII.GetString(result.Take(4).ToArray());
            pdfHeader.Should().Be("%PDF");
        }

        [Fact]
        public async Task GenerateInvoicePdfAsync_ShouldGeneratePdf_WhenInvoiceHasNoLines()
        {
            // Arrange
            var invoice = CreateSampleInvoice();
            invoice.Lines.Clear();

            // Act
            var result = await _sut.GenerateInvoicePdfAsync(invoice);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GenerateInvoicePdfAsync_ShouldGeneratePdf_WhenInvoiceHasNoCustomer()
        {
            // Arrange
            var invoice = CreateSampleInvoice();
            invoice.Customer = null!;

            // Act
            var result = await _sut.GenerateInvoicePdfAsync(invoice);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GenerateInvoicePdfAsync_ShouldGeneratePdf_WhenInvoiceHasMultipleLines()
        {
            // Arrange
            var invoice = CreateSampleInvoice();
            
            // Add more lines
            for (int i = 0; i < 5; i++)
            {
                var product = CreateSampleProduct();
                var line = InvoiceLine.FromWriteDto(new InvoiceLineWriteDto
                {
                    ProductId = product.Id,
                    Quantity = _faker.Random.Int(1, 10),
                    UnitPrice = _faker.Finance.Amount(10, 500),
                    TaxRate = 0.21m
                }, invoice.Id);
                line.Product = product;
                invoice.Lines.Add(line);
            }

            // Act
            var result = await _sut.GenerateInvoicePdfAsync(invoice);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Length.Should().BeGreaterThan(1000); // Should be larger with more content
        }

        [Fact]
        public async Task GenerateInvoicePdfAsync_ShouldGenerateDifferentPdfs_ForDifferentInvoices()
        {
            // Arrange
            var invoice1 = CreateSampleInvoice();
            var invoice2 = CreateSampleInvoice();

            // Act
            var result1 = await _sut.GenerateInvoicePdfAsync(invoice1);
            var result2 = await _sut.GenerateInvoicePdfAsync(invoice2);

            // Assert
            result1.Should().NotBeNull();
            result2.Should().NotBeNull();
            result1.Should().NotBeEquivalentTo(result2);
        }

        [Fact]
        public async Task GenerateInvoicePdfAsync_ShouldHandleDifferentInvoiceStatuses()
        {
            // Arrange
            var statuses = Enum.GetValues<InvoiceStatus>();

            foreach (var status in statuses)
            {
                var invoice = CreateSampleInvoice();
                invoice.Status = status;

                // Act
                var result = await _sut.GenerateInvoicePdfAsync(invoice);

                // Assert
                result.Should().NotBeNull();
                result.Should().NotBeEmpty();
            }
        }

        [Fact]
        public async Task GenerateInvoicePdfAsync_ShouldHandleProductsWithoutDescription()
        {
            // Arrange
            var invoice = CreateSampleInvoice();
            if (invoice.Lines.Any())
            {
                var firstLine = invoice.Lines.First();
                if (firstLine.Product != null)
                {
                    firstLine.Product.Description = null;
                }
            }

            // Act
            var result = await _sut.GenerateInvoicePdfAsync(invoice);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }

        private Invoice CreateSampleInvoice()
        {
            var customer = Customer.FromWriteDto(new CustomerWriteDto
            {
                Name = _faker.Company.CompanyName(),
                VatNumber = _faker.Finance.Account(10),
                Email = _faker.Internet.Email(),
                Address = _faker.Address.FullAddress()
            });

            var invoice = Invoice.FromWriteDto(new InvoiceWriteDto
            {
                InvoiceNumber = _faker.Finance.Account(8),
                CustomerId = customer.Id,
                IssueDate = _faker.Date.Past(),
                DueDate = _faker.Date.Future(),
                Status = InvoiceStatus.Sent,
                Lines = new List<InvoiceLineWriteDto>()
            });

            invoice.Customer = customer;

            // Add a sample line with product
            var product = CreateSampleProduct();
            var line = InvoiceLine.FromWriteDto(new InvoiceLineWriteDto
            {
                ProductId = product.Id,
                Quantity = _faker.Random.Int(1, 10),
                UnitPrice = _faker.Finance.Amount(10, 500),
                TaxRate = 0.21m
            }, invoice.Id);
            
            line.Product = product;
            invoice.Lines.Add(line);

            return invoice;
        }

        private Product CreateSampleProduct()
        {
            return Product.FromWriteDto(new ProductWriteDto
            {
                Name = _faker.Commerce.ProductName(),
                Description = _faker.Commerce.ProductDescription(),
                UnitPrice = _faker.Finance.Amount(10, 500),
                TaxRate = 0.21m
            });
        }
    }
}
