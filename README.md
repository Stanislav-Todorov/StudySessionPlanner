# StudySessionPlanner

StudySessionPlanner is an ASP.NET Core MVC web application for managing study topics, organizing study sessions, and enabling user interaction through enrollment and feedback.
Users can browse topics, join study sessions, and leave feedback, while administrators have access to a dedicated management area.

## Features

* Topics management (CRUD)
* Study sessions management (CRUD)
* User enrollment in study sessions
* Feedback system (comments and ratings per session)
* Each study session is linked to a topic (dropdown selection)
* Role-based authentication and authorization (Admin/User)
* Admin area for privileged access
* Search and filtering of study sessions by title and topic
* Server-side and client-side validation using DataAnnotations
* Default topics and administrator account are seeded on startup
* Custom error pages (404 and 500)

## Tech Stack

* ASP.NET Core MVC (.NET 8)
* Entity Framework Core
* SQL Server (LocalDB)
* ASP.NET Core Identity

## Data Model

Entities:

* **Topic**

  * One Topic can have many StudySessions
* **StudySession**

  * Belongs to one Topic
  * Has many Enrollments
  * Has many Feedback entries
* **Enrollment**

  * Links a User to a StudySession
* **Feedback**

  * Belongs to a User and a StudySession
  * Contains rating and comment
* **ApplicationUser**

  * Extends IdentityUser
  * Participates in Enrollments and Feedback

## Getting Started

### Prerequisites

* Visual Studio 2022
* .NET 8 SDK
* SQL Server LocalDB

### Setup & Run

1. Clone the repository.
2. Open the solution in Visual Studio 2022.
3. Apply migrations to create/update the database: "Update-Database".
4. Run the project.

* The application uses the DefaultConnection connection string from appsettings.json and creates a LocalDB database on your machine.

### Default Administrator Account

* Email: admin@studysessionplanner.com
* Password: Admin123!

## Unit Tests
* Unit tests are implemented using xUnit.
* The service layer is covered with tests, including:
  - EnrollmentService
  - FeedbackService
  - StudySessionService
* An in-memory database (EF Core InMemory) is used for testing.

## Design Decisions

* **Identifiers**

  * Entity identifiers use `int` primary keys instead of `Guid`. This decision was made to keep the data model and routing simple and readable during development, while acknowledging security considerations.

* **Architecture**

  * A service layer is introduced to separate business logic from controllers. This improves maintainability, testability, and aligns with best practices for ASP.NET Core applications.

* **Data Loading**

  * Lazy loading is not used. Related data is loaded explicitly using Entity Framework Core to maintain clarity and control over queries.

* **Database Choice**

  * SQL Server LocalDB is used for development. It requires no external configuration and is suitable for local environments.

* **Authentication and Authorization**

  * ASP.NET Core Identity is used for user management.
  * Role-based authorization is implemented with Administrator and User roles.
  * An admin area is introduced to separate privileged functionality from public features.

* **User Interaction Design**

  * Enrollment and feedback systems are implemented to provide real user interaction.
  * Duplicate enrollments and feedback submissions are prevented through business logic in the service layer.

* **Error Handling**

  * Custom 404 and 500 error pages are implemented to improve user experience.
