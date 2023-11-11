using Dapper_Basic.Models;

namespace Dapper_Basic.Repository
{
    public interface ICompanyRepostiory
    {
        Company Add(Company obj);
        Company Find(int id);
        List<Company> GetAll();
        void Remove();

    }
}
