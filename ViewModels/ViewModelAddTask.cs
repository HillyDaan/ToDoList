using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDolistVersion2.Services;

namespace ToDolistVersion2.ViewModels
{
    public partial class ViewModelAddTask : ViewModelBase
    {
        private readonly TaskService _taskService;

        [ObservableProperty]
        private string? _newTitle;


        public ObservableCollection<ViewModelTask> Tasks { get; set; }
        public ViewModelAddTask(TaskService taskService)
        {
            _taskService = taskService;
            Tasks = new ObservableCollection<ViewModelTask>(
                _taskService.Tasks.Select(task => new ViewModelTask(task))
            );
        }

        //Add check if item is valid later
        //Using      [RelayCommand(CanExecute = nameof(CanAddItem))]
        [RelayCommand]
        private void AddItem()
        {
            var newTask = new ViewModelTask() { Title = _newTitle, IsChecked = false, Points = 5 };
            _taskService.AddTask(newTask.GetTask());
            Tasks.Add(newTask);

        }
    }
}
