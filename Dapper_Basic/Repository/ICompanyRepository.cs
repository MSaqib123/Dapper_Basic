using Dapper_Basic.Models;

namespace Dapper_Basic.Repository
{
    public interface ICompanyRepository
    {
        Company Add(Company obj);
        Company Find(int id);
        List<Company> GetAll();
        void Remove(int Id);
        Company Update(Company obj);
        
    }
}
