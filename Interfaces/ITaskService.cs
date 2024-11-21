﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDolistVersion2.Models;

namespace ToDolistVersion2.Interfaces
{
    public interface ITaskService
    {
        ObservableCollection<TaskModel> Tasks { get; }
        public void AddTask(TaskModel task);
    }
}