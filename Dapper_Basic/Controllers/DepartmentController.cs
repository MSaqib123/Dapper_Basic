using Dapper_Basic.Models;
using Dapper_Basic.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Dapper_Basic.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ICompanyRepository _repo;
        private readonly IBonusRepository _bonRepo;
        private readonly IDapperAsprocRepo _ProcRepo;

        public DepartmentController(
            ICompanyRepository repo,
            IBonusRepository bonRepo,
            IDapperAsprocRepo ProcRepo
            )
        {
            _repo = repo;
            _bonRepo = bonRepo;
            _ProcRepo = ProcRepo;
        }

        public async Task<IActionResult> Index()
        {
            var list = _ProcRepo.List<Department>("spGetDepartemnts");
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            Company obj = new Company();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Company obj)
        {
            if (ModelState.IsValid)
            {
                await _repo.Add(obj);
                TempData["Success"] = "Inserted Successfuly";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var obj = await _repo.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Company obj)
        {
            if (ModelState.IsValid)
            {
                await _repo.Update(obj);
                TempData["Success"] = "Updated Successfuly";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var obj = await _repo.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _repo.Remove(id);
            return RedirectToAction("Index");
        }
    }
}
