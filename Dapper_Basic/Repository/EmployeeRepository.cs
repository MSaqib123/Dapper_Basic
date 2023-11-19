using Dapper;
using Dapper.Contrib.Extensions;
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
        //public Employee Add(Employee obj)
        //{
        //    var sqlQuery = @"insert into tblEmployee(Name,Email,Phone,Title,CompanyId) values (@name,@email,@phone,@title,@companyIdFk)" +
        //            @"Select Cast(Scope_Identity() as int)";
        //    var id = _db.Query<int>(sqlQuery, new
        //    {
        //        name = obj.Name,
        //        email = obj.Email,
        //        phone = obj.Phone,
        //        title = obj.Title,
        //        companyIdFk = obj.CompanyId
        //    });
        //    obj.Id = id.FirstOrDefault();
        //    return obj;
        //}
        //public Employee Find(int id)
        //{
        //    string sqlQuery = @"SELECT * from tblEmployee where Id = @id";
        //    var s = _db.Query<Employee>(sqlQuery,new {Id = id}).FirstOrDefault();
        //    return s;
        //}
        //public List<Employee> GetAll()
        //{
        //    string sqlQuery = @"SELECT * from tblEmployee";
        //    var s = _db.Query<Employee>(sqlQuery).ToList();
        //    return s;
        //}
        //public void Remove(int Id)
        //{
        //    var sqlQuery = @"delete tblEmployee where Id = @Id";
        //    _db.Execute(sqlQuery, new { Id = Id});
        //}
        //public Employee Update(Employee obj)
        //{
        //    var sqlQuery = @"update tblEmployee set Name = @name,Email = @email,Phone= @phone,Title=@title, CompanyId= @companyId where Id = @id";
        //    var id = _db.Execute(sqlQuery, obj);
        //    return obj;
        //}
        #endregion

        //____________ 2. Dapper Proccedure Approch _________
        #region ProcceduralBase_
        ////1commandType  :    storeProcedure , text
        //public Employee Add(Employee obj)
        //{
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@id", 0, DbType.Int32, direction: ParameterDirection.Output);
        //    parameters.Add("@name", obj.Name);
        //    parameters.Add("@title", obj.Title);
        //    parameters.Add("@email", obj.Email);
        //    parameters.Add("@phone", obj.Phone);
        //    parameters.Add("@CompanyId", obj.CompanyId);

        //    _db.Execute("spInsertEmployee", parameters, commandType: CommandType.StoredProcedure);
        //    obj.Id = parameters.Get<int>("id");
        //    return obj;
        //}
        //public Employee Find(int id)
        //{
        //    string sqlQuery = @"spSelectEmployeeById";
        //    var s = _db.Query<Employee>(sqlQuery, new { Id = id } , commandType:CommandType.StoredProcedure).FirstOrDefault();
        //    return s;
        //}
        //public List<Employee> GetAll()
        //{
        //    string sqlQuery = @"spSelectEmployees";
        //    var s = _db.Query<Employee>(sqlQuery,commandType:CommandType.StoredProcedure).ToList();
        //    return s;
        //}
        //public void Remove(int Id)
        //{
        //    var sqlQuery = @"spDeleteEmployeeById";
        //    _db.Execute(sqlQuery, new { id = Id });
        //}

        //public Employee Update(Employee obj)
        //{
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@id", 0, DbType.Int32);
        //    parameters.Add("@name", obj.Name);
        //    parameters.Add("@title", obj.Title);
        //    parameters.Add("@email", obj.Email);
        //    parameters.Add("@phone", obj.Phone);
        //    parameters.Add("@CompanyId", obj.CompanyId);
        //    _db.Execute("spUpdateEmployee", parameters, commandType: CommandType.StoredProcedure);
        //    return obj;
        //}
        #endregion

        //____________ 3. Dapper Contrib Approch _________
        #region Dapper_Contrib
        //____ Note  ____
        //1. for Contrib  method  ---> Model must have  [Key]  dapper.attibute in Model
        //2. for Contrib  ---> add   Table()   attibute in Module with SQL table Name
        //public Employee Add(Employee obj)
        //{
        //    var id = _db.Insert(obj);
        //    obj.Id =(int)id;
        //    return obj;
        //}
        //public Employee Find(int id)
        //{
        //    return _db.Get<Employee>(id);
        //}
        //public List<Employee> GetAll()
        //{
        //    return _db.GetAll<Employee>().ToList();
        //}
        //public void Remove(int Id)
        //{
        //    _db.Delete(new Company { Id = Id });
        //}
        //public Employee Update(Employee obj)
        //{
        //    _db.Update(obj);
        //    return obj;
        //}
        #endregion


        //____________ 4. Dapper Async _________
        #region Assync _ Dapper
        public async Task<Employee> Add(Employee obj)
        {
            var id = await _db.InsertAsync(obj);
            obj.Id = (int)id;
            return obj;
        }
        public async Task<Employee> Find(int id)
        {
            return await _db.GetAsync<Employee>(id);
        }
        public async Task<List<Employee>> GetAll()
        {
            var list = await _db.GetAllAsync<Employee>();
            return list.ToList();
        }
        public async void Remove(int Id)
        {
            await _db.DeleteAsync(new Company { Id = Id });
        }
        public async Task<Employee> Update(Employee obj)
        {
            await _db.UpdateAsync(obj);
            return obj;
        }
        #endregion
    }
}
