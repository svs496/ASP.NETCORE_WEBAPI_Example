using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Contracts;
using TaskManager.Entities;


namespace TaskManager.DataLayer
{
    public class UserRepository : IDataRepository<User>
    {
        readonly ProjectTaskManagerContext _context;

        public UserRepository(ProjectTaskManagerContext context)
        {
            _context = context;
        }


        public void Add(User entity)
        {
            _context.Users.Add(entity);
            _context.SaveChanges();
        }

        public bool CanDeleteEntity(long id)
        {
            //user is tagged to task or project than that user cannot be deleted.
            if (_context.Tasks.Any(p => p.UserId == id) || _context.Projects.Any(p => p.UserId == id))
                return true;

            return false;
        }

        public void Delete(User entity)
        {
            _context.Users.Remove(entity);
            _context.SaveChanges();
        }

        public User Get(long id)
        {
            return _context.Users.AsNoTracking()
              .Where(e => e.UserId == id).FirstOrDefault();
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.AsNoTracking()
                .OrderBy(a => a.FirstName).ToList();
        }

        public IEnumerable<User> GetListById(long id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<User> GetParentTasks()
        {
            throw new System.NotImplementedException();
        }

        public void Update(User entity)
        {
            _context.Users.Update(entity);
            _context.SaveChanges();
        }
    }
}
