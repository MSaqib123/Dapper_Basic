using Dapper_Basic.Models;
using Dapper_Basic.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace Dapper_Basic.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository _repo;
        private readonly IBonusRepository _bonRepo;

        public CompanyController(
            ICompanyRepository repo,
            IBonusRepository bonRepo
            )
        {
            _repo = repo;
            _bonRepo = bonRepo;
        }

        public IActionResult Index()
        {
            var list = _repo.GetAll();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            Company obj = new Company();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Company obj)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(obj);
                TempData["Success"] = "Inserted Successfuly";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var obj = _repo.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Company obj)
        {
            if (ModelState.IsValid)
            {
                _repo.Update(obj);
                TempData["Success"] = "Updated Successfuly";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Delete(int id)
        {
            var obj = _repo.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _repo.Remove(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            Company obj = new Company();
            obj = _bonRepo.GetCompanyWithAllEmployee(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

    }
}
