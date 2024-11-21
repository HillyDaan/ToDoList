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
    public partial class ViewModelAllTask : ViewModelBase
    {
        private readonly ITaskService _taskService;

        public ObservableCollection<ViewModelTask> Tasks { get; }
        public ViewModelAllTask(ITaskService taskService)
        {
            _taskService = taskService;
            Tasks = new ObservableCollection<ViewModelTask>(
                _taskService.Tasks.Select(task => new ViewModelTask(task))
            );
        }
    }
}
