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
        
        //_____ GetAllEmploye  + Employee Assosiative to Company ________
        public List<Employee> GetAllEmployeeWithCompany(int id)
        {
            string SqlQuery = @"select e.*,c.* from tblEmployee e inner join tblCompany c on c.id = e.companyId";
            if (id != 0 && id > 0)
            {
                SqlQuery += " WHERE e.CompanyId = @Id";
            }
            var emp = _db.Query<Employee, Company, Employee>(SqlQuery, (emp, comp) =>
            {
                emp.Company = comp;
                return emp;
                //},splitOn:"CompanyId");
            }, new {id}, splitOn:"CompanyId");
            return emp.ToList();
        }

        //_____ GetCompany + With All Employee  ________
        public Company GetCompanyWithAllEmployee(int id)
        {
            string SqlQuery = @"select c.* from tblCompany c where c.id = @id"
                + " select e.* from tblEmployee e where e.CompanyId = @id";

            Company c = new Company();
            using (var lists = _db.QueryMultiple(SqlQuery, new { id}))
            {
                c = lists.Read<Company>().ToList().FirstOrDefault();
                c.EmpList = lists.Read<Employee>().ToList();
            }
            return c;
        }

        //_____ GetCompany + With All Employees with Distinct ________
        public List<Company> GetCompanyWithEmployeeWithDistinct()
        {
            string SqlQuery = @"select c.* , e.* from tblCompany c inner join tblEmployee e on e.CompanyId = c.Id";

            var companyDic = new Dictionary<int, Company>();
            //__ dapper ____
            var company = _db.Query<Company, Employee, Company>(SqlQuery, (c, e) => {
                if (!companyDic.TryGetValue(c.Id, out var currentCompany))
                {
                    currentCompany = c;
                    companyDic.Add(currentCompany.Id, currentCompany);
                }
                //if (!companyDic.TryGetValue(c.Id, out var currentCompany))
                //{
                //    currentCompany = c;
                //    currentCompany.EmpList = new List<Employee>(); // Initialize EmpList
                //    companyDic.Add(currentCompany.Id, currentCompany);
                //}

                //if (currentCompany.EmpList == null)
                //{
                //    currentCompany.EmpList = new List<Employee>(); // Ensure EmpList is not null
                //}

                currentCompany.EmpList.Add(e);
                return currentCompany;
            },splitOn: "Id");
            return company.Distinct().ToList();
        }


    }
}
