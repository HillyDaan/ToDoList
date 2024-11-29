using System.Collections.ObjectModel;
using System.Linq;
using ToDolistVersion2.Models;
using ToDolistVersion2.Services;
using ToDolistVersion2.Interfaces;
using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ToDolistVersion2.ViewModels
{
    public partial class ViewModelHome : ViewModelBase
    {
        private readonly ITaskService _taskService;
        public ObservableCollection<ViewModelTask> Tasks { get; }

        public ObservableCollection<ViewModelTask> TopTasks  { get; set; }

       // public ObservableCollection<SubTaskModel> TopTasks { get; }

        public ViewModelHome(ITaskService taskService) {
            _taskService = taskService;
            //Convert tasks to ViewTaskmodel
            Tasks = new ObservableCollection<ViewModelTask>(
               _taskService.Tasks.Select(task => new ViewModelTask(task))
            );

            TopTasks = new ObservableCollection<ViewModelTask> { };
            //Calculate top tasks
            CalculateTopTasks(5);
        }

        public void CalculateTopTasks(int n)
        {
            // top n
            //We have severity 1 (low) - 10 (high) for total task
            //Subtasks each have points: 1 points == 1 hour
            //Deadline - currentdate
            //Three categories
            //< 3 days 
            // 3 - 7 days
            // > 7 days


            //Get current date
            DateTime currentDate = DateTime.Now;

            var scoredTasks = Tasks
                .Where(tasks => tasks.DeadlineDate.HasValue) //Only grab valid deadlines
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

    }
}
