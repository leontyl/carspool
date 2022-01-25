using Newtonsoft.Json;

namespace CarsPool.Api.Models
{
    public class DriverRequestModel
    {
        [JsonRequired]
        public string FirstName { get; set; }
        [JsonRequired]
        public string SecondName { get; set; }
        public int? CarId { get; set; }
    }
}
