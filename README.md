# To-Do Task Management App

## Objective

This project is a simple, pragmatic to-do task management solution with a .NET 8 Web API backend and a Vue.js frontend. 

## Features

- **Backend:** .NET 8 Web API with Entity Framework Core and SQLite for persistent storage.
- **Frontend:** Vue.js SPA (Single Page Application).
- **API Endpoints:**  
  - Create, retrieve (by ID and all), update status, and delete tasks.
- **MVP Features:**  
  - Task creation, status update, deletion, and retrieval.
  - Clear separation of concerns and RESTful API design.

 ## Installation & Setup

### Backend (.NET 8 API)
- cd task-manager/taskapi
- dotnet restore
- dotnet ef database update
- dotnet run

  ## Testing
  - Uses xUnit and SQLite in-memory for integration tests.
  - To run tests:
    - cd task-manager/taskapi.tests
    - dotnet test
   
### Frontend (Vue.js)
- cd task-manager/client/task-client
- npm install
- npm run dev

  ## Testing
  - Unit tests use Vitest and Vue Test Utils.
  - To run tests:
    - cd task-manager/client/task-client
    - npm run test


## Assumptions & Trade-offs
- Used SQLite for persistence; can be swapped for another provider if needed.
-	This project is intended as a demo and for development/testing purposes only.
-	The SQLite database file (tasks.db) is stored in the project directory for convenience; this is not recommended for production.
-	In a production environment, the database should be stored in a secure, persistent location outside the application directory, and a more robust DBMS may be preferred for scalability and reliability.
-	The database file should not be checked into source control for production use.
- No authentication for simplicity; not recommended for production.
- For local development, the backend and frontend are assumed to run on separate servers (e.g., backend on localhost:7049, frontend on localhost:8080), which is why CORS is enabled for http://localhost:8080.
- In a production environment, the backend would typically run on a dedicated server or cloud service, and the frontend would be served to clients (e.g., via a CDN or web server). CORS should be configured according to the actual deployment setup.
- This CORS policy is for development/debugging only and should be reviewed before production deployment.

## API Endpoints
  -	POST   /api/task/createtask — Create a new task
  -	GET    /api/task/gettaskbyid/{id} — Get a task by ID
  -	GET    /api/task/getalltasks — Get all tasks
  -	PATCH  /api/task/updatetaskstatus/{id} — Update task completion status
  -	DELETE /api/task/deletetask/{id} — Delete a task


## Scalability & Future Improvements
  -	Add user authentication and authorization.
  -	Implement pagination and filtering for large task lists.
  -	Improve error handling and validation.
  -	Containerize with Docker for easier deployment.
  -	Enhance UI/UX and accessibility.
