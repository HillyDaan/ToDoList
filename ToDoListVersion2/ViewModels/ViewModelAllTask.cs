// Filename: ViewModelAllTask.cs
// Description: This file contains the ViewModelAllTask class, which manages the display and actions for all tasks in the ToDo List application.
//              It allows the user to interact with tasks by updating their status (checked/unchecked), deleting tasks, and navigating to the task detail page for editing. 
//              The ViewModelAllTask class retrieves tasks from the ITaskService and provides methods for modifying tasks and subtasks, including updating their status and deleting them.  
//              Linked to ViewAllTask.axaml

using System.Collections.ObjectModel;
using System.Linq;
using ToDolistVersion2.Interfaces;
using CommunityToolkit.Mvvm.Input;

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
        public void UpdateSubTaskStatus(ViewModelSubTask subTask)
        {
            _taskService.CheckOffSubTask(subTask.GetSubTask(), subTask.IsChecked);
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
        public void DeleteTask(ViewModelTask task)
        {
            if(task != null)
            {
                //Remove from local collection
                Tasks.Remove(task);

                //Remove from service
                _taskService.DeleteTask(task.GetTask());

            }
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
