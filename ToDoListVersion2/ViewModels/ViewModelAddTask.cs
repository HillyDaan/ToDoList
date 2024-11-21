using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDolistVersion2.Services;
using ToDolistVersion2.Interfaces;

namespace ToDolistVersion2.ViewModels
{
    public partial class ViewModelAddTask : ViewModelBase
    {
        private readonly ITaskService _taskService;

        [ObservableProperty]
        private string? _newTitle;

        [ObservableProperty]
        private string? _newDescription;

        [ObservableProperty]
        private int? _newPoints;

        [ObservableProperty]
        private DateTimeOffset? _deadline;


        public ObservableCollection<ViewModelTask> Tasks { get; set; }
        public ViewModelAddTask(ITaskService taskService)
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
            var newTask = new ViewModelTask() {
                Title = NewTitle,
                IsChecked = false,
                Points = NewPoints,
                Description = NewDescription,
                CreatedDate = DateTime.Now,
                DeadlineDate = Deadline?.Date
            };
            _taskService.AddTask(newTask.GetTask());
            Tasks.Add(newTask);
        }
    }
}
