using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using TaskManager.API;
using TaskManager.Entities;
using Xunit;
using Task = System.Threading.Tasks.Task;


namespace TaskManager.Integration.Tests
{
    public class UserControllerIntegerationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public UserControllerIntegerationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_WhenCalled_ReturnsOkResultAsync()
        {
            var httpResponse = await _client.GetAsync("/api/user");
            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var tasks = JsonConvert.DeserializeObject<IEnumerable<User>>(stringResponse);
            Assert.Contains(tasks, p => p.FirstName == "KAPIL");
        }

        [Fact]
        public async Task GetById_UnknownUserId_ReturnsNotFoundResultAsync()
        {
            // Act
            var notFoundResult = await _client.GetAsync("/api/user/299/");

            // Assert
            Assert.Equal("NotFound", notFoundResult.StatusCode.ToString());
        }

        [Fact]
        public async Task GetById_ExistingUserId_MatchesTaskAsync()
        {
            var httpResponse = await _client.GetAsync("/api/user/2");

            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<Entities.User>(stringResponse);

            Assert.Equal("VIRAT", user.FirstName );

        }

        [Fact]
        public async Task Edit_ExistingUserId_ReturnsOkAsync()
        {
            // Arrange
            var request = new
            {
                Url = "api/user/1",
                Body = new User()
                {
                    UserId = 1,
                    EmployeeID = "Z0000",
                    LastName = "BHATIA",
                    FirstName = "MAYTAS"
                }
            };

            //Act
            var httpResponse = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            httpResponse.EnsureSuccessStatusCode();

            //Get Task By Id and check if it is updated.
            httpResponse = await _client.GetAsync("/api/user/1");

            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(stringResponse);

            Assert.Equal(request.Body.EmployeeID, user.EmployeeID);

        }




    }
}
