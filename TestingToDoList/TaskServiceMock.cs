using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDolistVersion2.Interfaces;
using ToDolistVersion2.Models;
using ToDolistVersion2.Services;

namespace TestingToDoList
{ 
    internal class TaskServiceMock : TaskService
        {
            private readonly string _mockFileContents;
            public bool IsSaved { get; private set; } = false;

            public TaskServiceMock(string mockFileContents = null)
            {
                _mockFileContents = mockFileContents;

                if (!string.IsNullOrEmpty(_mockFileContents))
                {
                    LoadTasks();
                }
            }

            public override void SaveTasks()
            {
                // Simulate saving tasks
                IsSaved = true;
            }

            public override void LoadTasks()
            {
                // Simulate loading tasks
                if (!string.IsNullOrEmpty(_mockFileContents))
                {
                    Tasks = JsonConvert.DeserializeObject<ObservableCollection<TaskModel>>(_mockFileContents);
                }
            }
    }
 }
   
