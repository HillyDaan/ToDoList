using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDolistVersion2.Interfaces;
using ToDolistVersion2.Models;
using ToDolistVersion2.ViewModels;

namespace TestingToDoList
{
    public class NavigationTests
    {
        //Test that homeView is intiallized on start up
        [Fact]
        public void Constructor_InitializesWithHomeView()
        {
            // Arrange
            var mockTaskService = new Mock<ITaskService>();
            // Ensure Tasks is not null
            mockTaskService.Setup(service => service.Tasks).Returns(new ObservableCollection<TaskModel>());

            // Act
            var mainViewModel = new MainViewModel(mockTaskService.Object);

            // Assert
            Assert.IsType<ViewModelHome>(mainViewModel.CurrentView);
        }

        //Test to move to addTask view
        [Fact]
        public void NavigateToAddTaskPage_SetsCurrentViewToAddTask()
        {
            // Arrange
            var mockTaskService = new Mock<ITaskService>();
            mockTaskService.Setup(service => service.Tasks).Returns(new ObservableCollection<TaskModel>());
            var mainViewModel = new MainViewModel(mockTaskService.Object);

            // Act
            mainViewModel.NavigateToAddTaskPageCommand.Execute(null);

            // Assert
            Assert.IsType<ViewModelAddTask>(mainViewModel.CurrentView);
        }

        //Test to move to allTaskView
        [Fact]
        public void NavigateToAllTaskPage_SetsCurrentViewToAllTask()
        {
            // Arrange
            var mockTaskService = new Mock<ITaskService>();
            mockTaskService.Setup(service => service.Tasks).Returns(new ObservableCollection<TaskModel>());
            var mainViewModel = new MainViewModel(mockTaskService.Object);

            //Act
            mainViewModel.NavigateToAllTaskPageCommand.Execute(null);

            //Assert
            Assert.IsType<ViewModelAllTask>(mainViewModel.CurrentView);
        }

        //Test to move to homePage

        [Fact]
        public void NavigateToHomePage_SetsCurrentViewToHome()
        {
            // Arrange
            var mockTaskService = new Mock<ITaskService>();
            mockTaskService.Setup(service => service.Tasks).Returns(new ObservableCollection<TaskModel>());
            var mainViewModel = new MainViewModel(mockTaskService.Object);

            //Act
            mainViewModel.NavigateToHomePageCommand.Execute(null);

            //Assert
            Assert.IsType<ViewModelHome>(mainViewModel.CurrentView);
        }
    }
}
