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

        public ViewModelTask() { }

        public ViewModelTask(TaskModel task)
        {
            IsChecked = task.IsChecked;
            Title = task.Title;
            Console.WriteLine(Title);
        }

        public TaskModel GetTask()
        {
            return new TaskModel()
            {
                Title = this.Title,
                IsChecked = this.IsChecked,
            };
        }
    }
}
