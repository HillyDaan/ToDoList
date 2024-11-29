﻿using System.Collections.ObjectModel;
using System.Linq;
using ToDolistVersion2.Models;
using ToDolistVersion2.Services;
using ToDolistVersion2.Interfaces;
using System;
using CommunityToolkit.Mvvm.ComponentModel;

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace ToDolistVersion2.ViewModels
{
    public partial class ViewModelHome : ViewModelBase
    {
        private readonly ITaskService _taskService;
        public ObservableCollection<ViewModelTask> Tasks { get; }

        public ObservableCollection<ViewModelTask> TopTasks  { get; set; }

        public ISeries[] PieData { get; set; }

        public ObservableCollection<ViewModelTask> Deadlines { get; set; }

      
        //This file could be optimised alot i think, i do alot of passes over all task

        public ViewModelHome(ITaskService taskService) {
            _taskService = taskService;
            //Convert tasks to ViewTaskmodel
            Tasks = new ObservableCollection<ViewModelTask>(
               _taskService.Tasks.Select(task => new ViewModelTask(task))
            );

            TopTasks = new ObservableCollection<ViewModelTask> { };
            Deadlines = new ObservableCollection<ViewModelTask> { };
            //Calculate top tasks
            CalculateTopTasks(5);
            //Create pie graph
            CreatePieChart();
            //Nearest Deadlines
            GetNearestDeadlines(5);

        }

        [RelayCommand]
        public void UpdateSubTaskStatus(ViewModelSubTask subTask)
        {
            _taskService.CheckOffSubTask(subTask.GetSubTask(), subTask.IsChecked);
        }

        [RelayCommand]
        public void UpdateTaskStatus(ViewModelTask task)
        {
            //Set viewModel change for each subtask
            foreach (ViewModelSubTask subTask in task.SubTasks)
            {
                subTask.IsChecked = task.IsChecked;
            }
            //Viewmodel already changed, inform taskService of change
            _taskService.CheckOffTask(task.GetTask(), task.IsChecked);

        }

        public void GetNearestDeadlines(int n)
        {
            //Get current date
            DateTime currentDate = DateTime.Now;

            var deadlineTasks = Tasks
                .Where(tasks => tasks.DeadlineDate.HasValue)
                .Where(tasks => tasks.IsActive())
                .Select(task =>
                {
                    int daysToDeadline = (int)(task.DeadlineDate.Value - currentDate).TotalDays;
                    return (new
                    {
                        daysToDealine = daysToDeadline,
                        Task = task
                    });

                }).OrderBy(x => x.daysToDealine)
                .Take(n)
                .ToList();
            //Clear and set toptasks
            Deadlines.Clear();
            foreach (var task in deadlineTasks) 
            {

                Deadlines.Add(task.Task);
            }
        }

        public void CalculateTopTasks(int n)
        {

            //Get current date
            DateTime currentDate = DateTime.Now;

            var scoredTasks = Tasks
                .Where(tasks => tasks.DeadlineDate.HasValue) //Only grab valid deadlines
                .Where(tasks => tasks.IsActive())
                .Select(task =>
                {
                    //Get total subtasks points
                    int totalSubTaskPoints = task.SubTasks.Sum(subTask => subTask.Points ?? 0); //default of 0 if points not set

                    //Calculate days to deadline
                    int daysToDeadline = (int)(task.DeadlineDate.Value - currentDate).TotalDays;

                    //Calculate deadline score
                    int deadlineScore = daysToDeadline switch
                    {
                        < 3 => 10,
                        <= 7 => 5,
                        _ => 1
                    };

                    //Severity mulitplier
                    int severity = task.Points ?? 1; //1 if null

                    //Total score
                    double taskScore = severity * (10.0 / (1 + totalSubTaskPoints)) + deadlineScore;

                    return new
                    {
                        Task = task,
                        Score = taskScore,
                    };

                })
                .OrderByDescending(x => x.Score)
                .Take(n)
                .ToList();

            //Clear and set toptasks
            TopTasks.Clear();
            foreach(var task in scoredTasks)
            {
                
                TopTasks.Add(task.Task);
            }
        }

        private void CreatePieChart()
        {
           
            // Categories - Calculate the points for each category
            int pointsLessThan3Days = Tasks
                .Where(task => task.DeadlineDate.HasValue && (task.DeadlineDate.Value - DateTime.Now).TotalDays < 3)
                .Where(tasks => tasks.IsActive())
                .Sum(task => task.SubTasks.Sum(subTask => subTask.Points ?? 0));

            int pointsBetween3and7Days = Tasks
                .Where(task => task.DeadlineDate.HasValue && (task.DeadlineDate.Value - DateTime.Now).TotalDays >= 3 && (task.DeadlineDate.Value - DateTime.Now).TotalDays <= 7)
                .Where(tasks => tasks.IsActive())
                .Sum(task => task.SubTasks.Sum(subTask => subTask.Points ?? 0));

            int pointsGreaterThan7Days = Tasks
                .Where(task => task.DeadlineDate.HasValue && (task.DeadlineDate.Value - DateTime.Now).TotalDays > 7)
                .Where(tasks => tasks.IsActive())
                .Sum(task => task.SubTasks.Sum(subTask => subTask.Points ?? 0));

            PieData = new ISeries[]
            {
                new PieSeries<int>
                {
                    Values = new [] {pointsLessThan3Days},
                    Name = "Less then 3 days",
                    Fill = new SolidColorPaint(SKColors.Red)
                },
                new PieSeries<int>
                {
                    Values = new [] {pointsBetween3and7Days},
                    Name = "Between 3 and 7 days",
                    Fill = new SolidColorPaint(SKColors.Blue)
                },
                new PieSeries<int>
                {
                    Values = new [] {pointsGreaterThan7Days},
                    Name = "Greater then 7 days",
                    Fill = new SolidColorPaint(SKColors.Green)
                }
            };
        }

    }
}
