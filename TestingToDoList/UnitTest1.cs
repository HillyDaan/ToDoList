using System.Reflection;
using ToDolistVersion2.Interfaces;
using ToDolistVersion2.ViewModels;
using ToDolistVersion2.Models;
using Moq;
using System.Collections.ObjectModel;

namespace TestingToDoList
{
    public class UnitTest1
    {
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
    }
}