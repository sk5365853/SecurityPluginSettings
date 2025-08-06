using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SecurityPluginSettings.Models;
using SecurityPluginSettings.Services;
using System.Net;
using System.Net.Http;

namespace SecurityPluginSettings.Tests.Services
{
    [TestClass]
    public class SettingsServiceTests
    {
        private const string EndpointUrl = "https://jsonplaceholder.typicode.com/posts";
        private HttpClient CreateMockHttpClient(HttpResponseMessage responseMessage, int failCount = 0)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            int callCount = 0;

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() =>
                {
                    callCount++;
                    if (callCount <= failCount)
                        return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    return responseMessage;
                });

            var client = new HttpClient(handlerMock.Object);
            return client;
        }

        [TestInitialize]
        public void TestInit()
        {
            System.Configuration.ConfigurationManager.AppSettings["SettingsApiUrl"] = EndpointUrl;
        }

        [TestMethod]
        public async Task GetSettingsAsync_ReturnsSettingsList_WhenResponseIsSuccessful()
        {
            var settings = new List<SettingsData>
            {
                new SettingsData { Id = 1, UserId = 2, Title = "Test", Body = "Body" }
            };
            var json = JsonConvert.SerializeObject(settings);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            };

            var httpClient = CreateMockHttpClient(response);
            var service = new SettingsService(httpClient);

            var result = await service.GetSettingsAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Test", result[0].Title);
        }

        [TestMethod]
        public async Task GetSettingsAsync_ReturnsEmptyList_WhenResponseIsEmpty()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("[]")
            };

            var httpClient = CreateMockHttpClient(response);
            var service = new SettingsService(httpClient);

            var result = await service.GetSettingsAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetSettingsAsync_TakesMaximum100Items()
        {
            var settings = new List<SettingsData>();
            for (int i = 0; i < 150; i++)
                settings.Add(new SettingsData { Id = i, UserId = i, Title = $"Title{i}", Body = $"Body{i}" });

            var json = JsonConvert.SerializeObject(settings);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            };

            var httpClient = CreateMockHttpClient(response);
            var service = new SettingsService(httpClient);

            var result = await service.GetSettingsAsync();

            Assert.AreEqual(100, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task GetSettingsAsync_ThrowsException_WhenAllRetriesFail()
        {
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var httpClient = CreateMockHttpClient(response, failCount: 3);
            var service = new SettingsService(httpClient);

            await service.GetSettingsAsync();
        }
    }
}
