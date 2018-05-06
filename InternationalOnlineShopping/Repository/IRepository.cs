using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace InternationalOnlineShopping.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAllRecords();
        IQueryable<T> GetAllRecordsIQueryable();
        IEnumerable<T> GetRecordsToShow(int pageNo, int pageSize, int currentPageNo, Expression<Func<T, bool>> wherePredict, Expression<Func<T, int>> orderByPredict);
        int GetAllRecordsCount();
        void Add(T entity);
        void Update(T entity);
        void UpdateByWhereClause(Expression<Func<T, bool>> wherePredict, Action<T> ForEachPredict);
        T GetFirstOrDefault(int recordId);
        void Remove(T entity);
        void RemoveByWhereClause(Expression<Func<T, bool>> wherePredict);
        void RemoveRangeByWhereClause(Expression<Func<T, bool>> wherePredict);
        void InactiveAndDeleteMarkByWhereClause(Expression<Func<T, bool>> wherePredict, Action<T> ForEachPredict);
        T GetFirstOrDefaultByParameter(Expression<Func<T, bool>> wherePredict);
        IEnumerable<T> GetListByParameter(Expression<Func<T, bool>> wherePredict);
        IEnumerable<T> GetResultBySqlProcedure(string query, params object[] parameters);
    }
}