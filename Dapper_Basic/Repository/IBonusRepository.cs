using Dapper_Basic.Models;

namespace Dapper_Basic.Repository
{
    public interface IBonusRepository
    {
        List<Employee> GetAllEmployeeWithCompany();
    }
}
