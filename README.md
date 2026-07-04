# 🎓 EduPulse Portal

A full-stack school management system built to model real-world architecture used in production ASP.NET Core + React applications — role-based access control, real-time notifications, file uploads, and weighted progress tracking, all built from scratch as a hands-on learning and portfolio project.

![.NET](https://img.shields.io/badge/.NET-ASP.NET%20Core-512BD4?logo=dotnet)
![React](https://img.shields.io/badge/React-Vite-61DAFB?logo=react)
![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-CC2927?logo=microsoftsqlserver)
![License](https://img.shields.io/badge/License-MIT-green)

---

## 📖 Overview

EduPulse Portal is a role-based school management platform supporting **Admin**, **Teacher**, and **Student** roles across 50+ secured API endpoints. It covers the full lifecycle of a school's daily operations: enrollment, attendance, homework distribution with file uploads, real-time notifications, and weighted academic progress tracking.

The project was built to deepen practical understanding of full-stack architecture — not just to produce a working app, but to deliberately practice the same layered patterns (Entity → DTO → Service → Controller, and API → Redux slice → Component → Page) used in professional codebases.

> 💡 **Why this matters for reviewers:** Every architectural decision in this project — from DTO mapping to Redux slice structure — was made intentionally, with trade-offs considered. See the [Key Technical Decisions](#-key-technical-decisions--learnings) section below for the reasoning behind them.

---

## ✨ Features

| Module | Capabilities |
|---|---|
| **Authentication** | JWT-based login/register, role-based access control (Admin / Teacher / Student), profile image embedded as a JWT claim |
| **Students, Teachers, Classes, Subjects, Departments, Courses** | Full CRUD with pagination, statistics panels, and capacity enforcement on class enrollment |
| **Homework** | File upload/download (PDF, Word, PowerPoint, images), teacher/class/subject scoping, status tracking (Active/Archived) |
| **Attendance** | Daily attendance logging, duplicate-entry prevention, automated consecutive-absence alert detection |
| **Progress Tracking** | Weighted scoring model — Homework 40% · Attendance 30% · Exams 30% |
| **Real-Time Notifications** | SignalR-powered live alerts for admins/teachers/students (new homework, absence warnings, subject assignments) |
| **AI Chatbot** | Google Gemini–powered assistant proxied securely through the backend — never exposes the API key to the client |
| **Class–Subject Management** | Many-to-many assignment interface between classes and subjects |

---

## 🏗️ Architecture

### Backend layering (ASP.NET Core)

```
Entity  →  EF Core Configuration  →  DTO  →  IService interface  →  Service  →  Controller
```

**Why this layering?**
- **Entity** — the raw database shape (what SQL Server actually stores).
- **EF Core Configuration** — keeps entity-relationship rules (keys, constraints, seed data) out of the entity class itself, so the entity stays a plain data model (Single Responsibility Principle).
- **DTO (Data Transfer Object)** — controls exactly what shape of data crosses the network boundary. This prevents over-exposing internal fields (e.g., password hashes) and decouples your API contract from your database schema — you can change a column name without breaking every frontend consumer.
- **Interface + Service** — business logic lives in a testable, swappable unit. Controllers stay "thin" — they only handle HTTP concerns (status codes, routing), never business rules.
- **Controller** — the thinnest possible layer: receives the HTTP request, delegates to the service, returns the HTTP response.

**Alternative approach:** Smaller teams sometimes skip DTOs and return entities directly (fewer files, faster to write). **Trade-off:** you lose the network-contract boundary — any change to your database schema immediately breaks your frontend, and you risk leaking sensitive fields by accident. For a project explicitly built to demonstrate professional patterns, the extra DTO layer is worth the boilerplate.

### Frontend layering (React + Redux Toolkit)

```
API file (axiosInstance)  →  Redux slice (async thunks + selectors)  →  Component  →  Page
```

**Why this layering?**
- **API file** — the only place that knows actual endpoint URLs. If a route changes, you fix it in one file, not in every component.
- **Redux slice** — owns the shared state and the async logic (loading/error/success states) via `createAsyncThunk`. Components never call `axios` directly.
- **Component / Page** — purely presentational; reads from Redux via selectors and dispatches actions. No business logic, no API calls.

**Alternative approach:** For smaller apps, `useState` + direct API calls inside components (no Redux) is simpler and faster to build. **Trade-off:** state stops being shareable across components without prop-drilling, and every component duplicates its own loading/error handling — Redux earns its complexity once state needs to be shared (e.g., notifications shown in both the Navbar and a dedicated page).

---

## 🛠️ Tech Stack

**Backend**
- ASP.NET Core Web API
- Entity Framework Core (Code-First, SQL Server)
- JWT Authentication & Role-Based Authorization
- SignalR (real-time notifications)
- Google Gemini API (AI chatbot, proxied server-side)

**Frontend**
- React (Vite)
- Redux Toolkit (state management, async thunks)
- Tailwind CSS
- Axios

**Database**
- Microsoft SQL Server

---

## 🚀 Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (8.0 or later)
- [Node.js](https://nodejs.org/) (18.x or later) & npm
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB or full instance)
- A Google Gemini API key (for the AI chatbot feature)

### 1. Clone the repository

```bash
git clone https://github.com/<your-username>/edupulse-portal.git
cd edupulse-portal
```

### 2. Backend setup

```bash
cd backend
```

Create your local config file from the provided template:

```bash
cp appsettings.Example.json appsettings.json
```

Fill in `appsettings.json` with your own values:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=EduPulseDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "REPLACE_WITH_A_LONG_RANDOM_SECRET_KEY",
    "Issuer": "EduPulseAPI",
    "Audience": "EduPulseAPIUsers",
    "ExpirationMinutes": "60"
  },
  "Gemini": {
    "ApiKey": "YOUR_GEMINI_API_KEY"
  }
}
```

> ⚠️ **Never commit `appsettings.json`.** It's excluded via `.gitignore` on purpose — it holds your JWT signing key and database credentials. Only `appsettings.Example.json` (with placeholder values) is tracked in Git.

Apply database migrations and seed data:

```bash
dotnet ef database update
```

Run the API:

```bash
dotnet run
```

The API will be available at `https://localhost:7001`, with Swagger UI at `https://localhost:7001/swagger/index.html`.

### 3. Frontend setup

```bash
cd ../frontend
npm install
```

Create your local environment file:

```bash
cp .env.example .env
```

```env
VITE_API_URL=https://localhost:7001/api
VITE_HUB_URL=https://localhost:7001/hubs/notifications
```

Run the dev server:

```bash
npm run dev
```

The app will be available at `http://localhost:5173`.

### 4. Default seeded accounts

| Role | Email | Password |
|---|---|---|
| Admin | admin@gmail.com | Admin@123 |
| Teacher | teacher1@gmail.com | Teacher@123 |
| Student | student1@gmail.com | Student@123 |

---

## 📁 Project Structure

```
edupulse-portal/
├── backend/
│   ├── Controllers/          # Thin HTTP layer — routes requests to services
│   ├── Services/             # Business logic (implements interfaces)
│   ├── Services/Interfaces/  # Contracts consumed via dependency injection
│   ├── DTO/                  # Request/response shapes exposed to the client
│   ├── Entities/             # EF Core models (database shape)
│   ├── Data/Config/          # IEntityTypeConfiguration<T> — relationships & seed data
│   ├── Hubs/                 # SignalR hubs (real-time notifications)
│   └── appsettings.Example.json
│
└── frontend/
    ├── src/
    │   ├── api/               # Axios calls — the only files that know endpoint URLs
    │   ├── features/          # Redux slices (one per domain: students, homework, etc.)
    │   ├── components/        # Reusable presentational UI
    │   ├── pages/              # Route-level views composed from components
    │   └── utils/              # Shared helpers (e.g., JWT decoding)
    └── .env.example
```

---

## 🧠 Key Technical Decisions & Learnings

A few patterns worth highlighting for anyone reviewing this code:

- **EF Core `.Select()` runs in SQL, not C#.** Methods like `Path.GetFileName()` can't execute inside a LINQ `.Select()` projection because EF Core translates it to a SQL query — SQL Server has no concept of a C# method. The fix: materialize the query first with `.ToListAsync()`, then apply C# transformations in a `foreach` loop afterward.

- **Frontend RBAC is UX only.** Hiding a button for non-admins in React only improves the experience — it provides zero real security, since any API endpoint can be called directly with a valid token via Postman or curl. Every sensitive endpoint is enforced with `[Authorize(Roles = "Admin")]` server-side, which is the actual security boundary.

- **`HasDefaultValueSql` vs. `HasDefaultValue`.** The former passes a raw SQL expression (e.g., `GETUTCDATE()`) evaluated by the database; the latter treats the value as a literal, which silently breaks for anything that isn't a fixed constant.

- **`SetNull` cascade behavior needs two changes, not one.** Making a foreign key nullable requires both the C# property being nullable (`int?`) *and* a corresponding EF Core migration — updating only the C# model without migrating the actual database column leaves the old `NOT NULL` constraint in place.

- **SignalR + React 18 Strict Mode.** Strict Mode intentionally double-invokes `useEffect` in development, which can trigger rapid connect/disconnect cycles on a SignalR connection. Guarding the connection lifecycle with a `useRef` (instead of relying purely on the effect cleanup) prevents this loop.

- **Redux store keys must match selectors exactly.** A mismatch between the key registered in `configureStore` (e.g., `departments`) and a selector referencing `state.departments` fails **silently** — no runtime error, just `undefined` state. This is why every slice in this project follows one consistent naming convention.

---

## 🗺️ Roadmap

- [ ] Resource-based authorization layer (e.g., a Teacher can only view/edit their *own* homework — not just role-gated, but ownership-gated)
- [ ] Extract repeated ownership-check logic into a custom `IAuthorizationHandler` once the pattern appears across 3+ controllers
- [ ] Migrate nullable `UserId` foreign keys on `Teacher`/`Student` to required, once registration flows fully populate them
- [ ] Add a direct Student ↔ Class relationship to support class-scoped homework filtering from the student's perspective

---

## 📄 License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.

---

## 👤 Author

**Mohammad Khaled Shiplak**
Built as a full-stack learning project to practice production-grade architecture patterns in ASP.NET Core and React.
