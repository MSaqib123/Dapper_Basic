using Dapper_Basic.Models;

namespace Dapper_Basic.Repository
{
    public interface IBonusRepository
    {
        List<Employee> GetAllEmployeeWithCompany(int CompanyId);
        Company GetCompanyWithAllEmployee(int id);
        List<Company> GetCompanyWithEmployeeWithDistinct();
        void InsertCompanyWithEmployee(Company obj);
        void RemoveRange(int[] companyId);
        List<Company> FilterCompanyByName(string name);
    }
}
