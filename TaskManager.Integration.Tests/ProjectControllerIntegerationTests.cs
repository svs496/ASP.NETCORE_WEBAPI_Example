using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskManager.API;
using TaskManager.Entities;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.Integration.Tests
{
    public class ProjectControllerIntegerationTests :  IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public ProjectControllerIntegerationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }


        [Fact]
        public async Task Get_WhenCalled_ReturnsOkResultAsync()
        {
            var httpResponse = await _client.GetAsync("/api/project");
            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var tasks = JsonConvert.DeserializeObject<IEnumerable<Project>>(stringResponse);
            Assert.Contains(tasks, p => p.ProjectName == "ANGULAR PROJECT 1");

        }

        [Fact]
        public async Task GetById_UnknownProjectId_ReturnsNotFoundResultAsync()
        {
            // Act
            var notFoundResult = await _client.GetAsync("/api/prroject/299/");

            // Assert
            Assert.Equal("NotFound", notFoundResult.StatusCode.ToString());
        }

        [Fact]
        public async Task GetById_ExistingProjectId_MatchesTaskAsync()
        {
            var httpResponse = await _client.GetAsync("/api/project/2");

            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var project = JsonConvert.DeserializeObject<Project>(stringResponse);

            Assert.Equal("ANGULAR PROJECT 1", project.ProjectName);

        }

       




    }

}
