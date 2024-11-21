using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDolistVersion2.Models;

namespace ToDolistVersion2.ViewModels
{
    public partial class ViewModelTask : ViewModelBase
    {
        [ObservableProperty]
        private bool _isChecked;

        [ObservableProperty]
        private string? _title;

        [ObservableProperty]
        private int? _points;

        public ViewModelTask() { }

        public ViewModelTask(TaskModel task)
        {
            IsChecked = task.IsChecked;
            Title = task.Title;
            Points = task.Points;
        }

        public TaskModel GetTask()
        {
            return new TaskModel()
            {
                Title = this.Title,
                IsChecked = this.IsChecked,
                Points = this.Points
            };
        }
    }
}
