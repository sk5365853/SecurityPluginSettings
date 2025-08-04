using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SecurityPluginSettings.Interface;
using System.Windows;

namespace SecurityPluginSettings
{
    public partial class App : Application
    {
        public static IHost Host { get; private set; }

        public App()
        {
            Host = new HostBuilder()
                .ConfigureServices(ConfigureServices)
                .UseConsoleLifetime()
                .Build();
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddHttpClient<ISettingsService, Services.SettingsService>();
            services.AddSingleton<ViewModels.PluginSettingsViewModel>();
            services.AddSingleton<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Host.Start();

            var mainWindow = Host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
