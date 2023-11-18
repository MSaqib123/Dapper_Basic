using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Dapper_Basic.Models
{
    [Table("tblEmployee")]
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
        public int CompanyId { get; set; }

        [Write(false)]
        [ValidateNever]
        public virtual Company Company { get; set; }
    }
}
