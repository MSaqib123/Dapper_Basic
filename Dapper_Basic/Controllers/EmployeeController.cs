﻿using Dapper_Basic.Models;
using Dapper_Basic.Repository;
using Dapper_Basic.VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace Dapper_Basic.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _repo;
        private readonly ICompanyRepository _repoComp;

        public EmployeeController(IEmployeeRepository repo, ICompanyRepository repoComp)
        {
            _repo = repo;
            _repoComp = repoComp;
        }

        public IActionResult Index()
        {
            EmployeeVM vm = new EmployeeVM();

            /*
             _______________ 1... Single Model get ______________
             vm.Employees = _repo.GetAll();
            */


            /*
             _______________ 2... N + 1 __________________________
             this is Bad Logic to get   Multiple Models Record
             ---- if we have 11 Record
             ---- then we ha 10 Calles to database
             ---- what will happend if we have  10000 of records
            */
            #region N+1
            var employees = _repo.GetAll();
            foreach (var obj in employees)
            {
                obj.Company = _repoComp.Find(obj.CompanyId);
            }
            vm.Employees = employees;
            #endregion

            /*
             _______________ 3... N + 1 __________________________
             this is Bad Logic to get   Multiple Models Record
             ---- if we have 11 Record
             ---- then we ha 10 Calles to database
             ---- what will happend if we have  10000 of records
            */ 
            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            EmployeeVM vm = new EmployeeVM();
            IEnumerable<SelectListItem> compList = _repoComp.GetAll().Select(x => new SelectListItem{Value = x.Id.ToString() , Text = x.Name});
            vm.ListCompany = compList;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeVM vm)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(vm.Employee);
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
            EmployeeVM vm = new EmployeeVM();
            vm.Employee = _repo.Find(id);
            IEnumerable<SelectListItem> compList = _repoComp.GetAll().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            vm.ListCompany = compList;
            if (vm.Employee == null)
            {
                return NotFound();
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeVM vm)
        {
            if (ModelState.IsValid)
            {
                _repo.Update(vm.Employee);
                TempData["Success"] = "Updated Successfuly";
                return RedirectToAction("Index");
            }
            else
            {
                IEnumerable<SelectListItem> compList = _repoComp.GetAll().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
                vm.ListCompany = compList;
                return View(vm);
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
    }
}
