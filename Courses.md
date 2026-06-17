# **Course & Enrollment Management System**

This project is a high-performance, enterprise-grade Web API built using **.NET 10**, **Clean Architecture**, **CQRS (Command Query Responsibility Segregation)**, and **Entity Framework Core with PostgreSQL**. It features custom middleware/pipeline validations, cursor-based pagination, transactional database auditing, structured logging, and simulated header-based authorization.

## **🏗️ Architecture Choices**

This system strictly adheres to the principles of **Clean Architecture** to enforce boundary separation, maintainability, and testability.  
┌────────────────────────────────────────────────────────┐  
│                      Presentation                      │  
│                  (Controllers, HTTP)                   │  
└───────────────────────────┬────────────────────────────┘  
                            │  
┌───────────────────────────▼────────────────────────────┐  
│                      Application                       │  
│             (Commands, Queries, MediatR,               │  
│               FluentValidation, DTOs)                  │  
└───────────────────────────┬────────────────────────────┘  
                            │  
┌───────────────────────────▼────────────────────────────┐  
│                        Domain                          │  
│            (Entities, Value Objects, Enums)            │  
└───────────────────────────▲────────────────────────────┘  
                            │  
┌───────────────────────────┴────────────────────────────┐  
│                    Infrastructure                      │  
│           (PostgreSQL DB, Configurations,              │  
│                Filters, Auditing, Logs)                │  
└────────────────────────────────────────────────────────┘

### **1\. Layer Responsibilities**

* **Domain Layer:** Contains core enterprise business models (Course, Learner, Enrollment, AuditRecord), enums, and domain rules. It has zero external dependencies.  
* **Application Layer:** Contains MediatR Commands and Queries, Handlers, FluentValidation rules, and output DTOs.  
* **Infrastructure Layer:** Implements persistence (PostgreSQL DB Context, configurations), logging, and external services. This is also where HTTP action filters like authorization reside.  
* **Presentation/API Layer:** The outer web interface. Controllers are strictly thin "traffic controllers" that convert HTTP parameters into MediatR messages and return HTTP responses.

### **2\. Design Patterns Used**

* **CQRS with MediatR:** Separates write operations (Commands) from read operations (Queries). This prevents write-models and query-projections from polluting each other, allowing for optimized query performance (e.g., using .AsNoTracking() on queries).  
* **MediatR Pipeline Behaviors:** Cross-cutting concerns are modularized:  
  * ValidationBehavior\<TRequest, TResponse\>: Automatically executes FluentValidation checks before reaching the command handler.  
  * LoggingBehavior\<TRequest, TResponse\>: Captures structured trace data, request processing durations, and identity diagnostics.  
* **Keyset (Cursor-Based) Pagination:** Uses a compound sorting strategy (CreatedAt, Id) to achieve high-performance ![][image1] database sliding-window seeks, bypassing the costly ![][image2] overhead of traditional index-offset (Skip/Take) paging on large databases.  
* **Dual Auditing:** Combines transactional relational database audit tracking (stored in the AuditRecord table) for critical business mutations alongside structured application logs (via ILogger/Serilog) for search diagnostics.

## **💾 Database Setup**

The application uses **PostgreSQL** as its persistent store.

### **1\. Connection String Configuration**

