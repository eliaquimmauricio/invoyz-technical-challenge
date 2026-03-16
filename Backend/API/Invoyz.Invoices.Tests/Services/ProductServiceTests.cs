using Bogus;
using FluentAssertions;
using Invoyz.Invoices.Domain.Dtos.Products;
using Invoyz.Invoices.Domain.Entities;
using Invoyz.Invoices.Domain.Interfaces.Data;
using Invoyz.Invoices.Domain.Services;
using Moq;

namespace Invoyz.Invoices.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IRepository<Product, ProductReadDto, ProductWriteDto>> _repositoryMock;
        private readonly ProductService _sut;
        private readonly Faker<ProductWriteDto> _productWriteDtoFaker;
        private readonly Faker<Product> _productFaker;

        public ProductServiceTests()
        {
            _repositoryMock = new Mock<IRepository<Product, ProductReadDto, ProductWriteDto>>();
            _sut = new ProductService(_repositoryMock.Object);
            
            _productWriteDtoFaker = new Faker<ProductWriteDto>()
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.UnitPrice, f => f.Finance.Amount(1, 1000))
                .RuleFor(p => p.TaxRate, f => f.Finance.Amount(0, 0.25m));

            _productFaker = new Faker<Product>()
                .CustomInstantiator(f => Product.FromWriteDto(new ProductWriteDto
                {
                    Name = f.Commerce.ProductName(),
                    Description = f.Commerce.ProductDescription(),
                    UnitPrice = f.Finance.Amount(1, 1000),
                    TaxRate = f.Finance.Amount(0, 0.25m)
                }));
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts_WhenProductsExist()
        {
            // Arrange
            var products = _productFaker.Generate(5);
            _repositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(5);
            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Product>());

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = _productFaker.Generate();
            _repositoryMock.Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync(product);

            // Act
            var result = await _sut.GetByIdAsync(productId);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be(product.Name);
            _repositoryMock.Verify(r => r.GetByIdAsync(productId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _sut.GetByIdAsync(productId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateProduct_WhenValidDtoProvided()
        {
            // Arrange
            var writeDto = _productWriteDtoFaker.Generate();
            var capturedProduct = (Product?)null;

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Product>()))
                .Callback<Product>(p => capturedProduct = p)
                .Returns(Task.CompletedTask);

            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.CreateAsync(writeDto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(writeDto.Name);
            result.Description.Should().Be(writeDto.Description);
            result.UnitPrice.Should().Be(writeDto.UnitPrice);
            result.TaxRate.Should().Be(writeDto.TaxRate);
            
            capturedProduct.Should().NotBeNull();
            capturedProduct!.Name.Should().Be(writeDto.Name);
            
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProduct_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var existingProduct = _productFaker.Generate();
            var writeDto = _productWriteDtoFaker.Generate();

            _repositoryMock.Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync(existingProduct);

            _repositoryMock.Setup(r => r.Update(It.IsAny<Product>()));

            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _sut.UpdateAsync(productId, writeDto);

            // Assert
            existingProduct.Name.Should().Be(writeDto.Name);
            existingProduct.Description.Should().Be(writeDto.Description);
            existingProduct.UnitPrice.Should().Be(writeDto.UnitPrice);
            existingProduct.TaxRate.Should().Be(writeDto.TaxRate);
            
            _repositoryMock.Verify(r => r.GetByIdAsync(productId), Times.Once);
            _repositoryMock.Verify(r => r.Update(existingProduct), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var writeDto = _productWriteDtoFaker.Generate();

            _repositoryMock.Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync((Product?)null);

            // Act
            var act = async () => await _sut.UpdateAsync(productId, writeDto);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Entity with id {productId} not found.");
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteProduct_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = _productFaker.Generate();

            _repositoryMock.Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync(product);

            _repositoryMock.Setup(r => r.Delete(product));

            _repositoryMock.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _sut.DeleteAsync(productId);

            // Assert
            _repositoryMock.Verify(r => r.GetByIdAsync(productId), Times.Once);
            _repositoryMock.Verify(r => r.Delete(product), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowKeyNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _repositoryMock.Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync((Product?)null);

            // Act
            var act = async () => await _sut.DeleteAsync(productId);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Entity with id {productId} not found.");
        }
    }
}
