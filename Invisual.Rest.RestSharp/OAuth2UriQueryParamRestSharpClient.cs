using Invisual.Rest.RestSharp.Authenticators;
using Invisual.Serialization;
using System;

namespace Invisual.Rest.RestSharp
{
	/// <summary>
	/// Rest client which uses OAuth2 with the querystring param method of authentication.
	/// </summary>
	public class OAuth2UriQueryParamRestSharpClient : RestSharpClient
	{
		public OAuth2UriQueryParamRestSharpClient(string accessToken, ISerializer serializer, string accessTokenKeyNameOverride = null)
			: base(serializer)
		{
			if (string.IsNullOrWhiteSpace(accessToken))
				throw new ArgumentException("accessToken must not be null, empty, or whitespace");

			Authenticator = new OAuth2QuerystringAuthenticator(accessToken);

			if (accessTokenKeyNameOverride != null)
				((OAuth2QuerystringAuthenticator)Authenticator).QueryStringKey = accessTokenKeyNameOverride;
		}
	}
}
