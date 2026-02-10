# StudySessionPlanner

StudySessionPlanner is an ASP.NET Core MVC web application for managing study topics and organizing study sessions.  
Users can create topics and then create study sessions linked to a selected topic.

## Features
- Topics management (CRUD)
- Study sessions management (CRUD)
- Each study session is linked to a topic (dropdown selection)
- Server-side and client-side validation using DataAnnotations
- Default topics are seeded on startup if the database contains no topics
- ASP.NET Core Identity included (Individual Accounts)

## Tech Stack
- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- SQL Server (LocalDB)
- ASP.NET Core Identity

## Data Model
Entities:
- **Topic**
  - One Topic can have many StudySessions
- **StudySession**
  - Belongs to one Topic
  - Has many Participants
- **Participant**
  - Belongs to one StudySession

## Getting Started

### Prerequisites
- Visual Studio 2022
- .NET 8 SDK
- SQL Server LocalDB

### Setup & Run
1. Clone the repository.
2. Open the solution in Visual Studio 2022.
3. Apply migrations to create/update the database: "Update-Database".
4. Run the project.
- The application uses the DefaultConnection connection string from appsettings.json and creates a LocalDB database on your machine.

## Design Decisions
- **Identifiers**
  - Entity identifiers use `int` primary keys instead of `Guid`. This decision was made to keep the data model and routing simple and readable during development, while acknowledging security considerations.
- **Data Loading**
  - Lazy loading is not used. Related data is loaded explicitly using Entity Framework Core.
- **Database Choice**
  - LocalDB requires no external configuration.
- **Authentication Setup**
  - ASP.NET Core Identity is included from the initial project setup to allow future extension.