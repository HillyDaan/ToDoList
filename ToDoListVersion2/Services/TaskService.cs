
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using ToDolistVersion2.Interfaces;
using ToDolistVersion2.Models;
using Newtonsoft.Json;


namespace ToDolistVersion2.Services
{
    public class TaskService : ITaskService
    {
        public ObservableCollection<TaskModel> Tasks { get; private set; }

        private readonly string _taskFilePath = "tasks.json";

        public TaskService()
        {
            Tasks = new ObservableCollection<TaskModel>();
            LoadTasks();
        }

        // Saves tasks to the file
        public void SaveTasks()
        {
            var json = JsonConvert.SerializeObject(Tasks, Formatting.Indented);
            File.WriteAllText(_taskFilePath, json);
        }

        public void LoadTasks()
        {
            if(File.Exists(_taskFilePath))
            {
                var json = File.ReadAllText(_taskFilePath);
                var tasks = JsonConvert.DeserializeObject<ObservableCollection<TaskModel>>(json);
                if (tasks != null)
                {
                    Tasks = tasks;
                }
            }
        }
        /// <summary>
        /// Adds task to observable collection
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(TaskModel task)
        {
            Tasks.Add(task);
        }

        public void DeleteTask(TaskModel task)
        {
            var taskToRemove = Tasks.FirstOrDefault(x => x.Id == task.Id);
            if(taskToRemove != null)
            {
                Tasks.Remove(taskToRemove);
            }
            
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