Navigate to the Presentation layer (API/appsettings.json or equivalent) and configure your PostgreSQL connection string in the DefaultConnection block:  
{  
  "ConnectionStrings": {  
    "DefaultConnection": "Host=localhost;Database=CoursesDb;Username=postgres;Password=\<your-password\>

### **2\. Run Database Migrations**

Make sure you have the EF Core CLI tools installed:  
dotnet tool install \--global dotnet-ef

Generate the database schema and apply all EF Core configurations by applying the migrations to your target local/Docker PostgreSQL server:  
dotnet ef database update \--project Infrastructure \--startup-project Courses

## **🚀 How to Run the Project**

### **Prerequisites**

* **.NET 10 SDK** installed on your workstation.  
* Running instance of **PostgreSQL** (Docker container or local service).

### **Running via CLI**

1. Restore all project dependencies:  
   dotnet restore

2. Run the application pointing to the startup API project:  
   dotnet run \--project Courses

3. Open your browser and navigate to the local Swagger UI endpoint to inspect and test the interactive API definition:  
   http://localhost:5290/swagger/index.html

## **🚦 API Examples**

### **🔒 Required Headers for Simulated Authorization**

* **Create Courses:** Must provide header X-User-Role: Admin.  
* **Request Enrollment:** Must provide headers X-User-Role: Learner and X-User-Id: \<Guid\>.  
* **Make Enrollment Decision:** Must provide header X-User-Role: Manager.

### **Courses Endpoints**

#### **1\. Create Course (POST /api/Courses)**

* **Headers:**  
  X-User-Role: Admin  
  Content-Type: application/json

* **Request Payload:**  
  {  
    "title": "Domain-Driven Design Fundamentals",  
    "description": "Learn bounded contexts, aggregates, and value objects.",  
    "durationHours": 18,  
    "requiresApproval": true,  
    "isActive": true  
  }

* **cURL command:**  
  curl \-X POST http://localhost:5290/api/Courses \\  
    \-H "X-User-Role: Admin" \\  
    \-H "Content-Type: application/json" \\  
    \-d '{"title": "Domain-Driven Design Fundamentals", "description": "Learn bounded contexts, aggregates, and value objects.", "durationHours": 18, "requiresApproval": true, "isActive": true}'

#### **2\. Get Courses List (GET /api/Courses)**

* **Query Parameters:** Limit=2  
* **cURL command:**  
  curl \-X GET "http://localhost:5290/api/Courses?Limit=2"

* **Response Payload:**  
  {  
    "items": \[  
      {  
        "id": "e70d8923-b12e-423c-a982-f542918861c8",  
        "title": "Domain-Driven Design Fundamentals",  
        "description": "Learn bounded contexts, aggregates, and value objects.",  
        "durationHours": 18,  
        "requiresApproval": true,  
        "isActive": true  
      }  
    \],  
    "nextCursor": "eyJDcmVhdGVkQXQiOiIyMDI2LTA2LTE3VDExOjM1OjAwWiIsIkklZCI6ImU3MGQ4OTIzLWIxMmUtNDIzYy1hOTgyLWY1NDI5MTg4NjFjOCJ9"  
  }

### **Enrollments Endpoints**

#### **1\. Request Enrollment (POST /api/Enrollments)**

* **Headers:**  
  X-User-Role: Learner  
  X-User-Id: f9d0fcdb-cfae-418f-9bae-9e80b80866f9  
  Content-Type: application/json

* **Request Payload:**  
  {  
    "learnerId": "f9d0fcdb-cfae-418f-9bae-9e80b80866f9",  
    "courseId": "e70d8923-b12e-423c-a982-f542918861c8"  
  }

* **cURL command:**  
  curl \-X POST http://localhost:5290/api/Enrollments \\  
    \-H "X-User-Role: Learner" \\  
    \-H "X-User-Id: f9d0fcdb-cfae-418f-9bae-9e80b80866f9" \\  
    \-H "Content-Type: application/json" \\  
    \-d '{"learnerId": "f9d0fcdb-cfae-418f-9bae-9e80b80866f9", "courseId": "e70d8923-b12e-423c-a982-f542918861c8"}'

#### **2\. Make Approval Decision (POST /api/Enrollments/{EnrollmentId}/{Decision})**

* **Headers:**  
  X-User-Role: Manager  
  Content-Type: application/json

* **Parameters:**  
  * EnrollmentId: 2a8435d1-12a8-4211-bb03-dd914cd3b772  
  * Decision: Rejected  
* **Request Payload (Raw String Reason):**  
  "Required pre-requisite courses not completed."

* **cURL command:**  
  curl \-X POST "http://localhost:5290/api/Enrollments/2a8435d1-12a8-4211-bb03-dd914cd3b772/Rejected" \\  
    \-H "X-User-Role: Manager" \\  
    \-H "Content-Type: application/json" \\  
    \-d '"Required pre-requisite courses not completed."'

### **Learners Endpoints**

#### **1\. Register Profile (POST /api/Learners)**

* **Request Payload:**  
  {  
    "fullName": "Alice Smith",  
    "email": "alice.smith@university.edu",  
    "nationalId": "NAT-48920193",  
    "department": "Computer Science"  
  }

* **cURL command:**  
  curl \-X POST http://localhost:5290/api/Learners \\  
    \-H "Content-Type: application/json" \\  
    \-d '{"fullName": "Alice Smith", "email": "alice.smith@university.edu", "nationalId": "NAT-48920193", "department": "Computer Science"}'  


[image1]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACsAAAAaCAYAAAAue6XIAAABq0lEQVR4Xu2WvytHYRTGH5H8GJHsBoOB0eLXYJBVSTIYzMpfIIkB/4HBKouSkcGuyGRRFiVFKEp+nPN979Xb03vvOfRl0PdTz/Kc55733N773nuBGjV+hVs2DIZFM2z+BfeiFjYd7Ism2fTSJ3oTXYvWRXuiD9FBHCK2RJtsRrQi9ChCaw1sWlyJXtjM0IY3bGakBukQnSPUchUxJ7pjs4xX0TObEdMIC/aSvy06IY+xhlWs+he65Rqu4wKhmeOEN0ge4xn2XbTCJjOK0OiIC4Q+U5p7IN8aQvEMu4vyna2QN2rkAjGFkIu3vCfzLDzDzsPOuBoplwi52cjTd6TnWs8aAzAyXfA1UlK5hYSXInUtY+6Sbr0GHrlAbCDk1sh3bR18w/bDzpiN8hvig6WMofzaHGsNZQJ2BmcIodQBq0fxoEozHAvAN+wS7EyFJ4RgZ+QtZt5q5KXQTBObhGfYC9Ehm0UMiU4RvmT6mYxPfRmaX2YzIx+SlUL9bjarzTiKB/Bi/ehUFV2onc1voI+Avln+hDaEx+En6DmxXp1VZ0S0w6YD/YGpUePf8QmpEH9bVdzZrwAAAABJRU5ErkJggg==>

[image2]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADMAAAAaCAYAAAAaAmTUAAACNElEQVR4Xu2WO2tVQRSFlxpfaRXEX2AECaazUhS0ULAOElKKCJKINrHQwkIL9R9YaGFhp6AgSES0kvjE3kdjMBFDVNQEH3u553DnLueeGbzJBeF8sLj3rL1nzzlz5nGAhob/klk1uuSMabuavWDO1K/mEvDGtEXNUjgSP0zvTBdMN0y/TLfjJOGy6ZKaEU9Nr+F1KOURWrEF08P2cLJNlremb2oGWPC9moGSziZNd+C5ByRG1pruqxm4Cm9fzKLpq5oRh+A3sk38K6Yn4qVg2xXh96fEyEnTTjUDVbsiOKWqzupgzoOE1+kmYqq3+gXeZn0UIx/kWmGbvWoqe+CJ9zQg9MHz5sUvGbEdpqPh/yC8zVQr/IdcnRnTXTUVFqHWaEAYhufFU2ogeDluyXXVZwVnRG5NXENBX1q4E6/geaORNxK8HJpzOninwvUx5KfqBP6u08ZmlD9MKm884aVIHaZxvdx6IUeQ6YtTiwmfNCBchOedF/9w8OsYMo2paTyGt90afnMcR0FeasRjqgfWhU/2ob4tuWlaqaaxDq2BzK0XwgM81xdewJNSG8AqdH4Qwu0110FdnF8ajOfWC+GBOq1mis/wopsi70TwzkVeCuZwlFMchMdTb4aUvNkK5nHjKGKX6Tn8S+Al2netOph/VrzV8M5jpT5hyHc1OsAauUO9a/ajfHT/FQ40p2RP4MNsVHMJ4dvnrtcTNsA7XA74PfZMzeVmt+m6ml3CHfajmg0NDd3zG0JDlYBAXG1PAAAAAElFTkSuQmCC>