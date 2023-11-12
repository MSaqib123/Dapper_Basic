﻿using Dapper;
using Dapper_Basic.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Dapper_Basic.Repository
{
    public class EmployeeRepository: IEmployeeRepository
    {
        //________ Dapper DB Connection __________
        //When we work on Dapper  
        //We  always use   ConnectionString
        //SqlConnection is  ---> Dapper object/Class
        private IDbConnection _db;
        public EmployeeRepository(IConfiguration config)
        {
            //geting connectionstring from    appsetting useing configuraton
            _db = new SqlConnection(config.GetConnectionString("Default"));
        }

        //____________ 1. Dapper Query Approch ______________
        #region Query_Approch  (like SQL)
        //1. db.Query<T>(query,obj);      T  is Return Object  Generic (int, class ,......)
        //2. db.Execute()   used when you don't want to return
        public Employee Add(Employee obj)
        {
            var sqlQuery = @"insert into tblEmployee(Name,Email,Phone,Title,CompanyId) values (@name,@email,@phone,@title,@companyIdFk)" +
                    @"Select Cast(Scope_Identity() as int)";
            var id = _db.Query<int>(sqlQuery, new
            {
                name = obj.Name,
                email = obj.Email,
                phone = obj.Phone,
                title = obj.Title,
                companyIdFk = obj.CompanyId
            });
            obj.Id = id.FirstOrDefault();
            return obj;
        }
        public Employee Find(int id)
        {
            string sqlQuery = @"SELECT * from tblEmployee where Id = @id";
            var s = _db.Query<Employee>(sqlQuery,new {Id = id}).FirstOrDefault();
            return s;
        }
        public List<Employee> GetAll()
        {
            string sqlQuery = @"SELECT * from tblEmployee";
            var s = _db.Query<Employee>(sqlQuery).ToList();
            return s;
        }
        public void Remove(int Id)
        {
            var sqlQuery = @"delete tblEmployee where Id = @Id";
            _db.Execute(sqlQuery, new { Id = Id});
        }
        public Employee Update(Employee obj)
        {
            var sqlQuery = @"update tblEmployee set Name = @name,Email = @email,Phone= @phone,Title=@title, CompanyId= @companyId where Id = @id";
            var id = _db.Execute(sqlQuery, obj);
            return obj;
        }
        #endregion

        //____________ 2. Dapper Proccedure Approch _________

    }
}
