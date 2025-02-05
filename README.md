# Task Management System API

This is the API for a task management system (TMS) designed for redAcademy. The goal of this TMS is to improve visibility into task progression and streamline task management.

## Task Details
Each task has multiple properties that help provide context and tracking:

- **Title**
- **Description**
- **Category**
- **Status**
- **Start Date**
- **Due Date**
- **Assigned To**
- **Assigned By**
- **Difficulty**
- **Estimated Time**
- **Actual Time**
- **Percent Complete**
- **Feedback**

## Tech Stack

- **API:** C# Minimal API  
- **Authentication:** Firebase Auth  
- **Database:** SQL Server  
- **ORM:** Entity Framework
## API Endpoints

### Task Endpoints
- **GET** `/task` – Retrieve all tasks
- **GET** `/task/{id}` – Retrieve a task by ID
- **POST** `/task` – Create a new task
- **PUT** `/task/{id}` – Update an existing task
- **DELETE** `/task/{id}` – Delete a task

### User Endpoints
- **GET** `/user` – Retrieve all users
- **GET** `/user/{id}` – Retrieve a user by ID
- **POST** `/user` – Create a new user
- **PUT** `/user/{id}` – Update an existing user
- **DELETE** `/user/{id}` – Delete a user

### Future updates

- Roles based CRUD operations were added in a second iteration of this project. Roles were created for "students" and "lectures"

- The minimal api was refactored into a controller based api

- Data normalization was implemented to handle many-to-many relationships


#### Contributing

This version of the TMS was a group project and is no longer maintained. Please feel free to clone the project and adjust the code as required
