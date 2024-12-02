using System.Reflection;
using ToDolistVersion2.Interfaces;
using ToDolistVersion2.ViewModels;
using ToDolistVersion2.Models;
using Moq;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace TestingToDoList
{
    public class TaskServiceTests
    {
        private ObservableCollection<TaskModel> _testTasks;
        public TaskServiceTests()
        {
            // Sample data
            _testTasks = new ObservableCollection<TaskModel>
            {
                new TaskModel
                {
                    Id = "1",
                    Title = "Most Important task",
                    Points = 8,
                    Deadline = DateTime.Now.AddDays(4),
                    IsChecked = false,
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
            };
        }

        //Test for saving tasks
        [Fact]
        public void saveTasks_savesTasksToJson()
        {
            // Arrange
            var service = new TaskServiceMock();
            service.Tasks = new ObservableCollection<TaskModel>(_testTasks);

            // Act
            service.SaveTasks();

            // Assert
            Assert.True(service.IsSaved); 

        }
        //Test for loading tasks
        [Fact]
        public void loadTasks_loadTasksFromJson()
        {
            // Arrange
            var mockFile = JsonConvert.SerializeObject(_testTasks, Formatting.Indented); 
            var service = new TaskServiceMock(mockFile);

            // Act
            service.SaveTasks();

            // Assert
            Assert.NotNull(service.Tasks);
            Assert.Equal(_testTasks.Count, service.Tasks.Count);

        }

        //Test for deleting a task
        [Fact]
        public void deleteTask_deleteTaskFromList()
        {
            // Arrange
            var service = new TaskServiceMock();
            service.Tasks = new ObservableCollection<TaskModel>(_testTasks);

            // Act
            var taskToDelete = _testTasks[0];
            service.DeleteTask(taskToDelete); // Delete task two with id 1

            // Assert
            Assert.Equal(_testTasks.Count - 1, service.Tasks.Count);

        }
        //Test for checking off 
        [Fact]
        public void checkOffTask_CheckOffTaskFromList()
        {
            // Arrange
            var service = new TaskServiceMock();
            service.Tasks = new ObservableCollection<TaskModel>(_testTasks);
            
            bool checkOff = true;
            var taskToCheck = service.Tasks[0];
            // Act 

            service.CheckOffTask(taskToCheck, checkOff);

            // Assert
            Assert.True(taskToCheck.IsChecked);

            //Check all subtasks
            foreach (var subTask in taskToCheck.SubTasks)
            {
                Assert.True(subTask.IsChecked);
            }

        }

        //Test for checking off subtask
        [Fact]
        public void checkOffSubTask_CheckOffSubTaskFromList()
        {
            // Arrange
            var service = new TaskServiceMock();
            service.Tasks = new ObservableCollection<TaskModel>(_testTasks);

            bool checkOffSub = true;
            var parentTask = service.Tasks[0];
            var taskToCheck = service.Tasks[0].SubTasks[0];

            // Act
            service.CheckOffSubTask(taskToCheck.GetSubTask(), checkOffSub);

            // Assert
            Assert.True(taskToCheck.IsChecked);

            //Check that other subtasks are not checked
            Assert.False(parentTask.IsChecked);
            Assert.False(parentTask.SubTasks[1].IsChecked);
        }
    }
}