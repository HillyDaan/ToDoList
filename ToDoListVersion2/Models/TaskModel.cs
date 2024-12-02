using System;
using System.Collections.ObjectModel;
using ToDolistVersion2.ViewModels;

namespace ToDolistVersion2.Models
{
    public class TaskModel
    {
        public string? Id { get; set; }

        public string? Title { get; set; }

        public int? Points { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Deadline { get; set; }

        public bool IsChecked { get; set; }

        public string? Description { get; set; }

        public ObservableCollection<ViewModelSubTask>? SubTasks { get; set; }
    }
}
