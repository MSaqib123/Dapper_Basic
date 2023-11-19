using Dapper.Contrib.Extensions;

namespace Dapper_Basic.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
