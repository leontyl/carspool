using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsPool.Dal.Entities
{
    [Index(nameof(Id))]
    public class Driver
    {
        [Key]
        [ForeignKey("DriverId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SecondName { get; set; }
        public int? CarId { get; set; }
    }
}
