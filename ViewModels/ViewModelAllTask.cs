using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDolistVersion2.Services;

namespace ToDolistVersion2.ViewModels
{
    public partial class ViewModelAllTask : ViewModelBase
    {
        private readonly TaskService _taskService;

        public ObservableCollection<ViewModelTask> Tasks { get; }
        public ViewModelAllTask(TaskService taskService)
        {
            _taskService = taskService;
            Tasks = new ObservableCollection<ViewModelTask>(
                _taskService.Tasks.Select(task => new ViewModelTask(task))
            );
        }
    }
}
