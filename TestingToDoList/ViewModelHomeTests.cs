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
        [Fact]
        public void CalculateTopTasks_CalculatesTopTasksCorrectly_SameWeek()
        {
            // Arrange
            var mockTaskService = new Mock<ITaskService>();

            // Set up mocked tasks
            mockTaskService.Setup(service => service.Tasks).Returns(new ObservableCollection<TaskModel>
        {
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
                    new ViewModelSubTask { Title = "subTask 1", Points = 3 }
                }
            }
        });

            // Initialize ViewModelHome with the mocked task service
            var viewModelHome = new ViewModelHome(mockTaskService.Object);

            // Act
            viewModelHome.CalculateTopTasks(3); // Calculate top 3 tasks

            // Assert
            var topTasks = viewModelHome.TopTasks;

            // Verify the number of tasks in the TopTasks collection
            Assert.Equal(3, topTasks.Count);

            // Verify that tasks are sorted based on score, highest first
            Assert.Equal("1", topTasks[0].Id); // Task 1 should be first (highest priority)
            Assert.Equal("3", topTasks[1].Id); // Task 3 should be second
            Assert.Equal("2", topTasks[2].Id); // Task 2 should be third
        }

        [Fact]
        public void CalculateTopTasks_ExcludesTasksWithoutDeadline()
        {
            // Arrange
            var mockTaskService = new Mock<ITaskService>();

            // Set up the mock to return a collection of tasks
            mockTaskService.Setup(service => service.Tasks).Returns(new ObservableCollection<TaskModel>
        {
        new TaskModel
        {
            Id = "4",
            Title = "task 4",
            Points = 5,
            Deadline = null, // No deadline
            SubTasks = new ObservableCollection<ViewModelSubTask>()
        },
        new TaskModel
        {
            Id = "5",
            Title = "task 5",
            Points = 7,
            Deadline = new DateTime(2024, 12, 2), // Valid deadline
            SubTasks = new ObservableCollection<ViewModelSubTask>()
        }
        });

            // Initialize ViewModelHome with the mocked task service
            var viewModelHome = new ViewModelHome(mockTaskService.Object);

            // Act
            viewModelHome.CalculateTopTasks(3); // Calculate top 3 tasks

            // Assert
            var topTasks = viewModelHome.TopTasks;

            // Assert that only tasks with a valid deadline are included
            Assert.Single(topTasks); // Only task 5 should be in the list
            Assert.Equal("task 5", topTasks[0].Title); // Task 5 should be the only task in TopTasks
        }

        [Fact]
        public void CalculateTopTasks_SortsByDeadlineAndSeverity()
        {
            // Arrange
            var mockTaskService = new Mock<ITaskService>();

            // Set up the mock to return a collection of tasks
            mockTaskService.Setup(service => service.Tasks).Returns(new ObservableCollection<TaskModel>
    {
        new TaskModel
        {
            Id = "1",
            Title = "task 1",
            Points = 5,
            Deadline = new DateTime(2024, 12, 10),
            SubTasks = new ObservableCollection<ViewModelSubTask>
            {
                new ViewModelSubTask { Title = "subTask 1", Points = 3 },
                new ViewModelSubTask { Title = "subTask 2", Points = 2 }
            }
        },
        new TaskModel
        {
            Id = "2",
            Title = "task 2",
            Points = 5,
            Deadline = new DateTime(2024, 12, 15),
            SubTasks = new ObservableCollection<ViewModelSubTask>
            {
                new ViewModelSubTask { Title = "subTask 3", Points = 1 },
                new ViewModelSubTask { Title = "subTask 4", Points = 10 }
            }
        },
        new TaskModel
        {
            Id = "3",
            Title = "task 3",
            Points = 10,
            Deadline = new DateTime(2024, 12, 1),
            SubTasks = new ObservableCollection<ViewModelSubTask>
            {
                new ViewModelSubTask { Title = "subTask 1", Points = 3 }
            }
        }
        });

            // Initialize ViewModelHome with the mocked task service
            var viewModelHome = new ViewModelHome(mockTaskService.Object);

            // Act
            viewModelHome.CalculateTopTasks(3); // Calculate top 3 tasks

            // Assert
            var topTasks = viewModelHome.TopTasks;

            // Check that the tasks are sorted correctly: task 3 should be the first because it has the earliest deadline and highest severity
            Assert.Equal("task 3", topTasks[0].Title); // Task 3 should be first because it has the highest score due to its severity (10 points)
            Assert.Equal("task 1", topTasks[1].Title); // Task 1 should be second because it has a closer deadline
            Assert.Equal("task 2", topTasks[2].Title); // Task 2 should be third because it has the latest deadline
        }
    }
}


