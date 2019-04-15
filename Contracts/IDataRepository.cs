using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManager.Contracts
{
    public interface IDataRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(long id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        IEnumerable<TEntity> GetParentTasks();

        bool ChildTaskExits(long taskId);

    }
}
