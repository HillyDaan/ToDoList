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
                    Description = "Dit is de eerste description",
                    Created = new DateTime(2024, 11, 20), 
                    Deadline = new DateTime(2024, 12, 10), 
                    IsChecked = false,
                    SubTasks = new ObservableCollection<ViewModels.ViewModelSubTask> { 
                        new ViewModels.ViewModelSubTask(new SubTaskModel{Title = "subTask 1", IsChecked=false, Id="1", ParentId = "1", Points = 3}),
                        new ViewModels.ViewModelSubTask(new SubTaskModel{Title = "subTask 2", IsChecked=false, Id = "2", ParentId = "1",Points = 2}),
                    } 
                },
                new TaskModel {Id = "2", 
                    Title = "task 2", 
                    Points = 5,
                    Description = "Dit is de tweede description",
                    Created = new DateTime(2024, 11, 5), 
                    Deadline = new DateTime(2024, 12, 15), 
                    IsChecked = false,
                    SubTasks = new ObservableCollection<ViewModels.ViewModelSubTask> {
                        new ViewModels.ViewModelSubTask(new SubTaskModel{Title = "subTask 3", IsChecked=false, Id="3", ParentId = "2", Points = 1}),
                        new ViewModels.ViewModelSubTask(new SubTaskModel{Title = "subTask 4", IsChecked=false, Id="4", ParentId = "2", Points = 10}),
                    } 
                },
                new TaskModel {Id = "3",
                    Title = "task 3",
                    Points = 10,
                    Description = "High PRIORITY",
                    Created = new DateTime(2024, 11, 29),
                    Deadline = new DateTime(2024, 12, 1),
                    IsChecked = false,
                    SubTasks = new ObservableCollection<ViewModels.ViewModelSubTask> {
                        new ViewModels.ViewModelSubTask(new SubTaskModel{Title = "subTask 1: makkelijk maar moet gebeuren", IsChecked=false, Id="1", ParentId = "3", Points = 3}),
                        
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

        public void UpdateTask(TaskModel task)
        {
            var taskToUpdate = Tasks.FirstOrDefault(t => t.Id == task.Id);
            if ( taskToUpdate != null)
            {
                taskToUpdate.Title = task.Title;
                taskToUpdate.Description = task.Description;
                taskToUpdate.Points = task.Points;
                taskToUpdate.Deadline = task.Deadline;
                taskToUpdate.SubTasks = task.SubTasks;
            }
        }

        public void CheckOffTask(TaskModel task, bool isChecked)
        {
            var taskToUpdate = Tasks.FirstOrDefault(t => t.Id == task.Id);
            if (taskToUpdate != null)
            {
                taskToUpdate.IsChecked = isChecked;
                // Optionally, update subtasks if necessary
                foreach (var subTask in taskToUpdate.SubTasks)
                {
                    subTask.IsChecked = isChecked;  // This will sync subtask states too, if needed
                }
            }
        }
        public void CheckOffSubTask(SubTaskModel subTask, bool isChecked)
        {
            // Find the parent task by matching the task id
            var taskToUpdate = Tasks.FirstOrDefault(t => t.Id == subTask.ParentId); // Assuming each subtask has a reference to its parent task

            if (taskToUpdate != null)
            {
                // Find the subtask to update
                var subTaskToUpdate = taskToUpdate.SubTasks.FirstOrDefault(st => st.Id == subTask.Id);

                if (subTaskToUpdate != null)
                {
                    subTaskToUpdate.IsChecked = isChecked;
                }
            }
        }

    }
}
