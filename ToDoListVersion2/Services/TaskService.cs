using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDolistVersion2.Interfaces;
using ToDolistVersion2.Models;

namespace ToDolistVersion2.Services
{
    public class TaskService : ITaskService
    {
        public ObservableCollection<TaskModel> Tasks { get; private set; }

        public TaskService()
        {
            Tasks = new ObservableCollection<TaskModel>()
            {
                new TaskModel {Id = "1", 
                    Title = "task 1", 
                    Points = 5, 
                    Created = new DateTime(10), 
                    Deadline = new DateTime(15), 
                    IsChecked = false,
                    SubTasks = new ObservableCollection<ViewModels.ViewModelSubTask> { 
                        new ViewModels.ViewModelSubTask(){Title = "subTask 1", IsChecked=false},
                        new ViewModels.ViewModelSubTask(){Title = "subTask 2", IsChecked=false},
                    } 
                },
                new TaskModel {Id = "2", 
                    Title = "task 2", 
                    Points = 5, 
                    Created = new DateTime(10), 
                    Deadline = new DateTime(15), 
                    IsChecked = false,
                    SubTasks = new ObservableCollection<ViewModels.ViewModelSubTask> {
                        new ViewModels.ViewModelSubTask(){Title = "subTask 3", IsChecked=false},
                        new ViewModels.ViewModelSubTask(){Title = "subTask 4", IsChecked=false},
                    } 
                }
            };
        }

        /// <summary>
        /// Adds task to observable collection
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(TaskModel task)
        {
            Tasks.Add(task);
        }

        
    }
}
