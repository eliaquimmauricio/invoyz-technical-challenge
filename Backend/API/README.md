# Invoyz - Invoice Management API

A comprehensive .NET 10 RESTful API for managing invoices, customers, products, and generating PDF documents with background job processing.

## ?? Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [API Endpoints](#api-endpoints)
- [Background Jobs](#background-jobs)
- [Database](#database)
- [Testing](#testing)
- [PDF Generation](#pdf-generation)
- [Contributing](#contributing)

## ?? Overview

Invoyz is a modern invoice management system built with .NET 10 that provides a complete solution for:
- Managing customers, products, and invoices
- Generating professional PDF invoices
- Background job processing for asynchronous operations
- Real-time job monitoring through Hangfire dashboard

## ? Features

- **Customer Management**: Create, read, update, and delete customer records
- **Product Management**: Manage product catalog with pricing and tax rates
- **Invoice Management**: Complete invoice lifecycle management
- **PDF Generation**: Generate professional invoice PDFs using QuestPDF
- **Background Processing**: Asynchronous PDF generation with Hangfire
- **Job Monitoring**: Real-time job status tracking via Hangfire dashboard
- **RESTful API**: Clean and intuitive API design
- **Entity Framework Core**: Code-first database approach
- **Swagger Documentation**: Interactive API documentation

## ?? Technology Stack

### Framework & Runtime
- **.NET 10.0** - Latest .NET framework
- **C# 14.0** - Modern C# language features

### Core Libraries
- **ASP.NET Core 10.0** - Web API framework
- **Entity Framework Core 10.0** - ORM for database operations
- **SQL Server** - Database engine

### Third-Party Libraries
- **Hangfire 1.8.23** - Background job processing
- **QuestPDF 2026.2.3** - PDF document generation
- **Swashbuckle (Swagger) 10.1.5** - API documentation
- **xUnit** - Unit testing framework
- **FluentAssertions** - Fluent test assertions
- **Bogus** - Test data generation

## ?? Project Structure

```
Invoyz.Invoices/
??? Invoyz.Invoices.Api/           # Web API Layer
?   ??? Controllers/               # API Controllers
?   ??? Program.cs                 # Application entry point
?   ??? appsettings.json          # Configuration
?
??? Invoyz.Invoices.Domain/        # Domain Layer
?   ??? Entities/                  # Domain entities
?   ??? Dtos/                      # Data Transfer Objects
?   ??? Services/                  # Business logic services
?   ??? Interfaces/                # Service interfaces
?   ??? Helpers/                   # Utility helpers
?
??? Invoyz.Invoices.Data/          # Data Access Layer
?   ??? Context.cs                 # Database context
?   ??? Repository.cs              # Generic repository
?   ??? Migrations/                # EF migrations
?
??? Invoyz.Invoices.Tests/         # Test Projects
    ??? Services/                  # Service unit tests
```

## ?? Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) or SQL Server instance
- [Visual Studio 2025](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/eliaquimmauricio/invoyz-technical-challenge.git
   cd invoyz-technical-challenge
   ```

2. **Update connection string**
   
   Edit `Invoyz.Invoices.Api/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InvoyzDb;Trusted_Connection=true;TrustServerCertificate=true;"
     }
   }
   ```

3. **Apply database migrations**
   ```bash
   cd Invoyz.Invoices.Api
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access the application**
   - API: `https://localhost:5001` or `http://localhost:5000`
   - Swagger UI: `https://localhost:5001/swagger`
   - Hangfire Dashboard: `https://localhost:5001/hangfire`

## ?? API Endpoints

### Customers
- `GET /api/customers` - Get all customers
- `GET /api/customers/{id}` - Get customer by ID
- `POST /api/customers` - Create new customer
- `PUT /api/customers/{id}` - Update customer
- `DELETE /api/customers/{id}` - Delete customer

### Products
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product

### Invoices
- `GET /api/invoices` - Get all invoices
- `GET /api/invoices/{id}` - Get invoice by ID
- `POST /api/invoices` - Create new invoice
- `PUT /api/invoices/{id}` - Update invoice
- `DELETE /api/invoices/{id}` - Delete invoice
- `POST /api/invoices/{id}/generate-pdf` - Generate PDF and send by email (background job)

### Example Request

**Create Invoice:**
```json
POST /api/invoices
Content-Type: application/json

{
  "invoiceNumber": "INV-2024-001",
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "issueDate": "2024-01-15T00:00:00Z",
  "dueDate": "2024-02-15T00:00:00Z",
  "status": "Draft",
  "lines": [
    {
      "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa7",
      "quantity": 2,
      "unitPrice": 99.99,
      "taxRate": 0.21
    }
  ]
}
```

## ?? Background Jobs

The application uses **Hangfire** for background job processing, particularly for PDF generation and email sending.

### Hangfire Dashboard

Access the dashboard at `/hangfire` to monitor:
- Enqueued jobs
- Processing jobs
- Succeeded jobs
- Failed jobs
- Recurring jobs

### Job Configuration

Hangfire is configured in `Program.cs`:
- **Storage**: SQL Server (same database as the application)
- **Dashboard**: Available at `/hangfire`
- **Automatic Retry**: Failed jobs are automatically retried
- **Job Timeout**: 5 minutes

## ?? Database

### Database Schema

The application uses **Entity Framework Core** with a code-first approach.

**Main Entities:**
- **Customer**: Customer information (name, VAT number, email, address)
- **Product**: Product catalog (name, description, unit price, tax rate)
- **Invoice**: Invoice header (invoice number, dates, status)
- **InvoiceLine**: Invoice line items (quantity, unit price, tax rate)

### Migrations

Create a new migration:
```bash
dotnet ef migrations add MigrationName --project Invoyz.Invoices.Data --startup-project Invoyz.Invoices.Api
```

Update database:
```bash
dotnet ef database update --project Invoyz.Invoices.Api
```

## ?? Testing

The project includes comprehensive unit tests using **xUnit**, **FluentAssertions**, and **Bogus**.

### Run Tests

```bash
dotnet test
```

### Test Coverage

Tests are organized by service:
- `CustomerServiceTests.cs` - Customer service tests
- `ProductServiceTests.cs` - Product service tests
- `InvoiceServiceTests.cs` - Invoice service tests
- `InvoiceLineServiceTests.cs` - Invoice line service tests
- `PdfGeneratorServiceTests.cs` - PDF generation tests

### Example Test

```csharp
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
    var pdfHeader = System.Text.Encoding.ASCII.GetString(result.Take(4).ToArray());
    pdfHeader.Should().Be("%PDF");
}
```

## ?? PDF Generation

The application uses **QuestPDF** library to generate professional invoice PDFs.

### Features

- Professional invoice layout
- Company and customer information
- Itemized line items with calculations
- Tax calculations
- Totals (subtotal, tax, grand total)
- Invoice status and dates

### Generated Files

PDFs are saved in the `PDFs` folder with the naming convention:
```
Invoice_{invoiceId}_{timestamp}.pdf
```

### PDF Content

Each invoice PDF includes:
- Invoice number and dates
- Customer details (name, VAT number, email, address)
- Invoice status
- Line items table (product, description, quantity, unit price, tax, total)
- Summary (subtotal, total tax, grand total)

## ??? Architecture

### Design Patterns

- **Repository Pattern**: Generic repository for data access
- **Service Layer Pattern**: Business logic separation
- **Dependency Injection**: Built-in .NET DI container
- **DTO Pattern**: Data Transfer Objects for API communication

### Key Services

- **CustomerService**: Customer management operations
- **ProductService**: Product catalog management
- **InvoiceService**: Invoice lifecycle management
- **InvoiceLineService**: Invoice line item operations
- **PdfGeneratorService**: PDF document generation
- **FileUtility**: File system operations

## ?? Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InvoyzDb;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Environment-specific Configuration

Create additional appsettings files:
- `appsettings.Development.json` - Development settings
- `appsettings.Production.json` - Production settings

## ?? Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Code Style

- Follow C# coding conventions
- Write unit tests for new features
- Update documentation as needed
- Ensure all tests pass before submitting PR

## ?? Contact

**Eliaquim Mauricio**
- GitHub: [@eliaquimmauricio](https://github.com/eliaquimmauricio)
- Repository: [invoyz-technical-challenge](https://github.com/eliaquimmauricio/invoyz-technical-challenge)

## ?? License

This project is part of a technical challenge for Invoyz.

## ?? Acknowledgments

- QuestPDF for excellent PDF generation capabilities
- Hangfire for robust background job processing
- The .NET team for the amazing framework

---

**Built with ?? using .NET 10**
