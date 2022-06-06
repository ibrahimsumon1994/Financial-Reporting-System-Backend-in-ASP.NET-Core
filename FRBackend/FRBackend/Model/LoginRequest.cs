using System.ComponentModel.DataAnnotations;

namespace FRBackend.Model
{
    public class LoginRequest
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }
    }

    public class ChangePassword
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
    public class Token
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
