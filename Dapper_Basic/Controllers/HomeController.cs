using Dapper_Basic.Models;
using Dapper_Basic.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dapper_Basic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmployeeRepository _repo;
        private readonly ICompanyRepository _repoComp;
        private readonly IBonusRepository _bonusRepo;

        public HomeController(
            ILogger<HomeController> logger,
            IEmployeeRepository repo,
            ICompanyRepository repoComp,
            IBonusRepository bonusRepo
            )
        {
            _logger = logger;
            _repo = repo;
            _repoComp = repoComp;
            _bonusRepo = bonusRepo;
        }
        

        public IActionResult Index()
        {
            List<Company> obj = new List<Company>();
            obj = _bonusRepo.GetCompanyWithEmployeeWithDistinct();
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        
        public IActionResult addDummy()
        {
            Company company = new Company()
            {
                Name = "Test -- " + Guid.NewGuid().ToString(),
                Address = "Test address",
                City = "abc",
                PostalCode = "test",
                State = "test",
                EmpList = new List<Employee>()
            };

            List<Employee> emp = new List<Employee>()
            {
                new Employee{Email="Email1234",Name="Test :: " + Guid.NewGuid().ToString(),Phone="234",Title="title"},
                new Employee{Email="Emailss",Name="Test :: " + Guid.NewGuid().ToString(),Phone="234",Title="title"},
                new Employee{Email="Emails",Name="Test :: " + Guid.NewGuid().ToString(),Phone="234",Title="title"},
                new Employee{Email="Emailsd",Name="Test :: " + Guid.NewGuid().ToString(),Phone="234",Title="title"},
            };
            company.EmpList = emp;
            _bonusRepo.InsertCompanyWithEmployee(company);
            return RedirectToAction("Index");
        }

        public IActionResult deleteDummy()
        {
            return View();
        }
        


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}