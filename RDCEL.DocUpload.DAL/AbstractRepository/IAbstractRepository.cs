using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DAL.AbstractRepository
{
    public interface IAbstractRepository<TEntity> : IDisposable where TEntity : class
    {
        DbContext GetCurrentContext();

        DbContext Context
        {
            get; //get { return _context ?? (_context = GetCurrentUnitOfWork<EFUnitOfWork>().Context); }
        }

        IQueryable<TEntity> GetQuery();

        IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] navigationProperties);

        IEnumerable<TEntity> GetList(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] navigationProperties);

        TEntity GetSingle(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] navigationProperties);

        TEntity GetSingleTrack(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] navigationProperties);


        void Add(params TEntity[] items);

        void Update(params TEntity[] items);

        void Remove(params TEntity[] items);

        int SaveChanges();

        void Attach(TEntity entity);

        void Entry(TEntity entity, EntityState entityState);

        Hashtable CheckValidation(TEntity entity);

        void Dispose(bool disposing);
    }
}
