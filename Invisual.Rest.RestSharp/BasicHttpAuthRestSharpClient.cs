using RestSharp;
using Invisual.Serialization;
using System;

namespace Invisual.Rest.RestSharp
{
	public class BasicHttpAuthRestSharpClient : RestSharpClient
	{
		public BasicHttpAuthRestSharpClient(string username, string password, ISerializer serializer)
			: base (serializer)
		{
			if (string.IsNullOrWhiteSpace(username))
				throw new ArgumentException("username must not be null, emtpy, or whitespace");
			if (string.IsNullOrWhiteSpace(password))
				throw new ArgumentException("password must not be null, emtpy, or whitespace");

			Authenticator = new HttpBasicAuthenticator(username, password);
		}
	}
}
