using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;


namespace Core.DataService
{
    public class EntityContextDataService<TEntity> : IEntityDataService<TEntity> where TEntity : class, new()
    {
        protected readonly DbContext DbContext;

        public EntityContextDataService(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual TEntity GetById<TId>(TId id)
        {
            return DbContext.Set<TEntity>().Find(id);
        }

        public virtual TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> criteria)
        {
            return DbContext.Set<TEntity>().FirstOrDefault(criteria);
        }

        public virtual IList<TEntity> GetMultiple(Expression<Func<TEntity, bool>> criteria)
        {
            return DbContext.Set<TEntity>().Where(criteria).ToList();
        }

        public virtual IList<TEntity> GetAll()
        {
            return DbContext.Set<TEntity>().ToList();
        }

        public virtual TEntity Add(TEntity entity)
        {

            //===== Need to Look Here ======
            var obj = DbContext.Set<TEntity>().Add(entity);

            DbContext.SaveChanges();

            return obj;
        }

        public virtual TEntity Update(TEntity entity)
        {
            //===== Need to Look Here ======
            var obj = DbContext.Set<TEntity>().Attach(entity);

            DbContext.SaveChanges();

            return obj;
        }

        public virtual void Delete(TEntity entity)
        {

            //===== Need to Look Here ======

            DbContext.Set<TEntity>().Remove(entity);

            DbContext.SaveChanges();
        }


    }
}