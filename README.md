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


### Frontend (Vue.js)
 - cd \client\task-client
 - npm install
 - npm run dev


## Assumptions & Trade-offs

- Used SQLite for persistence; can be swapped for another provider if needed.
- No authentication for simplicity; not recommended for production.
