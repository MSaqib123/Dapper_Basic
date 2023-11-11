using Dapper;
using Dapper_Basic.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Dapper_Basic.Repository
{
    public class CompanyRepository: ICompanyRepostiory
    {
        //________ Dapper DB Connection __________
        //When we work on Dapper  
        //We  always use   ConnectionString
        //SqlConnection is  ---> Dapper object/Class
        private IDbConnection _db;
        public CompanyRepository(IConfiguration config)
        {
            //geting connectionstring from    appsetting useing configuraton
            _db = new SqlConnection(config.GetConnectionString("Default"));
        }


        //____________ 1. Dapper Query Approch ______________
        #region Query_Approch  (like SQL)
        //1. db.Query<T>(query,obj);      T  is Return Object  Generic (int, class ,......)
        //2. db.Execute()   used when you don't want to return
        public Company Add(Company obj)
        {
            var sqlQuery = @"insert into tblCompany(Name,Address,City,State,PostalCode) values (@name,@address,@city,@postalCode)"+
                    @"Select Cast(Scope_Identity() as int)";
            var id = _db.Query<int>(sqlQuery, new
            {
                name = obj.Name,
                city = obj.City,
                address = obj.Address,
                state = obj.State,
                postalCode = obj.PostalCode
            });
            obj.Id = id.FirstOrDefault();
            return obj;
        }
        public Company Find(int id)
        {
            string sqlQuery = @"SELECT * from tblCompany where Id = @id";
            var s = _db.Query<Company>(sqlQuery,new {Id = id}).FirstOrDefault();
            return s;
        }
        public List<Company> GetAll()
        {
            string sqlQuery = @"SELECT * from tblCompany";
            var s = _db.Query<Company>(sqlQuery).ToList();
            return s;
        }
        public void Remove()
        {

            return;
        }
        public Company Update(Company obj)
        {
            var sqlQuery = @"update tblCompany set Name = @name,Address = @address,City = @City, PostalCode = @postalCode where Id = @id";
            var id = _db.Execute(sqlQuery, new {Id = obj.Id});
            return obj;
        }
        #endregion


        //____________ 2. Dapper Proccedure Approch ______________
    }
}
