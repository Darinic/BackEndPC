using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thoughts.Core.Requests.Auth
{
    public class RegisterRequest
    {
		[Required]
		[StringLength(30, MinimumLength = 3)]
        public string? UserName { get; set; }

        [Required]
		[EmailAddress]
		[StringLength(50)]
		public string? Email { get; set; }

		[Required]
		[StringLength(30, MinimumLength = 7)]
		public string? Password { get; set; }

		//[Required]
		//public Byte[]? ProfilePictureData { get; set; }
	}
}
