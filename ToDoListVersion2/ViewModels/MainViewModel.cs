// Filename: MainViewModel.cs
// Description: This file contains the MainViewModel class, which manages the navigation between different views of the ToDo List application. 
//              It handles setting the current view model and provides navigation commands to switch between the home page, add task page, and all tasks page.
//              The MainViewModel also integrates with the ITaskService to manage tasks across the application, including saving the tasks.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ToDolistVersion2.Interfaces;

namespace ToDolistVersion2.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets and Sets the current view model
        /// </summary>
        [ObservableProperty]
        private ViewModelBase _currentView;

        /// <summary>
        /// Reference to taskService that contains all tasks
        /// </summary>
        private readonly ITaskService _taskService;


        public MainViewModel(ITaskService taskService)
        {
            //Initialize taskservice
            this._taskService = taskService;
            //Initialize home page as current view
            CurrentView = new ViewModelHome(taskService);
        }
        //Commands for navigation
        [RelayCommand]
        public void NavigateToAddTaskPage(ViewModelTask? task = null)
        {
            //If tasks is supplied we edit an existing task
            if (task != null)
            {
                CurrentView = new ViewModelAddTask(_taskService, task);
            } else
            {
                //Pass taskService
                CurrentView = new ViewModelAddTask(_taskService);
            }
        }
        [RelayCommand]
        public void NavigateToHomePage()
        {
            CurrentView = new ViewModelHome(_taskService);
        }

        [RelayCommand]
        public void NavigateToAllTaskPage()
        {
            CurrentView = new ViewModelAllTask(_taskService, this);
        }

        [RelayCommand]
        public void SaveTasks()
        {
            _taskService.SaveTasks();
        }
    }
}
