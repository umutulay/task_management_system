using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagementSystem
{
    // Enum for task priorities
    public enum Priority
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4
    }

    // Enum for task status
    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed,
        Cancelled
    }

    // Task model class
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        public Task(int id, string title, string description, Priority priority = Priority.Medium, DateTime? dueDate = null)
        {
            Id = id;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Priority = priority;
            Status = TaskStatus.Pending;
            CreatedDate = DateTime.Now;
            DueDate = dueDate;
        }

        public void MarkAsCompleted()
        {
            Status = TaskStatus.Completed;
            CompletedDate = DateTime.Now;
        }

        public bool IsOverdue()
        {
            return DueDate.HasValue && DueDate < DateTime.Now && Status != TaskStatus.Completed;
        }

        public override string ToString()
        {
            var dueDateStr = DueDate?.ToString("yyyy-MM-dd") ?? "No due date";
            var overdueStr = IsOverdue() ? " [OVERDUE]" : "";
            return $"[{Id}] {Title} - {Status} ({Priority}) - Due: {dueDateStr}{overdueStr}";
        }
    }

    // Custom exception for task-related errors
    public class TaskManagementException : Exception
    {
        public TaskManagementException(string message) : base(message) { }
        public TaskManagementException(string message, Exception innerException) : base(message, innerException) { }
    }

    // Task Manager class - main business logic
    public class TaskManager
    {
        private readonly List<Task> _tasks;
        private int _nextId;

        public TaskManager()
        {
            _tasks = new List<Task>();
            _nextId = 1;
        }

        public Task CreateTask(string title, string description, Priority priority = Priority.Medium, DateTime? dueDate = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new TaskManagementException("Task title cannot be empty");

            if (string.IsNullOrWhiteSpace(description))
                throw new TaskManagementException("Task description cannot be empty");

            var task = new Task(_nextId++, title, description, priority, dueDate);
            _tasks.Add(task);
            return task;
        }

        public Task GetTaskById(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                throw new TaskManagementException($"Task with ID {id} not found");
            return task;
        }

        public void DeleteTask(int id)
        {
            var task = GetTaskById(id);
            _tasks.Remove(task);
        }

        public void UpdateTaskStatus(int id, TaskStatus status)
        {
            var task = GetTaskById(id);
            task.Status = status;
            
            if (status == TaskStatus.Completed)
            {
                task.MarkAsCompleted();
            }
        }

        public IEnumerable<Task> GetAllTasks() => _tasks.AsReadOnly();

        public IEnumerable<Task> GetTasksByStatus(TaskStatus status)
        {
            return _tasks.Where(t => t.Status == status);
        }

        public IEnumerable<Task> GetTasksByPriority(Priority priority)
        {
            return _tasks.Where(t => t.Priority == priority);
        }

        public IEnumerable<Task> GetOverdueTasks()
        {
            return _tasks.Where(t => t.IsOverdue());
        }

        public IEnumerable<Task> GetTasksSortedByPriority()
        {
            return _tasks.OrderByDescending(t => (int)t.Priority).ThenBy(t => t.DueDate ?? DateTime.MaxValue);
        }

        public void DisplayTaskSummary()
        {
            var totalTasks = _tasks.Count;
            var completedTasks = _tasks.Count(t => t.Status == TaskStatus.Completed);
            var overdueTasks = _tasks.Count(t => t.IsOverdue());
            var pendingTasks = _tasks.Count(t => t.Status == TaskStatus.Pending);

            Console.WriteLine("\n=== TASK SUMMARY ===");
            Console.WriteLine($"Total Tasks: {totalTasks}");
            Console.WriteLine($"Completed: {completedTasks}");
            Console.WriteLine($"Pending: {pendingTasks}");
            Console.WriteLine($"Overdue: {overdueTasks}");
            
            if (totalTasks > 0)
            {
                var completionRate = (completedTasks * 100.0) / totalTasks;
                Console.WriteLine($"Completion Rate: {completionRate:F1}%");
            }
        }
    }

    // Main program class
    class Program
    {
        private static TaskManager taskManager = new TaskManager();

        static void Main(string[] args)
        {
            Console.WriteLine("=== Task Management System ===\n");
            
            // Create sample data
            InitializeSampleData();
            
            // Interactive menu
            bool running = true;
            while (running)
            {
                ShowMenu();
                var input = Console.ReadLine();
                
                try
                {
                    switch (input?.ToLower())
                    {
                        case "1":
                            CreateNewTask();
                            break;
                        case "2":
                            ViewAllTasks();
                            break;
                        case "3":
                            ViewTasksByStatus();
                            break;
                        case "4":
                            MarkTaskCompleted();
                            break;
                        case "5":
                            ViewOverdueTasks();
                            break;
                        case "6":
                            ViewTasksSortedByPriority();
                            break;
                        case "7":
                            taskManager.DisplayTaskSummary();
                            break;
                        case "8":
                            DeleteTask();
                            break;
                        case "q":
                            running = false;
                            Console.WriteLine("Goodbye!");
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                catch (TaskManagementException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }
                
                if (running)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        static void InitializeSampleData()
        {
            try
            {
                taskManager.CreateTask("Design Database Schema", "Create ERD and database design for the new project", Priority.High, DateTime.Now.AddDays(-2));
                taskManager.CreateTask("Implement User Authentication", "Add login/logout functionality with JWT tokens", Priority.Critical, DateTime.Now.AddDays(3));
                taskManager.CreateTask("Write Unit Tests", "Create comprehensive unit tests for the API endpoints", Priority.Medium, DateTime.Now.AddDays(7));
                taskManager.CreateTask("Update Documentation", "Update API documentation and README file", Priority.Low, DateTime.Now.AddDays(14));
                taskManager.CreateTask("Code Review", "Review pull requests from team members", Priority.Medium);
                
                // Mark one task as completed
                taskManager.UpdateTaskStatus(1, TaskStatus.Completed);
                taskManager.UpdateTaskStatus(5, TaskStatus.InProgress);
                
                Console.WriteLine("Sample data initialized successfully!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing sample data: {ex.Message}");
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("\n=== MAIN MENU ===");
            Console.WriteLine("1. Create New Task");
            Console.WriteLine("2. View All Tasks");
            Console.WriteLine("3. View Tasks by Status");
            Console.WriteLine("4. Mark Task as Completed");
            Console.WriteLine("5. View Overdue Tasks");
            Console.WriteLine("6. View Tasks by Priority");
            Console.WriteLine("7. View Task Summary");
            Console.WriteLine("8. Delete Task");
            Console.WriteLine("Q. Quit");
            Console.Write("\nSelect an option: ");
        }

        static void CreateNewTask()
        {
            Console.Write("Enter task title: ");
            var title = Console.ReadLine();
            
            Console.Write("Enter task description: ");
            var description = Console.ReadLine();
            
            Console.Write("Enter priority (1=Low, 2=Medium, 3=High, 4=Critical) [default: 2]: ");
            var priorityInput = Console.ReadLine();
            var priority = Priority.Medium;
            
            if (int.TryParse(priorityInput, out int priorityValue) && priorityValue >= 1 && priorityValue <= 4)
            {
                priority = (Priority)priorityValue;
            }
            
            Console.Write("Enter due date (yyyy-mm-dd) [optional]: ");
            var dueDateInput = Console.ReadLine();
            DateTime? dueDate = null;
            
            if (!string.IsNullOrWhiteSpace(dueDateInput) && DateTime.TryParse(dueDateInput, out DateTime parsedDate))
            {
                dueDate = parsedDate;
            }
            
            var task = taskManager.CreateTask(title, description, priority, dueDate);
            Console.WriteLine($"\nTask created successfully: {task}");
        }

        static void ViewAllTasks()
        {
            var tasks = taskManager.GetAllTasks();
            DisplayTasks(tasks, "All Tasks");
        }

        static void ViewTasksByStatus()
        {
            Console.WriteLine("Select status:");
            Console.WriteLine("1. Pending");
            Console.WriteLine("2. In Progress");
            Console.WriteLine("3. Completed");
            Console.WriteLine("4. Cancelled");
            Console.Write("Enter choice: ");
            
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 4)
            {
                var status = (TaskStatus)(choice - 1);
                var tasks = taskManager.GetTasksByStatus(status);
                DisplayTasks(tasks, $"{status} Tasks");
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }

        static void MarkTaskCompleted()
        {
            Console.Write("Enter task ID to mark as completed: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                taskManager.UpdateTaskStatus(id, TaskStatus.Completed);
                Console.WriteLine("Task marked as completed!");
            }
            else
            {
                Console.WriteLine("Invalid task ID.");
            }
        }

        static void ViewOverdueTasks()
        {
            var overdueTasks = taskManager.GetOverdueTasks();
            DisplayTasks(overdueTasks, "Overdue Tasks");
        }

        static void ViewTasksSortedByPriority()
        {
            var tasks = taskManager.GetTasksSortedByPriority();
            DisplayTasks(tasks, "Tasks Sorted by Priority");
        }

        static void DeleteTask()
        {
            Console.Write("Enter task ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                taskManager.DeleteTask(id);
                Console.WriteLine("Task deleted successfully!");
            }
            else
            {
                Console.WriteLine("Invalid task ID.");
            }
        }

        static void DisplayTasks(IEnumerable<Task> tasks, string title)
        {
            Console.WriteLine($"\n=== {title.ToUpper()} ===");
            
            if (!tasks.Any())
            {
                Console.WriteLine("No tasks found.");
                return;
            }
            
            foreach (var task in tasks)
            {
                Console.WriteLine(task);
                if (!string.IsNullOrEmpty(task.Description))
                {
                    Console.WriteLine($"    Description: {task.Description}");
                }
                Console.WriteLine($"    Created: {task.CreatedDate:yyyy-MM-dd HH:mm}");
                if (task.CompletedDate.HasValue)
                {
                    Console.WriteLine($"    Completed: {task.CompletedDate:yyyy-MM-dd HH:mm}");
                }
                Console.WriteLine();
            }
        }
    }
}