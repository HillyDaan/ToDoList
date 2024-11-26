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

        private string Id = Guid.NewGuid().ToString();

        [ObservableProperty]
        private string _buttonText = "Add Task";


        [ObservableProperty]
        private ObservableCollection<ViewModelSubTask> _subTaskList = new();

        [ObservableProperty]
        private string? _newTitle;

        [ObservableProperty]
        private string? _newDescription;

        [ObservableProperty]
        private int? _newPoints;

        [ObservableProperty]
        private DateTimeOffset? _deadline;

        [ObservableProperty]
        private string? _newSubTaskTitle;


        public ObservableCollection<ViewModelTask> Tasks { get; set; }
        public ViewModelAddTask(ITaskService taskService)
        {
            _taskService = taskService;
            Tasks = new ObservableCollection<ViewModelTask>(
                _taskService.Tasks.Select(task => new ViewModelTask(task))
            );
        }

        public ViewModelAddTask(ITaskService taskService, ViewModelTask task)
        {
            _taskService = taskService;
            Tasks = new ObservableCollection<ViewModelTask>(
                _taskService.Tasks.Select(task => new ViewModelTask(task))
            );

            NewTitle = task.Title;
            NewDescription = task.Description;
            NewPoints = task.Points;
            Deadline = task.DeadlineDate;
            Id = task.Id;
            //Init subtasks list
            foreach(ViewModelSubTask st in task.SubTasks)
            {
                SubTaskList.Add(st);
            }
            ButtonText = "Update task";
        }

        //Add check if subtasks title isnt empty 
        [RelayCommand]
        public void AddSubTask()
        {
            int count = SubTaskList.Count();
            string finalCount = $"{count}";
            SubTaskList.Add(new ViewModelSubTask() { Title = NewSubTaskTitle, IsChecked = false, ParentId = Id, Id = finalCount });
        }

        [RelayCommand]
        public void RemoveSubTask(ViewModelSubTask subTask)
        {
            if (subTask != null)
            {
                SubTaskList.Remove(subTask);
            }
        }

        //Add check if item is valid later
        //Using      [RelayCommand(CanExecute = nameof(CanAddItem))]
        [RelayCommand]
        private void AddItem()
        {
            var existingTask = Tasks.FirstOrDefault(t => t.Id == Id);
            if (existingTask != null)
            {
                //Updating old takse
                existingTask.Title = NewTitle;
                existingTask.Description = NewDescription;
                existingTask.Points = NewPoints;
                existingTask.DeadlineDate = Deadline?.Date;
                existingTask.SubTasks.Clear();

                foreach (var subTask in SubTaskList)
                {
                    existingTask.SubTasks.Add(subTask);
                }

                _taskService.UpdateTask(existingTask.GetTask());
            } else
            {
                var newTask = new ViewModelTask()
                {
                    Title = NewTitle,
                    IsChecked = false,
                    Points = NewPoints,
                    Description = NewDescription,
                    CreatedDate = DateTime.Now,
                    DeadlineDate = Deadline?.Date,
                    SubTasks = SubTaskList,
                    Id = Id,
                };
                _taskService.AddTask(newTask.GetTask());
                Tasks.Add(newTask);
            }
           
           
            //add confirmation
            // reset input fields
        }

       
    }
}
