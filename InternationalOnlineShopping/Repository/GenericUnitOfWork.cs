using InternationalOnlineShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternationalOnlineShopping.Repository
{
    
    public class GenericUnitOfWork : IDisposable
    {
        private OnlineShoppingEntities DBEntity = new OnlineShoppingEntities();

        public IRepository<T> GetRepositoryInstance<T>() where T : class
        {
            return new GenericRepository<T>(DBEntity);
        }

        public void SaveChanges()
        {
            DBEntity.SaveChanges();
        }


        #region Disposing the Unit of work context ...
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DBEntity.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}