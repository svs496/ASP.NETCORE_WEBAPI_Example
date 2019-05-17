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

        public TaskManagerFakeRepository()
        {
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
                StartDate = DateTime.Now.Date.AddDays(15) },
                 new Task() {
                TaskId = 2,
                TaskName = "Master Angular Task # 2",
                CreateTime = DateTime.Now,
                Status = Statuses.InProgress,
                ParentTaskId = 0,
                Priority = 30,
                EndDate = DateTime.Now.Date.AddDays(15),
                StartDate = DateTime.Now.Date },
                new Task() {
                TaskId = 3,
                TaskName = "Child Task # 1",
                CreateTime = DateTime.Now,
                Status = Statuses.InProgress,
                ParentTaskId = 1,
                Priority = 15,
                EndDate = DateTime.Now.Date.AddDays(110),
                StartDate = DateTime.Now.Date.AddDays(15) },
                new Task() { TaskId = 4,
                TaskName = "Child Task # 2",
                CreateTime = DateTime.Now,
                Status = Statuses.InProgress,
                ParentTaskId = 1,
                Priority = 19,
                EndDate = DateTime.Now.Date.AddDays(44),
                StartDate = DateTime.Now.Date.AddDays(15) },
                new Task()  {
                TaskId = 5,
                TaskName = "Child of Master Task# 2",
                CreateTime = DateTime.Now,
                Status = Statuses.InProgress,
                ParentTaskId = 2,
                Priority = 11,
                EndDate = DateTime.Now.Date.AddDays(2),
                StartDate = DateTime.Now.Date }
            };
        }

        public void Add(Task entity)
        {
            entity.TaskId = _tasks.Count() + 1;
            _tasks.Add(entity);
        }

        public bool ChildTaskExits(long taskId)
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
