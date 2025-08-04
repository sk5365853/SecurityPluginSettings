using SecurityPluginSettings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityPluginSettings.Interface
{
    public interface ISettingsService
    {
        Task<List<SettingsData>> GetSettingsAsync();
    }
}
