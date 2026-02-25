# ExpenseTracker API

A simple backend **Expense Tracker API** built with **ASP.NET Core (.NET 9)** using **Clean Architecture** and common backend patterns. The project focuses on clean design, background processing, and real infrastructure setup.

---

## 🛠 Tech Stack

* **ASP.NET Core (.NET 9)**
* **Entity Framework Core**
* **PostgreSQL** (database)
* **Redis** (caching)
* **Docker & Docker Compose**

---

## 🧱 Architecture & Patterns

The project follows **Clean Architecture** with clear separation of concerns:

* **Core** – domain entities
* **Application** – DTOs, services, specifications
* **Infrastructure** – EF Core, repositories, Redis, background workers
* **API** – controllers and endpoints

Patterns used:

* **Generic Repository Pattern**
* **Specification Pattern** (filtering, sorting, pagination)
* **Background Worker (BackgroundService)** for async jobs
* **Cache-aside pattern** with Redis

---

## ✨ Main Features

* CRUD for users, categories, and expenses
* Expense filtering, sorting, and pagination
* Monthly category budget tracking
* Expense breakdown by category
* CSV bulk import for expenses
* Background report generation (async, non-blocking)
* Redis caching for read-heavy endpoints

---

## 🐳 Project Setup

### 1️⃣ Start infrastructure (PostgreSQL + Redis)

```bash
docker compose up -d
```

---

### 2️⃣ Apply database migrations

```powershell
Update-Database
```

---

### 3️⃣ Run the API

```bash
dotnet run
```

Open API UI:

```
https://localhost:{PORT}/scalar
```

---

## 📊 Background Jobs

* Report jobs are stored in the database with status:

  * `Pending → Processing → Completed`
* A background worker processes jobs asynchronously
* Generates `.txt` report files under:

```
ExpenseTrackerApi/Reports/
```

---

## ⚡ Redis Caching

* Used to cache frequently requested data (e.g. expense lists)
* Reduces database load
* Improves response time

---

## 📝 Notes

* Authentication is intentionally simple (no JWT)
* Easily extendable with auth, emails, or more caching

---

Built as a hands-on backend engineering project.
