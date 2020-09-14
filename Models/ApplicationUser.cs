using Microsoft.AspNetCore.Identity;

namespace UserApi.Models
{
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public class ApplicationUser
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [Required]
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [Required]
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}