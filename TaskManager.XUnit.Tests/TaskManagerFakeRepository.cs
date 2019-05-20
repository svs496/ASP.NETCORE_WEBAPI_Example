using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManager.Contracts;
using TaskManager.Entities;

namespace TaskManager.XUnit.Tests
{
    public class TaskManagerFakeRepository : IDataRepository<Task>
    {

        private readonly List<Task> _tasks;
        private readonly List<Project> _projects;
        private readonly List<User> _users;

        public TaskManagerFakeRepository()
        {
            _users = new List<User>()
            {
                new User() {EmployeeID = "A12345", LastName = "BHATIA", FirstName ="SATYAM", UserId = 1 },
                new User() {EmployeeID = "B12345", LastName = "KHAN", FirstName ="SALMAN",  UserId = 2 },
                new User() {EmployeeID = "C12345", LastName = "DHONI", FirstName ="MAHI",  UserId = 3 },
                new User() {EmployeeID = "D12345", LastName = "KOHLI", FirstName ="VIRAT",  UserId = 4 },
                new User() {EmployeeID = "E12345", LastName = "SHARMA", FirstName ="KAPIL",  UserId = 5 }
            };


            _projects = new List<Project>()
            {
                new Project() {ProjectId = 1, ProjectName = "TEST PROJECT 1", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(6), Priority = 30, IsSuspended = false, UserId = 2},
                new Project() {ProjectId = 21, ProjectName = "ANGULAR PROJECT 1", StartDate = DateTime.Now.AddDays(2), EndDate = DateTime.Now.AddDays(11), Priority = 30, IsSuspended = false, UserId = 4},
                new Project() {ProjectId = 31, ProjectName = "API PROJECT 1", StartDate = DateTime.Now.AddDays(1), EndDate = DateTime.Now.AddDays(5), Priority = 30, IsSuspended = false, UserId = 4},
                new Project() {ProjectId = 41, ProjectName = "DEPLOY PROJECT 1", StartDate = DateTime.Now,EndDate = DateTime.Now.AddDays(10), Priority = 30, IsSuspended = true, UserId = 5},
            };

            _tasks = new List<Task>()
            {
                new Task(){
                TaskId = 1,
                TaskName = "Master API Task # 1",
                CreateTime = DateTime.Now,
                Status = Statuses.InProgress,
                ParentTaskId = 0,
                Priority = 25,
                EndDate = DateTime.Now.Date.AddDays(110),
                StartDate = DateTime.Now.Date.AddDays(15),
                ProjectId = 21,
                UserId =1
                },
                new Task() {
                TaskId = 2,
                TaskName = "Master Angular Task # 2",
                CreateTime = DateTime.Now,
                Status = Statuses.InProgress,
                ParentTaskId = 0,
                Priority = 30,
                EndDate = DateTime.Now.Date.AddDays(15),
                StartDate = DateTime.Now.Date,
                ProjectId = 31,
                UserId =5},
                new Task() {
                TaskId = 3,
                TaskName = "Child Task # 1",
                CreateTime = DateTime.Now,
                Status = Statuses.InProgress,
                ParentTaskId = 1,
                Priority = 15,
                EndDate = DateTime.Now.Date.AddDays(110),
                StartDate = DateTime.Now.Date.AddDays(15),
                ProjectId = 1,
                UserId = 4
                },
                new Task() { TaskId = 4,
                TaskName = "Child Task # 2",
                CreateTime = DateTime.Now,
                Status = Statuses.InProgress,
                ParentTaskId = 1,
                Priority = 19,
                EndDate = DateTime.Now.Date.AddDays(44),
                StartDate = DateTime.Now.Date.AddDays(15) ,
                ProjectId = 1,
                UserId = 4
                },
                new Task()  {
                TaskId = 5,
                TaskName = "Child of Master Task# 2",
                CreateTime = DateTime.Now,
                Status = Statuses.InProgress,
                ParentTaskId = 2,
                Priority = 11,
                EndDate = DateTime.Now.Date.AddDays(2),
                StartDate = DateTime.Now.Date,
                ProjectId = 1,
                UserId = 5}
            };
        }

        public void Add(Task entity)
        {
            entity.TaskId = _tasks.Count() + 1;
            _tasks.Add(entity);
        }

        public bool CanDeleteEntity(long taskId)
        {
            return _tasks.Any(p => p.ParentTaskId == taskId);
        }

        public void Delete(Task entity)
        {
            _tasks.Remove(entity);
        }

        public Task Get(long id)
        {
            return _tasks.Where(a => a.TaskId == id)
           .FirstOrDefault();
        }

        public IEnumerable<Task> GetAll()
        {
            return _tasks;
        }

        public IEnumerable<Task> GetListById(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Task> GetParentTasks()
        {
            return _tasks.Select(p => new Task { TaskId = p.TaskId, TaskName = p.TaskName })
               .Distinct().OrderBy(k => k.TaskName).ToList();
        }

        public void Update(Task entity)
        {
            var indexOf = _tasks.IndexOf(_tasks.Find(p => p.TaskId == entity.TaskId));
            _tasks[indexOf] = entity;

        }
    }
}
