using Battleship.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Battleship.Infrastructure.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext context;
        private readonly DbSet<TEntity> set;

        public Repository(ApplicationDbContext context)
        {
            this.context = context;
            this.set = context.Set<TEntity>();
        }

        public void Create(TEntity item)
        {
            set.Add(item);
            context.SaveChanges();
        }

        public void CreateRange(IEnumerable<TEntity> entities)
        {
             set.AddRange(entities);
            context.SaveChanges();
        }

        public TEntity FindById(int id)
        {
            TEntity entity = set.Find(id);
            return entity;
        }

        public TEntity FirstOrDefault(Func<TEntity, bool> predicate)
        {
            return set.FirstOrDefault(predicate);
        }

        public IEnumerable<TEntity> Get()
        {
            return set.AsNoTracking().ToList();
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return set.AsNoTracking().Where(predicate).ToList();
        }

        public void Remove(TEntity item)
        {
            context.Entry(item).State = EntityState.Deleted;
            context.SaveChanges();
        }

        public void Update(TEntity item)
        {
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }
       
        public IQueryable<TEntity> GetQueryable()
        {
            return set;
        }

        public IQueryable<TEntity> GetQueryable(Func<TEntity, bool> predicate)
        {
            return set.Where(predicate).AsQueryable();
        }
    }
}