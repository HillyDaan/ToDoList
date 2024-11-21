using System.Collections.ObjectModel;
using System.Linq;
using ToDolistVersion2.Models;
using ToDolistVersion2.Services;
using ToDolistVersion2.Interfaces;

namespace ToDolistVersion2.ViewModels
{
    public partial class ViewModelHome : ViewModelBase
    {
        private readonly ITaskService _taskService;
        public ObservableCollection<ViewModelTask> Tasks { get; }

        public ViewModelHome(ITaskService taskService) {
            _taskService = taskService;
            //Convert tasks to ViewTaskmodel
            Tasks = new ObservableCollection<ViewModelTask>(
               _taskService.Tasks.Select(task => new ViewModelTask(task))
            );
        }

    }
}
