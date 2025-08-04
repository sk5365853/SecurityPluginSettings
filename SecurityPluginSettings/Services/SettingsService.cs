using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using SecurityPluginSettings.Interface;
using SecurityPluginSettings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SecurityPluginSettings.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        private readonly string _endpointUrl;

        public SettingsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _endpointUrl = System.Configuration.ConfigurationManager.AppSettings["SettingsApiUrl"];
            _retryPolicy = Policy
             .Handle<HttpRequestException>()
             .Or<TaskCanceledException>()
             .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
             .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2),
                 (outcome, timespan, retryCount, context) =>
                 {
                     Console.WriteLine($"Retry {retryCount} after {timespan.TotalSeconds} seconds due to: {outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}");
                 });
        }

        public async Task<List<SettingsData>> GetSettingsAsync()
        {
            var response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync(_endpointUrl));

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var settingsList = JsonConvert.DeserializeObject<List<SettingsData>>(json);

            return settingsList?.Take(100).ToList() ?? new List<SettingsData>();
        }

    }
}
