using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManager.Contracts;
using TaskManager.Entities;

namespace TaskManager.DataLayer
{
    public class ProjectRepository : IDataRepository<Project>
    {
        readonly ProjectTaskManagerContext _context;

        public ProjectRepository(ProjectTaskManagerContext context)
        {
            _context = context;
        }

        public void Add(Project entity)
        {
            _context.Projects.Add(entity);
            _context.SaveChanges();
        }

        public bool ChildTaskExits(long taskId)
        {
            throw new NotImplementedException();
        }

        public void Delete(Project entity)
        {
            _context.Projects.Remove(entity);
            _context.SaveChanges();
        }

        public Project Get(long id)
        {
            return _context.Projects.AsNoTracking()
                .Include(k =>k.User)
                  .Where(e => e.ProjectId == id).FirstOrDefault();
        }

        public IEnumerable<Project> GetAll()
        {
            return _context.Projects
                .Include(p => p.User)
                .Include(q => q.Tasks)
                .AsNoTracking().ToList();
        }

        public IEnumerable<Project> GetParentTasks()
        {
            throw new NotImplementedException();
        }

        public void Update(Project entity)
        {
            _context.Projects.Update(entity);
            _context.SaveChanges();
        }
    }
}
