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
using ToDolistVersion2.Models;

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
        [NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
        private string? _newTitle;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
        private string? _newDescription;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
        private int? _newPoints;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
        private DateTimeOffset? _deadline;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddSubTaskCommand))]
        private string? _newSubTaskTitle;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddSubTaskCommand))]
        private int? _newSubTaskPoints;


        public ObservableCollection<ViewModelTask> Tasks { get; set; }
        public ViewModelAddTask(ITaskService taskService)
        {
            _taskService = taskService;
            Tasks = new ObservableCollection<ViewModelTask>(
                _taskService.Tasks.Select(task => new ViewModelTask(task))
            );
            Console.WriteLine($"AddSubTaskCommand.CanExecute: {AddSubTaskCommand.CanExecute(null)}");
            AddSubTaskCommand.NotifyCanExecuteChanged();

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
            AddSubTaskCommand.NotifyCanExecuteChanged();
        }
        private bool CanAddSubTask() 
        {
            bool canAddSubTask = true;
            canAddSubTask = canAddSubTask && !string.IsNullOrWhiteSpace(NewSubTaskTitle);
            canAddSubTask = canAddSubTask && NewSubTaskPoints > 0;

            return canAddSubTask;
        }

        //Add check if subtasks title isnt empty 
        [RelayCommand(CanExecute = nameof(CanAddSubTask))]
        public void AddSubTask()
        {
            int count = SubTaskList.Count();
            string finalCount = $"{count}";

            SubTaskList.Add(new ViewModelSubTask() { Title = NewSubTaskTitle, IsChecked = false, ParentId = Id, Id = finalCount, Points = NewSubTaskPoints });
            //Reset input fields
            NewSubTaskTitle = null;
            NewSubTaskPoints = null;
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
        [RelayCommand(CanExecute = nameof(CanAddItem))]
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
                //Reset input fields
                ClearFields();
            }
        }

        private void ClearFields()
        {
            NewTitle = null;
            NewDescription = null;
            NewPoints = null;
            Deadline = null;
            SubTaskList.Clear();
        }
        private bool CanAddItem()
        {
            bool canAddItem = true;
            //Check title
            canAddItem = canAddItem && !string.IsNullOrWhiteSpace(NewTitle);
            //Check description
            canAddItem = canAddItem && !string.IsNullOrEmpty(NewDescription);
            //Check severity, int between 1 - 10
            canAddItem = canAddItem && NewPoints >= 1 && NewPoints <= 10;
            //Check deadline date
            canAddItem = canAddItem && Deadline.HasValue && Deadline.Value > DateTime.Now;
            return canAddItem;
        }
  
    }
}
