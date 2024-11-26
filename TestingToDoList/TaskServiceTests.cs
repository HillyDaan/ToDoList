using System.Reflection;
using ToDolistVersion2.Interfaces;
using ToDolistVersion2.ViewModels;
using ToDolistVersion2.Models;
using Moq;
using System.Collections.ObjectModel;

namespace TestingToDoList
{
    public class TaskServiceTests
    {
        //Test for checking if the AddTaskViewModel correctly calls the service AddTask
        [Fact]
        public void AddItem_CallsAddTaskMethodInService_UsingReflection()
        {
            // Arrange
            var mockTaskService = new Mock<ITaskService>();
            mockTaskService.Setup(service => service.Tasks)
                           .Returns(new ObservableCollection<TaskModel>());

            var viewModel = new ViewModelAddTask(mockTaskService.Object);

            // Setup ViewModel's properties
            viewModel.NewTitle = "New Task";
            viewModel.NewDescription = "Description of the task";
            viewModel.NewPoints = 10;
            viewModel.Deadline = new DateTimeOffset(new DateTime(2024, 12, 31));

            // Act - Invoke AddItem via reflection
            var methodInfo = typeof(ViewModelAddTask).GetMethod("AddItem", BindingFlags.NonPublic | BindingFlags.Instance);
            methodInfo.Invoke(viewModel, null);

            // Assert
            mockTaskService.Verify(service => service.AddTask(It.IsAny<TaskModel>()), Times.Once);
        }
        //Test for checking if an item is actually added to the collection
        [Fact]
        public void AddTask_AddsTaskWithSubtasksToCollection()
        {
            // Arrange
            var tasksCollection = new ObservableCollection<TaskModel>();
            var taskService = new Mock<ITaskService>();

            // Set up the mock to use the real collection
            taskService.Setup(service => service.Tasks).Returns(tasksCollection);
            taskService.Setup(service => service.AddTask(It.IsAny<TaskModel>()))
                       .Callback<TaskModel>(task => tasksCollection.Add(task)); // Define AddTask behavior

            var initialCount = tasksCollection.Count;

            var newTask = new TaskModel
            {
                Id = "3",
                Title = "New Task",
                Points = 10,
                Created = DateTime.Now,
                Deadline = DateTime.Now.AddDays(1),
                IsChecked = false,
                SubTasks = new ObservableCollection<ViewModelSubTask> // Add subtasks to the new task
                    {
                        new ViewModelSubTask { Title = "Subtask 1", IsChecked = false },
                        new ViewModelSubTask { Title = "Subtask 2", IsChecked = false }
                    }
            };

            // Act
            taskService.Object.AddTask(newTask);

            // Assert
            Assert.Equal(initialCount + 1, tasksCollection.Count); // Check task count
            Assert.Contains(newTask, tasksCollection); // Check if the task was added to the collection

            // Check if the task has subtasks
            Assert.Equal(2, newTask.SubTasks.Count); // Verify that the task has 2 subtasks
            Assert.Contains(newTask.SubTasks, subTask => subTask.Title == "Subtask 1"); // Check if Subtask 1 is in the list
            Assert.Contains(newTask.SubTasks, subTask => subTask.Title == "Subtask 2"); // Check if Subtask 2 is in the list
        }

        //Test for checking if subtasks are added
        [Fact]
        public void AddSubTask_AddsSubTaskToList()
        {

            {
                // Arrange
                var mockTaskService = new Mock<ITaskService>();
                mockTaskService.Setup(service => service.Tasks)
                               .Returns(new ObservableCollection<TaskModel>());

                var viewModel = new ViewModelAddTask(mockTaskService.Object);

                // Initial state: SubTaskList is empty
                var initialCount = viewModel.SubTaskList.Count;
                Console.WriteLine(initialCount);

                var newSubTaskTitle = "New SubTask";
                viewModel.NewSubTaskTitle = newSubTaskTitle; // Set the title for the new subtask

                // Act
                viewModel.AddSubTask(); // This should add the new subtask to the SubTaskList
                // Assert
                Assert.Equal(initialCount + 1, viewModel.SubTaskList.Count); // Check that the count increased
                Assert.Contains(viewModel.SubTaskList, subTask => subTask.Title == newSubTaskTitle); // Check if the new subtask is added to the list
            }
        }
    }
}