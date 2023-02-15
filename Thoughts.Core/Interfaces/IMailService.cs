using Thoughts.Domain.Entities;

namespace Thoughts.Core.Interfaces
{
	public interface IMailService
    {
		Task SendEmailConfirmation(string email, User userEntity);

		Task SendEmailForgotPassword(string email, User user);
	}
}
