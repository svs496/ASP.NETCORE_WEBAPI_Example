using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManager.Contracts;
using TaskManager.Entities;

namespace TaskManager.DataLayer
{
    public class TaskManagerRepository : IDataRepository<Task>
    {
        readonly ProjectTaskManagerContext _context;

        public TaskManagerRepository(ProjectTaskManagerContext context)
        {
            _context = context;
        }

        public IEnumerable<Task> GetAll()
        {

            var query = (from r in _context.Tasks
                         join q in _context.Tasks on r.ParentTaskId equals q.TaskId
                         select new Task
                         {
                             TaskId = q.TaskId,
                             TaskName = q.TaskName
                         }).Distinct().AsNoTracking().ToList();


            return _context.Tasks
                .Select (m => new Task { TaskId = m.TaskId,
                                         ParentTaskId = m.ParentTaskId,
                                         EndDate = m.EndDate.HasValue  ?  m.EndDate.Value.Date : (DateTime?)null,
                                         TaskName = m.TaskName,
                                         Priority = m.Priority,
                                         StartDate = m.StartDate.HasValue ? m.StartDate.Value.Date : (DateTime?)null,
                                         Status = m.Status,
                                         CreateTime = m.CreateTime,  
                                         UserId = m.UserId,
                                         ProjectId = m.ProjectId,
                                         ParentTaskName = query.Where (p => p.TaskId == m.ParentTaskId).Select (p => p.TaskName).FirstOrDefault() })
                .OrderByDescending( s => s.CreateTime).ThenByDescending (z => z.ModifyDate)
                .AsNoTracking()
                .ToList();
        }

        public Task Get(long id)
        {
            return _context.Tasks.AsNoTracking()
                  .Where(e => e.TaskId == id).FirstOrDefault();
        }


        public IEnumerable<Task> GetListById(long id)
        {
            List<Task> tasks = GetAll().ToList();

            return tasks
                 .Where(e => e.ProjectId == id);
        }


            public void Add(Task entity)
        {
            entity.CreateTime = DateTime.Now;
            _context.Tasks.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Task task)
        {
            task.ModifyDate = DateTime.Now;
            _context.Tasks.Update(task);
            _context.SaveChanges();
        }

        public void Delete(Task task)
        {
            _context.Tasks.Remove(task);
            _context.SaveChanges();
        }

        public IEnumerable<Task> GetParentTasks()
        {
          return  _context.Tasks.Select(p => new Task { TaskId = p.TaskId, TaskName = p.TaskName })
                .Distinct().OrderBy(k => k.TaskName).AsNoTracking().ToList();
        }

        public bool ChildTaskExits(long taskId)
        {
            if (_context.Tasks.Any(p => p.ParentTaskId == taskId))
                return true;

            return false;
        }
    }
}
