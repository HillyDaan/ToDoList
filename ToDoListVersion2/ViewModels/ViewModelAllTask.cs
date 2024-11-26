using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDolistVersion2.Services;
using ToDolistVersion2.Interfaces;
using CommunityToolkit.Mvvm.Input;
using ToDolistVersion2.Models;

namespace ToDolistVersion2.ViewModels
{
    public partial class ViewModelAllTask : ViewModelBase
    {
        private readonly ITaskService _taskService;
        private readonly MainViewModel _mainViewModel;

        [RelayCommand]
        public void UpdateTaskStatus(ViewModelTask task)
        {
            //Set viewModel change for each subtask
            foreach(ViewModelSubTask subTask in task.SubTasks)
            {
                subTask.IsChecked = task.IsChecked;
            }
            //Viewmodel already changed, inform taskService of change
            _taskService.CheckOffTask(task.GetTask(), task.IsChecked);

        }
        [RelayCommand]
        public void NavigateToAddTaskPage(ViewModelTask task)
        {
            if(task != null)
            {
                _mainViewModel.NavigateToAddTaskPage(task);
            }
        }
        [RelayCommand]
        public void UpdateSubTaskStatus(ViewModelSubTask subTask)
        {
            _taskService.CheckOffSubTask(subTask.GetSubTask(), subTask.IsChecked);
        }
       
        public ObservableCollection<ViewModelTask> Tasks { get; }
       
        public ViewModelAllTask(ITaskService taskService, MainViewModel mainViewModel)
        {
            _taskService = taskService;
            Tasks = new ObservableCollection<ViewModelTask>(
                _taskService.Tasks.Select(task => new ViewModelTask(task))
            );
            _mainViewModel = mainViewModel;
        }
    }
}
