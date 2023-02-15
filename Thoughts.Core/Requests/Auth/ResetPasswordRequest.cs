using System.ComponentModel.DataAnnotations;

namespace Thoughts.Core.Requests.Auth
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 7)]
        public string Password { get; set; }
    }
}
