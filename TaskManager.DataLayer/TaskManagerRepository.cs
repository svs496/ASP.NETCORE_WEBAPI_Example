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
        readonly TaskManagerContext _context;

        public TaskManagerRepository(TaskManagerContext context)
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
                         }).Distinct().ToList();


            return _context.Tasks
                .Select (m => new Task { TaskId = m.TaskId,
                                         ParentTaskId = m.ParentTaskId,
                                         EndDate = m.EndDate.Date,
                                         TaskName = m.TaskName,
                                         Priority = m.Priority,
                                         StartDate = m.StartDate.Date,
                                         Status = m.Status,
                                         CreateTime = m.CreateTime,  
                                         ParentTaskName = query.Where (p => p.TaskId == m.ParentTaskId).Select (p => p.TaskName).FirstOrDefault() })
                .OrderByDescending( s => s.CreateTime).ThenByDescending (z => z.ModifyDate)
                .ToList();
        }

        public Task Get(long id)
        {
            return _context.Tasks
                  .FirstOrDefault(e => e.TaskId == id);
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
                .Distinct().OrderBy(k => k.TaskName).ToList();
        }

        public bool ChildTaskExits(long taskId)
        {
            if (_context.Tasks.Any(p => p.ParentTaskId == taskId))
                return true;

            return false;
        }
    }
}
