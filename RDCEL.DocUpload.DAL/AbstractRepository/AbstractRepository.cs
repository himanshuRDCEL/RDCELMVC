using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DAL.AbstractRepository
{
    /// <summary>
    /// Implementation of Repository Pattern for Entity Framework
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract class AbstractRepository<TEntity> : IAbstractRepository<TEntity> where TEntity : class
    {
        DbContext _context;
        public DbContext GetCurrentContext()
        {
            return new Digi2l_DBEntities();
        }
        public DbContext Context
        {
            get
            {
                if (this._context == null)
                    this._context = GetCurrentContext();
                try
                {
                    ((IObjectContextAdapter)this._context).ObjectContext.CommandTimeout = 600;
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    throw;
                }
                return this._context;
            }

        }

        public IQueryable<TEntity> GetQuery()
        {
            return Context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            IQueryable<TEntity> dbQuery = Context.Set<TEntity>();

            //Apply eager loading
            dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include(navigationProperty));

            IEnumerable<TEntity> list = dbQuery
                .AsNoTracking();

            return list;
        }

        public virtual IEnumerable<TEntity> GetList(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            IQueryable<TEntity> dbQuery = Context.Set<TEntity>();

            //Apply eager loading
            dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include(navigationProperty));

            IQueryable<TEntity> d = dbQuery.AsNoTracking().Where(where).AsQueryable();

            IEnumerable<TEntity> list = dbQuery
                .AsNoTracking()
                .Where(where)
                .ToList();

            return list;
        }

        public virtual TEntity GetSingle(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            IQueryable<TEntity> dbQuery = Context.Set<TEntity>();

            //Apply eager loading
            foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                dbQuery = dbQuery.Include<TEntity, object>(navigationProperty);

            TEntity item = dbQuery
                .AsNoTracking() //Don't track any changes for the selected item
                .FirstOrDefault(@where);

            return item;
        }

        public virtual TEntity GetSingleTrack(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            IQueryable<TEntity> dbQuery = Context.Set<TEntity>();

            //Apply eager loading
            foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                dbQuery = dbQuery.Include<TEntity, object>(navigationProperty);

            TEntity item = dbQuery
                //.AsNoTracking() //Don't track any changes for the selected item
                .FirstOrDefault(@where);

            return item;
        }


        public virtual void Add(params TEntity[] items)
        {
            try
            {
                foreach (TEntity item in items)
                {
                    Context.Entry(item).State = EntityState.Added;
                }
            }
            catch (Exception ex)
            {

                throw ex;

            }
        }

        public virtual void Update(params TEntity[] items)
        {
            foreach (TEntity item in items)
            {
                Context.Entry(item).State = EntityState.Modified;
            }
        }

        public virtual void Remove(params TEntity[] items)
        {
            foreach (TEntity item in items)
            {
                Context.Entry(item).State = EntityState.Deleted;
            }
        }

        public int SaveChanges()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {


                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                throw;
            }
        }

        public void Attach(TEntity entity)
        {
            Context.Set<TEntity>().Attach(entity);
        }

        public void Entry(TEntity entity, EntityState entityState)
        {
            var entry = Context.Entry(entity);

            switch (entityState)
            {
                case EntityState.Added:
                    entry.State = EntityState.Added;
                    break;
                case EntityState.Modified:
                    entry.State = EntityState.Modified;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Deleted;
                    break;
            }
        }
        public DbEntityEntry Entry(TEntity entity)
        {
            return Context.Entry(entity);
        }

        public Hashtable CheckValidation(TEntity entity)
        {
            var validationresult = Context.Entry(entity).GetValidationResult();
            Hashtable hstErrors = new Hashtable();
            if (validationresult != null && validationresult.ValidationErrors.Count > 0)
            {
                foreach (var eror in validationresult.ValidationErrors)
                    hstErrors.Add(eror.PropertyName, eror.ErrorMessage);
            }
            return hstErrors;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._context != null)
                {
                    this._context.Dispose();
                    this._context = null;
                }
            }
        }
    }
}
