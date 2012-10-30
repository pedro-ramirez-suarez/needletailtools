using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Needletail.DataAccess.Engines;
using Needletail.DataAccess.Entities;

namespace Needletail.DataAccess.DataSources
{
    public class XMLDataSource<E, K> : IDataSource<E, K> where E : class {



        public bool Delete(object where, FilterType filterType) {
            throw new NotImplementedException();
        }

        public bool Delete(object where)
        {
            throw new NotImplementedException();
        }
        
        public bool DeleteEntity(E item)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<E> GetAll() {
            throw new NotImplementedException();
        }

        public IEnumerable<E> GetAll(object orderBy) {
            throw new NotImplementedException();
        }

        public IEnumerable<E> GetAll(string orderBy, System.Data.Common.DbCommand cmd = null) {
            throw new NotImplementedException();
        }

        public IEnumerable<E> GetMany(string select, string where, string orderBy) {
            throw new NotImplementedException();
        }
        public IEnumerable<E> GetMany(object where, FilterType filterType, object orderBy, int? topN) {
            throw new NotImplementedException();
        }

        public IEnumerable<E> GetMany(object where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<E> GetMany(object where, object orderBy)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<E> GetMany(object where, object orderBy, FilterType filterType, int page, int pageSize) {
            throw new NotImplementedException();
        }

        public IEnumerable<E> GetMany(string where, string orderBy, Dictionary<string, object> args, int page, int pageSize) {
            throw new NotImplementedException();
        }

        public IEnumerable<E> GetMany(string where, string orderBy, Dictionary<string, object> args, int? topN) {
            throw new NotImplementedException();
        }

        public IEnumerable<DynamicEntity> Join(string selectColumns, string joinQuery, string whereQuery, string orderBy, Dictionary<string, object> args)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> JoinGetTyped<T>(string selectColumns, string joinQuery, string whereQuery, string orderBy, Dictionary<string, object> args)
        {
            throw new NotImplementedException();
        }

        public E GetSingle(object filter, FilterType filterType) {
            throw new NotImplementedException();
        }

        public E GetSingle(string where, Dictionary<string, object> args) {
            throw new NotImplementedException();
        }

        public K Insert(E newItem) {
            throw new NotImplementedException();
        }

        public bool Update(object item) {
            throw new NotImplementedException();
        }

        public bool UpdateWithWhere(object values, object where, FilterType filterType) {
            throw new NotImplementedException();
        }

        public bool UpdateWithWhere(object values, object where)
        {
            throw new NotImplementedException();
        }

        public void ExecuteNonQuery(string query, Dictionary<string, object> args)
        {
            throw new NotImplementedException();
        }

        public T ExecuteScalar<T>(string query, Dictionary<string, object> args)
        {
            throw new NotImplementedException();
        }
    }
}
