using Dapper;
using Dapper_Basic.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Transactions;

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


        //_________ Insert Company With Employee ____________
        //_____ GetCompany + With All Employees with Distinct ________
        public void InsertCompanyWithEmployee(Company obj)
        {
            //____ Insert Company ______
            var sqlQuery = @"insert into tblCompany(Name,Address,City,State,PostalCode) values (@name,@address,@city,@state,@postalCode)" +
                    @"Select Cast(Scope_Identity() as int)";
            var id = _db.Query<int>(sqlQuery, new
            {
                name = obj.Name,
                address = obj.Address,
                city = obj.City,
                state = obj.State,
                postalCode = obj.PostalCode
            });
            obj.Id = id.FirstOrDefault();

            //____ Bulk Insert Employee ______
            //foreach (var item in obj.EmpList)
            //{
            //    var empQuery = @"insert into tblEmployee(Name,Email,Phone,Title,CompanyId) values (@name,@email,@phone,@title,@companyIdFk)" +
            //        @"Select Cast(Scope_Identity() as int)";
            //    var empid = _db.Query<int>(empQuery, new
            //    {
            //        name = item.Name,
            //        email = item.Email,
            //        phone = item.Phone,
            //        title = item.Title,
            //        companyIdFk = obj.Id
            //    });
            //}

            //____ Bulk Insert Employee by  Linq ______
            obj.EmpList.Select(c => { c.CompanyId = obj.Id; return c; }).ToList();
            var empQuery = @"insert into tblEmployee(Name,Email,Phone,Title,CompanyId) values (@name,@email,@phone,@title,@companyId)" +
                @"Select Cast(Scope_Identity() as int)";

            _db.Execute(empQuery,obj.EmpList);
        }

        public void RemoveRange(int[] companyId)
        {
            _db.Query("Delete from tblCompany where id in @id" , new {id = companyId });
        }
        public List<Company> FilterCompanyByName(string name)
        {
            return _db.Query<Company>("SELECT * FROM tblCompany where name like'%' + @name + '%'", new {name}).ToList();
        }

        public void InsertCompanyWithEmployeeWithTransaction(Company obj)
        {
            using (var trans = new TransactionScope())
            {
                try
                {
                    //____ Insert Company ______
                    var sqlQuery = @"insert into tblCompany(Name,Address,City,State,PostalCode) values (@name,@address,@city,@state,@postalCode)" +
                            @"Select Cast(Scope_Identity() as int)";
                    var id = _db.Query<int>(sqlQuery, new
                    {
                        name = obj.Name,
                        address = obj.Address,
                        city = obj.City,
                        state = obj.State,
                        postalCode = obj.PostalCode
                    });
                    obj.Id = id.FirstOrDefault();

                    //____ Bulk Insert Employee by  Linq ______
                    obj.EmpList.Select(c => { c.CompanyId = obj.Id; return c; }).ToList();
                    var empQuery = @"insert into tblEmployee(Name,Email,Phone,Title,CompanyId) values (@name,@email,@phone,@title,@companyId)" +
                        @"Select Cast(Scope_Identity() as int)";

                    _db.Execute(empQuery, obj.EmpList);
                    trans.Complete();
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
