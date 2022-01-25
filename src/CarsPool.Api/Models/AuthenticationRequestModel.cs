using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CarsPool.Api.Models
{
    public class AuthenticationRequestModel
    {
        [JsonRequired]
        public string Username { get; set; }

        [JsonRequired]
        public string Password { get; set; }
    }
}
