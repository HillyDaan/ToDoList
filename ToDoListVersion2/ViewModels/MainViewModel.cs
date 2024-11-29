using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDolistVersion2.Services;
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
