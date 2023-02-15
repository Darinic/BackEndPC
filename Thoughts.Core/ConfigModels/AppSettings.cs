
namespace Thoughts.Core.ConfigModels
{
	public class AppSettings
	{
			public string? AuthTokenKey { get; set; }

			public string? FrontEndUrl { get; set; }

			public string? BackEndUrl { get; set; }

			public string? EmailServer { get; set; }
		
			public int EmailServerPort { get; set; }

			public string? EmailUsername { get; set; }

			public string? MailJetApiKey { get; set; }

			public string? MailJetSecretKey { get; set; }

			//public int ProfilePictureWidth { get; set; }

			//public int ProfilePictureHeight { get; set; }
	}
}
