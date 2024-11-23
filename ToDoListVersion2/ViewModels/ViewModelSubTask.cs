using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDolistVersion2.Models;

namespace ToDolistVersion2.ViewModels
{
    public partial class ViewModelSubTask : ViewModelBase
    {
        [ObservableProperty]
        private string? _title;

        [ObservableProperty]
        private Boolean _isChecked;

        public ViewModelSubTask() { }

        public ViewModelSubTask(SubTaskModel subTask)
        {
            this.Title = subTask.Title;
            this.IsChecked = subTask.isChecked;
        }

        public SubTaskModel GetSubTask()
        {
            return new SubTaskModel() { Title = Title, isChecked = IsChecked };
        }


    }
}
