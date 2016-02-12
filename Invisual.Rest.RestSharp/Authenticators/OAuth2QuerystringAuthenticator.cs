using RestSharp;

namespace Invisual.Rest.RestSharp.Authenticators
{
	/// <summary>
	/// RestSharp OAuth 2 authenticator using URI query parameter.
	/// </summary>
	/// <remarks>
	public class OAuth2QuerystringAuthenticator : OAuth2Authenticator
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OAuth2UriQueryParameterAuthenticator"/> class.
		/// </summary>
		/// <param name="accessToken">
		/// The access token.
		/// </param>
		public OAuth2QuerystringAuthenticator(string accessToken)
			: base(accessToken) { }

		/// <summary>
		/// The name to use for the querystring token paramater.
		/// Default: "oauth_token".
		/// </summary>
		public string QueryStringKey { get; set; }

		public override void Authenticate(global::RestSharp.IRestClient client, IRestRequest request)
		{
			request.AddParameter(QueryStringKey ?? "oauth_token", AccessToken, ParameterType.GetOrPost);
		}
	}
}
