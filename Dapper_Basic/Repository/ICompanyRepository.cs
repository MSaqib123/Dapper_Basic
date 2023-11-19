using Dapper_Basic.Models;

namespace Dapper_Basic.Repository
{
    public interface ICompanyRepository
    {
        Task<Company> Add(Company obj);
        Task<Company> Find(int id);
        Task<List<Company>> GetAll();
        void Remove(int Id);
        Task<Company> Update(Company obj);
        
    }
}
