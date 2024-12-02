// Filename: ViewModelSubTask.cs
// Description: Contains the ViewModelSubTask class, which represents a subtask in the ToDo List app. 
//              It provides properties for the subtask's ID, title, completion status, points, and parent task ID,
//              and methods to convert between the ViewModel and the SubTaskModel.
//              Linked to SubTaskModel.cs

using CommunityToolkit.Mvvm.ComponentModel;
using System;
using ToDolistVersion2.Models;

namespace ToDolistVersion2.ViewModels
{
    public partial class ViewModelSubTask : ViewModelBase
    {
        [ObservableProperty]
        private string? _parentId;

        [ObservableProperty]
        private string? _title;

        [ObservableProperty] 
        private string? _id;

        [ObservableProperty]
        private Boolean _isChecked;

        [ObservableProperty]
        private int? _points;

        public ViewModelSubTask() { }

        public ViewModelSubTask(SubTaskModel subTask)
        {
            this.Title = subTask.Title;
            this.IsChecked = subTask.IsChecked;
            this.Id = subTask.Id;
            this.ParentId = subTask.ParentId;
            this.Points = subTask.Points;
        }

        public SubTaskModel GetSubTask()
        {
            return new SubTaskModel() { Id = Id, Title = Title, IsChecked = IsChecked, ParentId = ParentId, Points = Points };
        }


    }
}
