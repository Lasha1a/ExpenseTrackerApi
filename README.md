# ExpenseTracker API

A backend **Expense Tracker API** built with **ASP.NET Core (.NET 9)** using **Clean Architecture**. This project was built as a hands-on backend engineering exercise covering everything from clean architecture patterns to real infrastructure like caching, background jobs, email delivery, and authentication.

---

## 🛠 Tech Stack

* **ASP.NET Core (.NET 9)**
* **Entity Framework Core**
* **PostgreSQL** — primary database
* **Redis** — caching layer
* **MailKit** — SMTP email sending
* **FluentValidation** — request validation
* **JWT Bearer** — authentication
* **Docker & Docker Compose** — infrastructure setup

---

## 🧱 Architecture & Patterns

The project follows **Clean Architecture** with clear separation of concerns across four layers:

* **Core** — domain entities and base classes
* **Application** — DTOs, services, interfaces, specifications, and validators
* **Infrastructure** — EF Core, repositories, Redis, background workers, email, and JWT
* **API** — controllers, endpoints, and dependency wiring

Patterns used:

* **Generic Repository Pattern**
* **Specification Pattern** (filtering, sorting, pagination)
* **Background Worker (BackgroundService)** for async jobs
* **Cache-aside pattern** with Redis
* **Soft Delete** — records are deactivated, not physically removed

---

## ✨ Features

* Full CRUD for users, categories, and expenses
* Soft delete on categories with proper filtering
* Expense filtering and pagination by category and page number
* Monthly category budget tracking and status
* Expense breakdown analytics by category
* Background report generation — async and non-blocking
* Background budget alert worker — runs every 10 minutes, triggers alerts at 80%+ budget usage
* Redis caching with cache invalidation on updates and deletes
* Email delivery via Gmail SMTP — reports sent directly to user's registered email
* JWT authentication — protected endpoints require a valid Bearer token
* FluentValidation on all request DTOs — clean 400 responses with readable error messages

---

## 🐳 Project Setup

### 1️⃣ Start infrastructure (PostgreSQL + Redis)

```bash
docker compose up -d
```

### 2️⃣ Apply database migrations

**Using Package Manager Console (Visual Studio):**
```powershell
Update-Database
```

**Using .NET CLI:**
```bash
dotnet ef database update --project ExpenseTracker.Infrastructure --startup-project ExpenseTrackerApi
```

### 3️⃣ Configure settings

Make sure your `appsettings.json` has the following sections filled in:

```json
"ConnectionStrings": {
  "DefaultConnection": "your-postgres-connection-string"
},
"Redis": {
  "ConnectionString": "localhost:6379",
  "InstanceName": "ExpenseTracker:"
},
"JwtSettings": {
  "SecretKey": "your-secret-key-min-32-characters",
  "Issuer": "ExpenseTrackerApi",
  "Audience": "ExpenseTrackerApi",
  "ExpiryMinutes": 60
},
"EmailSettings": {
  "Host": "smtp.gmail.com",
  "Port": 587,
  "SenderEmail": "youremail@gmail.com",
  "SenderName": "Expense Tracker",
  "AppPassword": "your-gmail-app-password"
}
```

> For Gmail SMTP, generate an **App Password** from your Google Account → Security → 2-Step Verification → App Passwords.

### 4️⃣ Run the API

```bash
dotnet run --project ExpenseTrackerApi
```

Open API UI:
```
https://localhost:{PORT}/scalar
```

---

## 🔐 Authentication

All endpoints except `/api/User` (register/login) are protected with JWT.

1. Register a user via `POST /api/User`
2. Login via `POST /api/User/login` — you'll receive an `AccessToken`
3. Add the token to the `Authorization` header as `Bearer <token>` on all subsequent requests

---

## 📊 Background Jobs

Two background workers run automatically:

**ReportJobWorker** — picks up pending report jobs every minute:
* `Pending → Processing → Completed`
* Fetches the user's email from the database
* Sends the report as an email via Gmail SMTP

**BudgetAlertWorker** — runs every 10 minutes:
* Checks all active categories with a defined monthly budget
* If spending reaches 80% or more of the budget, a `BudgetAlert` record is created in the database

---

## ⚡ Redis Caching

* `GetById` responses for expenses and categories are cached for 5 minutes
* Cache is invalidated automatically on update and delete operations
* Reduces database load on read-heavy endpoints

---

## ✅ Validation

All request DTOs are validated using **FluentValidation**. Invalid requests return `400 Bad Request` with clear error messages before reaching the controller or service layer. Examples:

* `ColorHex` must match `#RRGGBB` format
* `ExpenseDate` cannot be in the future
* `Month` must be between 1 and 12
* `Year` must be between 2000 and the current year

---

Built as a hands-on backend engineering project.
