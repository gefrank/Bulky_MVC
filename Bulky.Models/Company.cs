using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models
{
    public class Company
    {
        // Core already knows it's a PK becauuse it has or contains Id
        [Key]
        public int Id { get; set; }
        [Required]       
        [DisplayName("Company Name")]
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }

    }
}
