using System;
using TaskManager.DataLayer;

namespace TaskManager.Integration.Tests
{
    public static class SeedData
    {
        public static void PopulateTestData(TaskManagerContext dbContext)
        {
            dbContext.Tasks.Add(new Entities.Task
            {
                //TaskId = 1,
                TaskName = "Master API Task # 1",
                CreateTime = DateTime.Now,
                Status = Entities.Statuses.InProgress,
                ParentTaskId = 0,
                Priority = 25,
                EndDate = DateTime.Now.Date.AddDays(110),
                StartDate = DateTime.Now.Date.AddDays(15)
            });
            dbContext.Tasks.Add(new Entities.Task
            {
                // TaskId = 2,
                TaskName = "Master Angular Task # 2",
                CreateTime = DateTime.Now,
                Status = Entities.Statuses.InProgress,
                ParentTaskId = 0,
                Priority = 2,
                EndDate = DateTime.Now.Date.AddDays(15),
                StartDate = DateTime.Now.Date
            });
            dbContext.Tasks.Add(new Entities.Task
            {
                //TaskId = 3,
                TaskName = "Child Task # 1",
                CreateTime = DateTime.Now,
                Status = Entities.Statuses.InProgress,
                ParentTaskId = 1,
                Priority = 19,
                EndDate = DateTime.Now.Date.AddDays(44),
                StartDate = DateTime.Now.Date.AddDays(15)
            });
            dbContext.Tasks.Add(new Entities.Task
            {
                // TaskId = 4,
                TaskName = "Child Task # 2",
                CreateTime = DateTime.Now,
                Status = Entities.Statuses.InProgress,
                ParentTaskId = 2,
                Priority = 11,
                EndDate = DateTime.Now.Date.AddDays(2),
                StartDate = DateTime.Now.Date
            });
            dbContext.Tasks.Add(new Entities.Task
            {
                // TaskId = 5,
                TaskName = "Child Task # 3",
                CreateTime = DateTime.Now,
                Status = Entities.Statuses.InProgress,
                ParentTaskId = 1,
                Priority = 30,
                EndDate = DateTime.Now.Date.AddDays(1),
                StartDate = DateTime.Now.Date
            });
            dbContext.Tasks.Add(new Entities.Task
            {
                //TaskId = 6,
                TaskName = "Child Task # 4",
                CreateTime = DateTime.Now,
                Status = Entities.Statuses.InProgress,
                ParentTaskId = 2,
                Priority = 5,
                EndDate = DateTime.Now.Date.AddDays(5),
                StartDate = DateTime.Now.Date
            });
            dbContext.Tasks.Add(new Entities.Task
            {
                // TaskId = 7,
                TaskName = "Child Task # 5",
                CreateTime = DateTime.Now,
                Status = Entities.Statuses.Completed,
                ParentTaskId = 1,
                Priority = 15,
                EndDate = DateTime.Now.Date.AddDays(100),
                StartDate = DateTime.Now.Date.AddDays(17)
            });

            dbContext.SaveChanges();
        }
    }
}
