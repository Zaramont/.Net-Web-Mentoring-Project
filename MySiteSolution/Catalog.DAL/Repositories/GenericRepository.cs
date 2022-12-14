using Catalog.DAL.DataContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.DAL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private CatalogDataContext Context { get; set; }
        public GenericRepository(CatalogDataContext context)
        {
            Context = context;
        }

        protected DbSet<TEntity> DbSet
        {
            get
            {
                if (_dbSet == null)
                    _dbSet = Context.Set<TEntity>();
                return _dbSet;
            }
        }
        private DbSet<TEntity> _dbSet;


        public virtual IEnumerable<TEntity> GetAll()
        {
            return this.DbSet.AsEnumerable();
        }
        public virtual TEntity GetById(int id)
        {
            var entity = this.DbSet.Find(id);
            Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public void Delete(TEntity entity)
        {
            var entityContext = Context.Entry(entity);
            entityContext.State = EntityState.Detached;
            Context.SaveChanges();


            this.DbSet.Remove(entity);
            Context.SaveChanges();
        }

        public void Create(TEntity entity)
        {
            this.DbSet.Add(entity);
            Context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            var entityContext = Context.Entry(entity);
            entityContext.State = EntityState.Modified;
            Context.SaveChanges();

        }
    }
}
