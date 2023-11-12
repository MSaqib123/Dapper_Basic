using Dapper_Basic.Models;

namespace Dapper_Basic.Repository
{
    public interface IEmployeeRepository
    {
        Employee Add(Employee obj);
        Employee Find(int id);
        List<Employee> GetAll();
        void Remove(int Id);
        Employee Update(Employee obj);   
    }
}
