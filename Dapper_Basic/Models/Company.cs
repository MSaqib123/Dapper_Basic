using System.ComponentModel.DataAnnotations;

namespace Dapper_Basic.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public int Name { get; set; }
        public int Address { get; set; }
        public int City { get; set; }
        public int State { get; set; }
        public int PostalCode { get; set; }
    }
}
