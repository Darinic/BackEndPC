using Thoughts.Core.Requests.Auth;
using Thoughts.Core.Responses.Auth;

namespace Thoughts.Core.Interfaces
{
	public interface IUserService
    {
        Task Register(RegisterRequest request);

        Task<LoginResponse> Login(LoginRequest request);

		Task ConfirmEmail(ConfirmEmailRequest request);

		Task ForgotPassword(ForgotPasswordRequest request);

		Task ResetPassword(ResetPasswordRequest request);
	}
}
