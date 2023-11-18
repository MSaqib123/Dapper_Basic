using Dapper;
using Dapper_Basic.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Dapper_Basic.Repository
{
    public class BonusRepository : IBonusRepository
    {
        private IDbConnection _db;
        public BonusRepository(IConfiguration config)
        {
            //geting connectionstring from    appsetting useing configuraton
            _db = new SqlConnection(config.GetConnectionString("Default"));
        }
        
        public List<Employee> GetAllEmployeeWithCompany()
        {
            string SqlQuery = @"select e.*,c.* from tblEmployee e inner join tblCompany c on c.id = e.companyId";
            var emp = _db.Query<Employee, Company, Employee>(SqlQuery, (emp, comp) =>
            {
                emp.Company = comp;
                return emp;
            },splitOn:"CompanyId");
            return emp.ToList();
        }
    }
}
