using SecurityPluginSettings.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecurityPluginSettings.Interface
{
    public interface ISettingsService
    {
        Task<List<SettingsData>> GetSettingsAsync();
    }
}
