using InternationalOnlineShopping.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace InternationalOnlineShopping.Repository
{
    // This is used to Isolate the EntityFramework based Data Access Layer from the MVC Controller class
   
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        DbSet<T> _dbSet;
        private OnlineShoppingEntities dbEntity;

        public GenericRepository(OnlineShoppingEntities DBEntity)
        {
            dbEntity = DBEntity;
            _dbSet = dbEntity.Set<T>();

        }

        public IEnumerable<T> GetAllRecords()
        {
            return _dbSet.ToList();
        }

        public IQueryable<T> GetAllRecordsIQueryable()
        {
            return _dbSet;
        }

        public IEnumerable<T> GetRecordsToShow(int pageNo, int pageSize, int currentPageNo, Expression<Func<T, bool>> wherePredict, Expression<Func<T, int>> orderByPredict)
        {        
            if (wherePredict != null)
                return _dbSet.OrderBy(orderByPredict).Where(wherePredict).ToList();
            else
                return _dbSet.OrderBy(orderByPredict).ToList();
        }

        public int GetAllRecordsCount()
        {
            return _dbSet.Count();
        }

        public void Add(T entity)
        {
            
            _dbSet.Add(entity);
            dbEntity.SaveChanges();
        }

        /// <summary>
        /// Updates table entity passed to it
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            dbEntity.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateByWhereClause(Expression<Func<T, bool>> wherePredict, Action<T> forEachPredict)
        {
            _dbSet.Where(wherePredict).ToList().ForEach(forEachPredict);
        }

        public T GetFirstOrDefault(int recordId)
        {
            return _dbSet.Find(recordId);
        }

        public T GetFirstOrDefaultByParameter(Expression<Func<T, bool>> wherePredict)
        {
            return _dbSet.Where(wherePredict).FirstOrDefault();
        }

        public IEnumerable<T> GetListByParameter(Expression<Func<T, bool>> wherePredict)
        {
            return _dbSet.Where(wherePredict).ToList();
        }

        public void Remove(T entity)
        {
            if (dbEntity.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);
            _dbSet.Remove(entity);
        }

        public void RemoveByWhereClause(Expression<Func<T, bool>> wherePredict)
        {
            T entity = _dbSet.Where(wherePredict).FirstOrDefault();
            Remove(entity);
        }

        public void RemoveRangeByWhereClause(Expression<Func<T, bool>> wherePredict)
        {
            List<T> entity = _dbSet.Where(wherePredict).ToList();
            foreach (var ent in entity)
            {
                Remove(ent);
            }
        }
        public void DeleteMarkByWhereClause(Expression<Func<T, bool>> wherePredict, Action<T> ForEachPredict)
        {
            _dbSet.Where(wherePredict).ToList().ForEach(ForEachPredict);
            dbEntity.SaveChanges();
        }

        public void InactiveAndDeleteMarkByWhereClause(Expression<Func<T, bool>> wherePredict, Action<T> ForEachPredict)
        {
            _dbSet.Where(wherePredict).ToList().ForEach(ForEachPredict);
            dbEntity.SaveChanges();
        }

        /// <summary>
        /// Returns result by where clause in descending order
        /// </summary>
        /// <param name="orderByPredict"></param>
        /// <returns></returns>
        public IQueryable<T> OrderByDescending(Expression<Func<T, int>> orderByPredict)
        {
            if (orderByPredict == null)
            {
                return _dbSet;
            }
            return _dbSet.OrderByDescending(orderByPredict);
        }

        /// <summary>
        /// Executes procedure in database and returns result
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<T> GetResultBySqlProcedure(string query, params object[] parameters)
        {
            if (parameters != null)
                return dbEntity.Database.SqlQuery<T>(query, parameters).ToList();
            else
                return dbEntity.Database.SqlQuery<T>(query).ToList();
        }
    }
}