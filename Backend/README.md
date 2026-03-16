# 🧾 Invoyz — Invoice Management API

A modern **.NET 10 RESTful API** for managing invoices, customers, and products, with **PDF generation** and **background job processing**.

Invoyz provides a complete solution for:

- Managing **customers, products, and invoices**
- Generating **professional invoice PDFs**
- Processing tasks asynchronously with **background jobs**
- Monitoring jobs in real time using **Hangfire Dashboard**

---

# 🚀 Features

### 👤 Customer Management
Create, update, retrieve, and delete customer records.

### 📦 Product Management
Maintain a product catalog with pricing and tax information.

### 🧾 Invoice Management
Handle the full invoice lifecycle from creation to processing.

### 📄 PDF Generation
Generate professional invoice PDFs using **QuestPDF**.

### ⚙️ Background Processing
Process heavy operations asynchronously using **Hangfire**.

### 📊 Job Monitoring
Track background job execution through the **Hangfire Dashboard**.

### 🌐 RESTful API
Clean and well-structured REST endpoints.

### 🗄️ Database Integration
Code-first approach with **Entity Framework Core**.

### 📚 API Documentation
Interactive API documentation using **Swagger**.

---

# 🧰 Technology Stack

## 🖥️ Framework & Runtime

- **.NET 10**
- **C# 14**

## ⚙️ Core Components

- **ASP.NET Core** — Web API framework  
- **Entity Framework Core** — ORM for database access  
- **SQL Server** — Database engine  

## 📦 Third-Party Libraries

- **Hangfire** — Background job processing  
- **QuestPDF** — PDF document generation  
- **Swashbuckle (Swagger)** — API documentation  
- **xUnit** — Unit testing framework  
- **FluentAssertions** — Readable test assertions  
- **Bogus** — Fake data generation for tests  

---

# 🧑‍💻 Prerequisites

Before running the project, make sure you have:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) or a SQL Server instance
- **Visual Studio 2025** or **Visual Studio Code**

---

# ⚙️ Background Jobs

The system uses **Hangfire** to handle asynchronous tasks such as:

- PDF generation
- Email sending
- Long-running processes

This ensures that heavy operations do not block API responses.

---

# 📊 Hangfire Dashboard

You can monitor background jobs through the dashboard: /hangfire

The dashboard provides visibility into:

- 📥 Enqueued jobs
- ⚙️ Processing jobs
- ✅ Succeeded jobs
- ❌ Failed jobs
- 🔁 Recurring jobs
