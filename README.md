# Expense Tracker API

Personal expense tracking API built with ASP.NET Core, Entity Framework Core and SQLite.

---

## Features

- Create and manage expense categories
- Create and manage expenses
- Filter expenses by month and year
- Monthly dashboard analytics
- Input validation with FluentValidation
- Swagger/OpenAPI documentation
- SQLite database with EF Core migrations

---

## Tech Stack

- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- FluentValidation
- Swagger / OpenAPI
- C#

---

## Project Structure

```text
src/
 ├── ExpenseTracker.Api
 ├── ExpenseTracker.Application
 ├── ExpenseTracker.Domain
 └── ExpenseTracker.Infrastructure
```

---

## Getting Started

### Clone the repository

```bash
git clone https://github.com/YOUR_USERNAME/expense-tracker-api.git
```

### Navigate to the project

```bash
cd expense-tracker-api
```

### Restore dependencies

```bash
dotnet restore
```

### Apply migrations

```bash
dotnet ef database update \
--project src/ExpenseTracker.Infrastructure \
--startup-project src/ExpenseTracker.Api
```

### Run the API

```bash
dotnet run --project src/ExpenseTracker.Api
```

---

## Swagger

After running the application, open:

```text
http://localhost:5179/swagger
```

---

## Example Endpoints

### Create Category

```http
POST /api/categories
```

Request body:

```json
{
  "name": "Food"
}
```

---

### Create Expense

```http
POST /api/expenses
```

Request body:

```json
{
  "description": "Lunch",
  "amount": 12.50,
  "date": "2026-05-13",
  "categoryId": "CATEGORY_ID"
}
```

---

### Get Expenses

```http
GET /api/expenses
```

### Filter Expenses

```http
GET /api/expenses?month=5&year=2026
```

---

### Monthly Dashboard

```http
GET /api/dashboard/monthly?month=5&year=2026
```

---

## Validation

The API uses FluentValidation for request validation.

Examples:
- Category name is required
- Expense amount must be greater than zero
- Description has maximum length validation

---

## Future Improvements

- JWT authentication
- Docker support
- Unit tests
- Pagination
- Global exception handling
- AutoMapper
- CI/CD pipeline
- PostgreSQL support

---