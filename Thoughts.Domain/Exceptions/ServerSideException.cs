
namespace Thoughts.Domain.Exceptions
{
	public class ServerSideException : Exception
	{
			public ServerSideException(string message)
				: base(message)
			{
			}
	}
}
