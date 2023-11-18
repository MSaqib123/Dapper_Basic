using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Dapper_Basic.Models
{
    [Table("tblCompany")]
    public class Company
    {
        public Company()
        {
            EmpList = new List<Employee>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        [Write(false)]
        [ValidateNever]
        public List<Employee> EmpList { get; set; }
    }
}
