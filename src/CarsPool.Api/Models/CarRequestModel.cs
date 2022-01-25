using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CarsPool.Api.Models
{
    public class CarRequestModel
    {
        [JsonRequired] // Licence code must be binded to model. If use [Required] field is populated by default value
        public string LicenceNumber { get; set; }
        public string Manufacturer { get; set; }
        public string Color { get; set; }
    }
}
