using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using TaskManager.API;
using TaskManager.Entities;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.Integration.Tests
{
    public class TaskControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public TaskControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }


        [Fact]
        public async Task Get_WhenCalled_ReturnsOkResultAsync()
        {

            var httpResponse = await _client.GetAsync("/api/task");
            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var tasks = JsonConvert.DeserializeObject<IEnumerable<Entities.Task>>(stringResponse);
            Assert.Contains(tasks, p => p.TaskName == "Master Angular Task # 2");
           
        }

        [Fact]
        public async Task GetById_ExistingTaskId_MatchesTaskAsync()
        {
            var httpResponse = await _client.GetAsync("/api/task/2");

            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var task = JsonConvert.DeserializeObject<Entities.Task>(stringResponse);

            Assert.Equal("Master Angular Task # 2", task.TaskName);

        }

        [Fact]
        public async Task GetById_UnknownTaskId_ReturnsNotFoundResultAsync()
        {
            // Act
            var notFoundResult = await _client.GetAsync("/api/task/299");

            // Assert
            Assert.Equal("NotFound", notFoundResult.StatusCode.ToString());
        }


        [Fact]
        public async Task Edit_ExistingTaskId_ReturnsOkAsync()
        {
            // Arrange
            var request = new
            {
                Url = "/api/task/1",
                Body = new Entities.Task()
                {
                    TaskId = 1,
                    TaskName = "Update Master Task Test",
                    CreateTime = DateTime.Now,
                    Status = Statuses.InProgress,
                    ParentTaskId = 0,
                    Priority = 5,
                    EndDate = DateTime.Now.Date.AddDays(222),
                    StartDate = DateTime.Now.Date.AddDays(223)
                }
            };

            //Act
            var httpResponse = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            httpResponse.EnsureSuccessStatusCode();

            //Get Task By Id and check if it is updated.
            httpResponse = await _client.GetAsync("/api/task/1");
            
            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var task = JsonConvert.DeserializeObject<Entities.Task>(stringResponse);

            Assert.Equal("Update Master Task Test", task.TaskName);

        }


        [Fact]
        public async Task Add_ValidObject_ReturnsCreatedResponseAsync()
        {
            // Arrange
            var request = new
            {
                Url = "/api/task/",
                Body = new Entities.Task()
                {
                    TaskName = "New task from Integeration",
                    CreateTime = DateTime.Now,
                    Status = Statuses.InProgress,
                    ParentTaskId = 0,
                    Priority = 5,
                    EndDate = DateTime.Now.Date.AddDays(222),
                    StartDate = DateTime.Now.Date.AddDays(223)
                }
            };

            // Act
            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            // Assert
            response.EnsureSuccessStatusCode();

            // Assert
            Assert.Equal("Created", response.StatusCode.ToString());

            // check task created matches
            var stringResponse = await response.Content.ReadAsStringAsync();
            var newtask = JsonConvert.DeserializeObject<Entities.Task>(stringResponse);

            Assert.Equal(request.Body.TaskName, newtask.TaskName);

        }

        [Fact]
        public async Task Add_InValidObject_ReturnsBadResponseAsync()
        {
            // Arrange
            var request = new
            {
                Url = "/api/task/",
                Body = new Entities.Task()
                {
                    //TaskName = "New task from Integeration",
                    CreateTime = DateTime.Now,
                    Status = Statuses.InProgress,
                    ParentTaskId = 0,
                    Priority = 5,
                    EndDate = DateTime.Now.Date.AddDays(222),
                    StartDate = DateTime.Now.Date.AddDays(223)
                }
            };

            // Act
            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            //Assert
            Assert.Equal("BadRequest", response.StatusCode.ToString());

        }


        [Fact]
        public async Task Delete_NotExistingTaskId_ReturnsNotFoundResponseAsync()
        {
            // Arrange
            int notExistingId = 900;

            // Act
            var badResponse = await _client.DeleteAsync("/api/task/" + notExistingId);

            //Assert
            Assert.Equal("NotFound", badResponse.StatusCode.ToString());
        }


        [Fact]
        public async Task Delete_ExistingTaskIdWithChild_ReturnsConflictResultAsync()
        {
            // Arrange
            int existingId = 1;

            // Act
            var badResponse = await _client.DeleteAsync("/api/task/" + existingId);

            //Assert
            Assert.Equal("Conflict", badResponse.StatusCode.ToString());
        }


        [Fact]
        public async Task Delete_ExistingTaskID_ReturnsOkResultAsync()
        {
            // Arrange
            int existingId = 4;
            
            // Act
            var badResponse = await _client.DeleteAsync("/api/task/" + existingId);

            //Assert
            Assert.Equal("NoContent", badResponse.StatusCode.ToString());

            //check
            // Act
            var notFoundResult = await _client.GetAsync("/api/task/4");

            // Assert
            Assert.Equal("NotFound", notFoundResult.StatusCode.ToString());
        }


    }
}
