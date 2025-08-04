using SecurityPluginSettings.ViewModels;
using System.Windows;

namespace SecurityPluginSettings
{

    public partial class MainWindow : Window
    {
        private readonly PluginSettingsViewModel _viewModel;
        public MainWindow(PluginSettingsViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            Loaded += MainWindow_Loaded;
        }
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.InitializeAsync();
        }
    }

}
