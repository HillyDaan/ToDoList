﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDolistVersion2.Models;

namespace ToDolistVersion2.ViewModels
{
    public partial class ViewModelTask : ViewModelBase
    {
        [ObservableProperty]
        private string? _id;

        [ObservableProperty]
        private bool _isChecked;

        [ObservableProperty]
        private string? _title;

        [ObservableProperty]
        private int? _points;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private DateTime? _createdDate;

        [ObservableProperty]
        private DateTime? _deadlineDate;

        [ObservableProperty]
        private ObservableCollection<ViewModelSubTask> _subTasks = new();



        public ViewModelTask() { }

        public ViewModelTask(TaskModel task)
        {
            IsChecked = task.IsChecked;
            Title = task.Title;
            Points = task.Points;
            Description = task.Description;
            CreatedDate = task.Created;
            DeadlineDate = task.Deadline;
            Id = task.Id;

            // Initialize SubTasks from TaskModel
            if (task.SubTasks != null)
            {
                foreach (var subTask in task.SubTasks)
                {
                    SubTasks.Add(new ViewModelSubTask
                    {
                        Title = subTask.Title,
                        IsChecked = subTask.IsChecked,
                        Id = subTask.Id,
                        ParentId = subTask.ParentId,
                    });
                }
            }
        }

        public TaskModel GetTask()
        {
            return new TaskModel()
            {
                Title = this.Title,
                IsChecked = this.IsChecked,
                Points = this.Points,
                Description = this.Description,
                Created = this.CreatedDate,
                Deadline = this.DeadlineDate,
                Id = this.Id,
                SubTasks = new ObservableCollection<ViewModelSubTask>(
                    SubTasks.Select(st => new ViewModelSubTask()
                    {
                        Title = st.Title,
                        IsChecked = false, // Subtasks might have additional fields to sync
                        Id = st.Id,
                        ParentId = st.ParentId,
                    })
                )
            };
        }
    }
}
