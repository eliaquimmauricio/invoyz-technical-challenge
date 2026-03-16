using Invoyz.Invoices.Data;
using Invoyz.Invoices.Domain.Dtos.Customers;
using Invoyz.Invoices.Domain.Dtos.InvoiceLines;
using Invoyz.Invoices.Domain.Dtos.Invoices;
using Invoyz.Invoices.Domain.Dtos.Products;
using Invoyz.Invoices.Domain.Entities;
using Invoyz.Invoices.Domain.Interfaces.Data;
using Invoyz.Invoices.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.SqlServer;
using Invoyz.Invoices.Domain.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<Context>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register DbContext as DbContext for Repository
builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<Context>());

// Register Repositories
builder.Services.AddScoped<IRepository<Customer, CustomerReadDto, CustomerWriteDto>, Repository<Customer, CustomerReadDto, CustomerWriteDto>>();
builder.Services.AddScoped<IRepository<Product, ProductReadDto, ProductWriteDto>, Repository<Product, ProductReadDto, ProductWriteDto>>();
builder.Services.AddScoped<IRepository<Invoice, InvoiceReadDto, InvoiceWriteDto>, Repository<Invoice, InvoiceReadDto, InvoiceWriteDto>>();
builder.Services.AddScoped<IRepository<InvoiceLine, InvoiceLineReadDto, InvoiceLineWriteDto>, Repository<InvoiceLine, InvoiceLineReadDto, InvoiceLineWriteDto>>();

// Register Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IInvoiceLineService, InvoiceLineService>();
builder.Services.AddScoped<IPdfGeneratorService, PdfGeneratorService>();

//Register utility services
builder.Services.AddScoped<IFileUtility, FileUtility>();

// Configure HangFire
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

builder.Services.AddHangfireServer();

builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Invoyz Invoices API",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Invoyz Invoices API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHangfireDashboard("/hangfire");

app.MapControllers();

app.Run();
