# Task Management System

A comprehensive C# console application for managing tasks with priority levels, due dates, and status tracking. Built to demonstrate object-oriented programming principle and LINQ operations.
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-blue?style=for-the-badge)

## 🚀 Features

- **Task Creation & Management**: Create, update, and delete tasks with detailed information
- **Priority System**: Organize tasks by priority levels (Low, Medium, High, Critical)
- **Status Tracking**: Track task progress (Pending, In Progress, Completed, Cancelled)
- **Due Date Management**: Set due dates and automatically detect overdue tasks
- **Advanced Filtering**: View tasks by status, priority, or overdue status
- **Task Analytics**: View completion rates and task summaries
- **Interactive Console UI**: User-friendly menu-driven interface

## 🛠️ Technologies Used

- **Language**: C# 
- **Framework**: .NET (Console Application)
- **Key Concepts**: 
  - Object-Oriented Programming
  - LINQ Queries
  - Exception Handling
  - Generic Collections
  - DateTime Manipulation

## 📋 Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (6.0 or later)
- Any C# IDE:
  - [Visual Studio](https://visualstudio.microsoft.com/) (recommended)
  - [Visual Studio Code](https://code.visualstudio.com/) with C# extension
  - [JetBrains Rider](https://www.jetbrains.com/rider/)

## 🔧 Installation & Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/umutulay/task_management_system.git
   cd task_management_system/TaskManagementSystem/
   ```

2. **Build the project**
   ```bash
   dotnet build
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

## 🎮 Usage

When you run the application, you'll be greeted with an interactive menu:

```
=== Task Management System ===

=== MAIN MENU ===
1. Create New Task
2. View All Tasks
3. View Tasks by Status
4. Mark Task as Completed
5. View Overdue Tasks
6. View Tasks by Priority
7. View Task Summary
8. Delete Task
Q. Quit

Select an option:
```

### Creating a New Task
```
Enter task title: Implement User Authentication
Enter task description: Add login/logout functionality with JWT tokens
Enter priority (1=Low, 2=Medium, 3=High, 4=Critical) [default: 2]: 4
Enter due date (yyyy-mm-dd) [optional]: 2024-12-31
```

### Sample Output
```
=== ALL TASKS ===
[1] Design Database Schema - Completed (High) - Due: 2024-09-07 
    Description: Create ERD and database design for the new project
    Created: 2024-09-09 10:30
    Completed: 2024-09-09 11:45

[2] Implement User Authentication - Pending (Critical) - Due: 2024-09-12
    Description: Add login/logout functionality with JWT tokens
    Created: 2024-09-09 10:30
```
