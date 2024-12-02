// Filename: ServiceCollectionExtensions.cs
// Description: Contains an extension method for IServiceCollection to register common services in the ToDo List app. 
//              It adds services for dependency injection, including the TaskService (singleton), 
//              and various ViewModels (transient). The method provides a convenient way to configure 
//              the application's dependencies and ensures that the appropriate lifetimes are used for each service.


using Microsoft.Extensions.DependencyInjection;
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
