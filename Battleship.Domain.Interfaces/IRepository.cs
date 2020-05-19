using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Create(TEntity item);

        void CreateRange(IEnumerable<TEntity> entities);

        TEntity FindById(int id);

        TEntity FirstOrDefault(Func<TEntity, bool> predicate);

        IEnumerable<TEntity> Get();

        IQueryable<TEntity> GetQueryable();

        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);

        IQueryable<TEntity> GetQueryable(Func<TEntity, bool> predicate);

        void Remove(TEntity item);

        void Update(TEntity item);

    }
}