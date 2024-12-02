using Moq;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;
using ToDolistVersion2.ViewModels;
using ToDolistVersion2.Models;
using ToDolistVersion2.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TestingToDoList
{

    public class ViewModelHomeTests
    {

        private ObservableCollection<TaskModel> _testCases = new ObservableCollection<TaskModel>() {
            new TaskModel
            {
                Id = "1",
                Title = "Most Important task",
                Points = 8,
                Deadline = DateTime.Now.AddDays(4),
                SubTasks = new ObservableCollection<ViewModelSubTask>
                {
                    new ViewModelSubTask { Title = "subTask 1", Points = 3, ParentId="1", IsChecked=false},
                    new ViewModelSubTask { Title = "subTask 2", Points = 2, ParentId="1", IsChecked=false}
                }
            },
            new TaskModel
            {
                Id = "2",
                Title = "Third most important",
                Points = 2,
                Deadline = DateTime.Now.AddDays(4),
                SubTasks = new ObservableCollection<ViewModelSubTask>
                {
                    new ViewModelSubTask { Title = "subTask 3", Points = 1 },
                    new ViewModelSubTask { Title = "subTask 4", Points = 10 }
                }
            },
            new TaskModel
            {
                Id = "3",
                Title = "Second most",
                Points = 7,
                Deadline = DateTime.Now.AddDays(4),
                SubTasks = new ObservableCollection<ViewModelSubTask>
                {
                    new ViewModelSubTask { Title = "subTask 1", Points = 8 }
                }
            } };
        [Fact]
        public void CalculateTopTasks_CalculatesTopTasksCorrectly_SameWeek()
        {
            // Arrange
            var mockTaskService = new TaskServiceMock();
            mockTaskService.Tasks = new ObservableCollection<TaskModel>(_testCases);

            // Initialize ViewModelHome with the mocked task service
            var viewModelHome = new ViewModelHome(mockTaskService);

            // Act
            viewModelHome.CalculateTopTasks(3); // Calculate top 3 tasks

            // Assert
            var topTasks = viewModelHome.TopTasks;

            // Verify the number of tasks in the TopTasks collection
            Assert.Equal(3, topTasks.Count);

            // Verify that tasks are sorted based on score, highest first
            Assert.Equal("1", topTasks[0].Item1.Id); // Task 1 should be first (highest priority)
            Assert.Equal("3", topTasks[1].Item1.Id); // Task 3 should be second
            Assert.Equal("2", topTasks[2].Item1.Id); // Task 2 should be third
        }

        [Fact]
        public void CalculateTopTasks_ExcludesTasksWithoutDeadline()
        {
            // Arrange
            var mockTaskService = new TaskServiceMock();
            _testCases[0].Deadline = DateTime.Now.AddDays(-1);
            mockTaskService.Tasks = new ObservableCollection<TaskModel>(_testCases);

            // Initialize ViewModelHome with the mocked task service
            var viewModelHome = new ViewModelHome(mockTaskService);

            // Act
            viewModelHome.CalculateTopTasks(3); // Calculate top 3 (will be two) tasks

            // Assert
            var topTasks = viewModelHome.TopTasks;

            // Assert that only two tasks with a valid deadline were added
            Assert.Equal(2, viewModelHome.TopTasks.Count);
            Assert.Equal("3", topTasks[0].Item1.Id); // Task with id 3 will be the highest
        }

        [Fact]
        public void GetNearestDeadline_SortByDays()
        {
            // Arrange
            var mockTaskService = new TaskServiceMock();
            _testCases[0].Deadline = DateTime.Now.AddDays(3);
            _testCases[1].Deadline = DateTime.Now.AddDays(2);
            _testCases[2].Deadline = DateTime.Now.AddDays(1);

            mockTaskService.Tasks = new ObservableCollection<TaskModel>(_testCases);

            // Initialize ViewModelHome with the mocked task service
            var viewModelHome = new ViewModelHome(mockTaskService);

            // Act
            viewModelHome.GetNearestDeadlines(3);
            var deadlines = viewModelHome.Deadlines;

            //Assert
            Assert.Equal(_testCases[2].Id, deadlines[0].Id); // 2 -> 0
            Assert.Equal(_testCases[1].Id, deadlines[1].Id); // 1 -> 1
            Assert.Equal(_testCases[0].Id, deadlines[2].Id); // 0 -> 2

        }

        [Fact]
        public void CreatePieChart_CheckPieData()
        {
            // Arrange
            var mockTaskService = new TaskServiceMock();
            _testCases[0].Deadline = DateTime.Now.AddDays(1); // 5 Points in x < 3 days
            _testCases[1].Deadline = DateTime.Now.AddDays(6); // 11 Points  in  3 > x  < 7 days
            _testCases[2].Deadline = DateTime.Now.AddDays(15);// 8 Points in x > 7 days

            mockTaskService.Tasks = new ObservableCollection<TaskModel>(_testCases);

            // Initialize ViewModelHome with the mocked task service
            var viewModelHome = new ViewModelHome(mockTaskService);

            // Act (Pie creation in viewmodel home

            //Assert
            var PieData = viewModelHome.PieData;
            Assert.Equal(new int[] { 5 }, PieData[0].Values);
            Assert.Equal(new int[] { 11 }, PieData[1].Values);
            Assert.Equal(new int[] { 8 }, PieData[2].Values);
        }
    }
}


