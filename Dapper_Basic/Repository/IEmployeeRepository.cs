using Dapper_Basic.Models;

namespace Dapper_Basic.Repository
{
    public interface IEmployeeRepository
    {
        Task<Employee> Add(Employee obj);
        Task<Employee> Find(int id);
        Task<List<Employee>> GetAll();
        void Remove(int Id);
        Task<Employee> Update(Employee obj);   
    }
}
