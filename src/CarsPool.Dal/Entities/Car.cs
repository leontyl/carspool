using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsPool.Dal.Entities
{
    [Index(nameof(Id))]
    public class Car
    {
        [Key]
        [ForeignKey("CarId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string LicenceNumber { get; set; }
        public string Manufacturer { get; set; }
        public string Color { get; set; }
    }
}
