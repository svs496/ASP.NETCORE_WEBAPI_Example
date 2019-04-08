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
            return _context.Tasks.ToList();
        }

        public Task Get(long id)
        {
            return _context.Tasks
                  .FirstOrDefault(e => e.TaskId == id);
        }

        public void Add(Task entity)
        {
            _context.Tasks.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Task task)
        {
            _context.Tasks.Update(task);
            _context.SaveChanges();
        }

        public void Delete(Task task)
        {
            _context.Tasks.Remove(task);
            _context.SaveChanges();
        }

    }
}
