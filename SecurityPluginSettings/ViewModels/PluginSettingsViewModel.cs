using SecurityPluginSettings.Commands;
using SecurityPluginSettings.Interface;
using SecurityPluginSettings.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SecurityPluginSettings.ViewModels
{
    public class PluginSettingsViewModel : INotifyPropertyChanged
    {
        private readonly ISettingsService _settingsService;
        public ICommand ToggleCommand { get; }

        private bool _showId = true;


        public ObservableCollection<SettingsData> Posts { get; set; } = new ObservableCollection<SettingsData>();


        public PluginSettingsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            ToggleCommand = new RelayCommand(_ => Toggle());


        }
        public async Task InitializeAsync()
        {
            await LoadAndFillPostsAsync();
        }
        private async Task LoadAndFillPostsAsync()
        {
            var data = await LoadSettingsAsync();
            foreach (var item in data)
            {
                Posts.Add(item);
            }
        }

        public async Task<List<SettingsData>> LoadSettingsAsync()
        {
            try
            {
                return await _settingsService.GetSettingsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading settings: {ex.Message}");
                return new List<SettingsData>();
            }
        }

        public void Toggle()
        {
            _showId = !_showId;
            OnPropertyChanged(nameof(ShowIds));
        }
        public bool ShowIds => _showId;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

