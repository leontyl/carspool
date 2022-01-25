using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsPool.Dal.Entities
{
    public class CarDriver
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int CarId { get; set; }
        public Car Car { get; set; }
        [Required]
        public int DriverId { get; set; }
        public Driver Driver { get; set; }
    }
}
