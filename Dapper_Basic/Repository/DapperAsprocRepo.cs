using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;

namespace Dapper_Basic.Repository
{
    public class DapperAsprocRepo : IDapperAsprocRepo
    {
        private IConfiguration _configuration { get; set; }
        public string ConnectionString { get; set; }
        public DapperAsprocRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("Default");
        }

        #region Execute
        public void Execute(string name)
        {
            Execute(name, null);
        }
        public void Execute(string name, object param)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Execute(name, param, commandType: CommandType.StoredProcedure);
            }
        }
        #endregion

        #region SingleObject _ Generic Object
        public T Single<T>(string name, int id)
        {
            return Single<T>(name, new { id });
        }

        public T Single<T>(string name, object param)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                var result = con.Query<T>(name, param, commandType: CommandType.StoredProcedure);
                if (result != null)
                    return result.FirstOrDefault();
            }
            return default(T);
        }
        #endregion

        #region Multi _ Generic Object ,with Param
        public List<T> List<T>(string name, int id)
        {
            return List<T>(name, new { id });
        }

        public List<T> List<T>(string name, object param)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                var result = con.Query<T>(name, param, commandType: CommandType.StoredProcedure);
                if (result != null)
                    return result.ToList();
            }
            return new List<T>();
        }
        #endregion

        #region 2 IEnumerable List with Param
        public Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string name, object param)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                var result = con.QueryMultiple(name, param, commandType: CommandType.StoredProcedure);

                var item1 = result.Read<T1>().ToList();
                var item2 = result.Read<T2>().ToList();

                if (item1 != null && item2 != null)
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);
            }
            return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(), new List<T2>());
        }
        #endregion

        #region 3 IEnumerable List with Param
        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> List<T1, T2, T3>(string name, object param)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                var result = con.QueryMultiple(name, param, commandType: CommandType.StoredProcedure);

                var item1 = result.Read<T1>().ToList();
                var item2 = result.Read<T2>().ToList();
                var item3 = result.Read<T3>().ToList();

                if (item1 != null && item2 != null)
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(item1, item2, item3);
            }
            return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(new List<T1>(), new List<T2>(), new List<T3>());
        }
        #endregion

        #region List withOut param (getAll List)
        public List<T> List<T>(string name)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                var result = con.Query<T>(name, commandType: CommandType.StoredProcedure);
                if (result != null)
                    return result.ToList();
            }
            return new List<T>();
        }
        #endregion

        #region QueryExecute with Param , without Param
        public void QueryExecute(string name, object param)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Execute(name, param, commandType: CommandType.StoredProcedure);
            }
        }
        public void QueryExecute(string name)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Execute(name, commandType: CommandType.StoredProcedure);
            }
        }
        #endregion
    }
}
