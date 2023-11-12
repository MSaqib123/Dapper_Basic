using Dapper_Basic.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dapper_Basic.VM
{
    public class EmployeeVM
    {
        public Employee Employee { get; set; }
        [ValidateNever]
        public IEnumerable<Employee> Employees { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> ListCompany { get; set; }

    }
}
