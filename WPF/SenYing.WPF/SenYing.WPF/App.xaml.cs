using System.Windows;
using System.Windows.Threading;

using CommunityToolkit.Mvvm.Messaging;


using MaterialDesignThemes.Wpf;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SenYing.Services;
using SenYing.Services.IServices;
using SenYing.WPF.ViewModels;
using SenYing.WPF.ViewModels.UserControls;
using SenYing.WPF.Views;
using SenYing.WPF.Views.UserControls;

namespace SenYing.WPF
{


    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        [STAThread]
        private static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }
        private static async Task MainAsync(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            await host.StartAsync().ConfigureAwait(true);

            ServiceProvider = host.Services;
            App app = new();
            app.InitializeComponent();
            app.MainWindow = host.Services.GetRequiredService<MainWindow>();
            app.MainWindow.Visibility = Visibility.Visible;
            app.Run();

            await host.StopAsync().ConfigureAwait(true);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder)
                => configurationBuilder.AddUserSecrets(typeof(App).Assembly))
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<IndexUserControl>();
                services.AddSingleton<IndexUserControlVm>();
                services.AddSingleton<SearchViewUserControl>();
                services.AddSingleton<SearchViewUserControlVm>();
                services.AddTransient<VideoWindow>();
                services.AddTransient<VideoWindowViewModel>();
                services.AddSingleton<IM3u8Service,M3u8Service>();



                services.AddSingleton<WeakReferenceMessenger>();
                services.AddSingleton<IMessenger, WeakReferenceMessenger>(provider => provider.GetRequiredService<WeakReferenceMessenger>());

                services.AddSingleton(_ => Current.Dispatcher);

                services.AddTransient<ISnackbarMessageQueue>(provider =>
                {
                    Dispatcher dispatcher = provider.GetRequiredService<Dispatcher>();
                    return new SnackbarMessageQueue(TimeSpan.FromSeconds(3.0), dispatcher);
                });
            });
    }
}
