using System.Collections.ObjectModel;
using System.Linq;
using ToDolistVersion2.Models;
using ToDolistVersion2.Services;

namespace ToDolistVersion2.ViewModels
{
    public partial class ViewModelHome : ViewModelBase
    {
        private readonly TaskService _taskService;
        public ObservableCollection<ViewModelTask> Tasks { get; }

        public ViewModelHome(TaskService taskService) {
            _taskService = taskService;
            //Convert tasks to taskmodel
            Tasks = new ObservableCollection<ViewModelTask>(
               _taskService.Tasks.Select(task => new ViewModelTask(task))
            );
        }

        public ViewModelHome() { }

    }
}
