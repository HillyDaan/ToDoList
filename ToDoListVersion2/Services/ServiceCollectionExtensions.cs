using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDolistVersion2.ViewModels;
using ToDolistVersion2.Interfaces;

namespace ToDolistVersion2.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection services)
        {
            services.AddSingleton<ITaskService, TaskService>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<ViewModelHome>();
            services.AddTransient<ViewModelAddTask>();
            services.AddTransient<ViewModelAllTask>();
        }
    }
}
