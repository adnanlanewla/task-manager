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
   


### Frontend (Vue.js)
 - cd task-manager/client/task-client
 - npm install
 - npm run dev


## Assumptions & Trade-offs

- Used SQLite for persistence; can be swapped for another provider if needed.
- No authentication for simplicity; not recommended for production.

## API Endpoints
  •	POST   /api/task/createtask — Create a new task
  •	GET    /api/task/gettaskbyid/{id} — Get a task by ID
  •	GET    /api/task/getalltasks — Get all tasks
  •	PATCH  /api/task/updatetaskstatus/{id} — Update task completion status
  •	DELETE /api/task/deletetask/{id} — Delete a task


## Scalability & Future Improvements
  •	Add user authentication and authorization.
  •	Implement pagination and filtering for large task lists.
  •	Improve error handling and validation.
  •	Containerize with Docker for easier deployment.
  •	Enhance UI/UX and accessibility.
