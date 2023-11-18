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