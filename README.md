# TodoApp Backend

REST API for Todo application built with ASP.NET Core Web API.

## Features

* JWT Authentication
* Task CRUD
* Categories
* Pagination
* Search & Filtering
* FluentValidation
* Global Exception Handling

---

## Tech Stack

* ASP.NET Core 8
* Entity Framework Core
* SQL Server
* JWT
* FluentValidation
* Swagger

---

## Architecture

TodoApp
├── TodoApp         → API
├── TodoApp.BLL     → Business Logic
└── TodoApp.DAL     → Data Access

---

## Run Project

dotnet ef database update
dotnet run

Swagger:

https://localhost:7148/swagger

---

## API

### Auth

POST /api/auth/register
POST /api/auth/login

### Categories

GET /api/categories
POST /api/categories

### Tasks

GET    /api/tasks
GET    /api/tasks/{id}
POST   /api/tasks
PUT    /api/tasks/{id}
DELETE /api/tasks/{id}

---

## Examples

Pagination:

GET /api/tasks?page=1&pageSize=5

Search:

GET /api/tasks?search=asp

Filter:

GET /api/tasks?categoryId={guid}
