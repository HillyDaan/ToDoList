// Filename: ITaskService.cs
// Description: Defines the ITaskService interface for managing tasks in the ToDo List app. 
//              It includes methods for adding, checking off, updating, deleting tasks and subtasks, 
//              as well as loading and saving tasks from a persistent storage. 
//              The interface is implemented by the TaskService class to provide task management functionality.


using System.Collections.ObjectModel;
using ToDolistVersion2.Models;

namespace ToDolistVersion2.Interfaces
{
    public interface ITaskService
    {
        ObservableCollection<TaskModel> Tasks { get; set; }
        public void AddTask(TaskModel task);

        public void CheckOffTask(TaskModel task, bool isChecked);

        public void CheckOffSubTask(SubTaskModel subTask, bool isChecked);

        public void UpdateTask(TaskModel task);

        public void DeleteTask(TaskModel task);

        public void SaveTasks();

        public void LoadTasks();
    }
}
