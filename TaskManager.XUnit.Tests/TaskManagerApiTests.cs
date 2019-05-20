using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.API.Controllers;
using TaskManager.Contracts;
using TaskManager.Entities;
using TaskManager.LoggerService;
using Xunit;

namespace TaskManager.XUnit.Tests
{
    public class TaskManagerApiTests
    {
        readonly ILoggerManager _logger;
        readonly IDataRepository<Task> _service;
        readonly TaskController _controller;

        public TaskManagerApiTests()
        {
            _logger = new LoggerManager();
            _service = new TaskManagerFakeRepository();
            _controller = new TaskController(_logger, _service);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = _controller.Get();

            // Assert
            Assert.IsType<OkObjectResult>(okResult);

            Assert.NotNull(okResult);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var results = _controller.Get();

            //Assert
            Assert.IsType<OkObjectResult>(results);

            Assert.NotNull(results);

            var okObjectResult = results as OkObjectResult;
            // Assert
            var tasks = okObjectResult.Value as IEnumerable<Task>;

            Assert.Equal(5, tasks.Count());  //5 is original count in FakeRepo
        }

        [Fact]
        public void GetById_UnknownTaskId_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = _controller.Get(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult);
        }


        [Fact]
        public void GetById_ExistingTaskId_ReturnsOkResult()
        {
            // Arrange
            int taskId = 2;

            // Act
            var data = _controller.Get(taskId);

            // Assert
            Assert.IsType<OkObjectResult>(data);

            Assert.NotNull(data);

        }

        [Fact]
        public void GetById_ExistingTaskId_MatchesTask()
        {
            // Arrange
            int taskId = 2;

            // Act
            var data = _controller.Get(taskId);

            // Assert
            Assert.IsType<OkObjectResult>(data);

            var okObjectResult = data as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var task = okObjectResult.Value as Entities.Task;

            Assert.Equal("Master Angular Task # 2", task.TaskName);

        }

        [Fact]
        public void Edit_NotExistingTaskId_ReturnsBadRequest()
        {
            // Arrange
            var editTask = new Task()
            {
                TaskId = 1,
                TaskName = "Update test",
                CreateTime = DateTime.Now,
                Status = Statuses.InProgress,
                ParentTaskId = 0,
                Priority = 5,
                EndDate = DateTime.Now.Date.AddDays(222),
                StartDate = DateTime.Now.Date.AddDays(223)
            };

            // Act
            var badResponse = _controller.Put(100,editTask);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);

        }

        [Fact]
        public void Edit_ExistingTaskId_ReturnsOk()
        {
            // Arrange
            var editTask = new Task()
            {
                TaskId = 1,
                TaskName = "Update Master Task Test",
                CreateTime = DateTime.Now,
                Status = Statuses.InProgress,
                ParentTaskId = 0,
                Priority = 5,
                EndDate = DateTime.Now.Date.AddDays(222),
                StartDate = DateTime.Now.Date.AddDays(223)
            };

            // Act
            var response = _controller.Put(1, editTask);

            // Assert
            Assert.IsType<NoContentResult>(response);

        }


        [Fact]
        public void Edit_ExistingTaskId_MatchResult()
        {
            // Arrange
            var editTask = new Task()
            {
                TaskId = 1,
                TaskName = "Update Master Task Test",
                CreateTime = DateTime.Now,
                Status = Statuses.InProgress,
                ParentTaskId = 0,
                Priority = 5,
                EndDate = DateTime.Now.Date.AddDays(222),
                StartDate = DateTime.Now.Date.AddDays(223)
            };

            // Act
            var response = _controller.Put(1, editTask);

            // Assert
            Assert.IsType<NoContentResult>(response);

            // Act
            var data = _controller.Get(1);

            // Assert
            Assert.IsType<OkObjectResult>(data);

            var okObjectResult = data as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var task = okObjectResult.Value as Entities.Task;

            Assert.Equal("Update Master Task Test", task.TaskName);
        }



        [Fact]
        public void Add_InvalidObject_ReturnsBadRequest()
        {
            // Arrange
            var nameMissingItem = new Task()
            {
                TaskId = 1,
                //TaskName = "Master API Task # 1",
                CreateTime = DateTime.Now,
                Status = Statuses.InProgress,
                ParentTaskId = 0,
                Priority = 25,
                EndDate = DateTime.Now.Date.AddDays(110),
                StartDate = DateTime.Now.Date.AddDays(15)
            };
            _controller.ModelState.AddModelError("TaskName", "Required");

            // Act
            var badResponse = _controller.Post(nameMissingItem);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);

            _controller.ModelState.Clear();
        }

        [Fact]
        public void Add_ValidObject_ReturnsCreatedResponse()
        {
            // Arrange
            var testTask = new Task()
            {
                TaskId = 1,
                TaskName = "New Task from Test",
                CreateTime = DateTime.Now,
                Status = Statuses.InProgress,
                ParentTaskId = 0,
                Priority = 25,
                EndDate = DateTime.Now.Date.AddDays(110),
                StartDate = DateTime.Now.Date.AddDays(15)
            };

            //Act
            IActionResult createdResponse = _controller.Post(testTask);

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);

        }

        [Fact]
        public void Add_TaskWithoutProjectAndUserIDPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            var testTask = new Task()
            {
                TaskId = 1,
                TaskName = "New Task from Test",
                CreateTime = DateTime.Now,
                Status = Statuses.InProgress,
                ParentTaskId = 0,
                Priority = 25,
                EndDate = DateTime.Now.Date.AddDays(110),
                StartDate = DateTime.Now.Date.AddDays(15)
            };

            // Act
            var createdResponse = _controller.Post(testTask) as CreatedAtActionResult;
            var item = createdResponse.Value as Task;

            // Assert
            Assert.IsType<Task>(item);
            Assert.Equal(testTask.TaskName.ToUpper(), item.TaskName);
        }

        [Fact]
        public void Add_TaskWithProjectAndUserIDPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            var testTask = new Task()
            {
                TaskId = 1,
                TaskName = "New Task from Test",
                CreateTime = DateTime.Now,
                Status = Statuses.InProgress,
                ParentTaskId = 0,
                Priority = 25,
                EndDate = DateTime.Now.Date.AddDays(110),
                StartDate = DateTime.Now.Date.AddDays(15),
                ProjectId = 1,
                UserId = 1
            };

            // Act
            var createdResponse = _controller.Post(testTask) as CreatedAtActionResult;
            var item = createdResponse.Value as Task;

            // Assert
            Assert.IsType<Task>(item);
            Assert.Equal(testTask.ProjectId, item.ProjectId);
        }



        [Fact]
        public void Delete_NotExistingTaskId_ReturnsNotFoundResponse()
        {
            // Arrange
            int notExistingId = 900;

            // Act
            var badResponse = _controller.Delete(notExistingId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(badResponse);
        }

        [Fact]
        public void Delete_ExistingTaskId_ReturnsOkResult()
        {
            // Arrange
            int existingId = 3;

            // Act
            var okResponse = _controller.Delete(existingId);

            // Assert
            Assert.IsType<NoContentResult>(okResponse);
        }

        [Fact]
        public void Delete_ExistingTaskIdWithChild_ReturnsConflictResult()
        {
            // Arrange
            int existingId = 1;

            // Act
            var okResponse = _controller.Delete(existingId);

            // Assert
            Assert.IsType<ConflictObjectResult>(okResponse);
        }

    }
}
