using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
