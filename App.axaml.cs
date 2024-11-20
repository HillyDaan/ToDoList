using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using ToDolistVersion2.Services;
using ToDolistVersion2.ViewModels;
using ToDolistVersion2.Views;

namespace ToDolistVersion2
{
     public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            //Register all services needed
            var collection = new ServiceCollection();
            collection.AddCommonServices();

            //Create a service provider containing services from the provided collection
            var services = collection.BuildServiceProvider();

            var vm = services.GetRequiredService<ViewModelHome>();
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Line below is needed to remove Avalonia data validation.
                // Without this line you will get duplicate validations from both Avalonia and CT
                BindingPlugins.DataValidators.RemoveAt(0);
                desktop.MainWindow = new MainWindow
                {
                    DataContext = vm
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
