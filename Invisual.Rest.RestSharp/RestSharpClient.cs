using RestSharp;
using Invisual.Rest.RestSharp.Extensions;
using Invisual.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

namespace Invisual.Rest.RestSharp
{
	public class RestSharpClient : IRestClient
	{
		private readonly ISerializer _serializer;

		public RestSharpClient(ISerializer serializer)
		{
			if (serializer == null)
				throw new ArgumentNullException("serializer");

			_serializer = serializer;
		}

		/// <summary>
		/// Make a request to the specified uri. The response will be deserialized into the specified type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="uri"></param>
		/// <param name="requestMethod"></param>
		/// <returns></returns>
		/// <exception cref="InvalidEnumArgumentException"><exception cref="ArgumentNullException"></exception></exception><exception cref="ArgumentException"></exception><exception cref="UriFormatException"></exception><exception cref="Non200HttpStatusException"></exception>
		public T Request<T>(string uri, HttpRequestMethod requestMethod)
		{
			if (string.IsNullOrWhiteSpace(uri))
				throw new ArgumentException("uri must not be null, empty, or whitespace");
			if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
				throw new UriFormatException();
			if (!Enum.IsDefined(typeof(HttpRequestMethod), requestMethod))
				throw new InvalidEnumArgumentException("requestMethod", (int)requestMethod, typeof(HttpRequestMethod));

			var restRequest = new RestRequest(requestMethod.ToRestSharpRequestMethod());

			var response = CreateRestClient(uri).Execute(restRequest);

			if (response.StatusCode != HttpStatusCode.OK)
				throw new Non200HttpStatusException(response.Content, (int)response.StatusCode);

			return _serializer.Deserialize<T>(response.Content);
		}

		/// <summary>
		/// Make a request to the specified uri.
		/// The specified requestData will be serialized and sent as part of the request body.
		/// To send request data using a different parameter type, use the <see cref="RestSharpClient.Request{T}(string, HttpRequestMethod, object, RequestParamType)"/> overload.
		/// The response will be deserialized into the specified type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="uri"></param>
		/// <param name="requestMethod"></param>
		/// <param name="requestData"></param>
		/// <returns></returns>
		/// <exception cref="InvalidEnumArgumentException"><exception cref="ArgumentNullException"></exception></exception><exception cref="ArgumentException"></exception><exception cref="UriFormatException"></exception><exception cref="Non200HttpStatusException"></exception>
		public T Request<T>(string uri, HttpRequestMethod requestMethod, object requestData)
		{
			if (string.IsNullOrWhiteSpace(uri))
				throw new ArgumentException("uri must not be null, empty, or whitespace");
			if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
				throw new UriFormatException();
			if (!Enum.IsDefined(typeof(HttpRequestMethod), requestMethod))
				throw new InvalidEnumArgumentException("requestMethod", (int)requestMethod, typeof(HttpRequestMethod));
			if (requestData == null)
				throw new ArgumentNullException("requestData");

			var restRequest = new RestRequest(requestMethod.ToRestSharpRequestMethod());
			restRequest.AddParameter("application/json; charset=utf-8", _serializer.Serialize(requestData), ParameterType.RequestBody);

			var response = CreateRestClient(uri).Execute(restRequest);

			if (response.StatusCode != HttpStatusCode.OK)
				throw new Non200HttpStatusException(response.Content, (int)response.StatusCode);

			return _serializer.Deserialize<T>(response.Content);
		}

		/// <summary>
		/// Make a request to the specified uri.
		/// The response will be deserialized into the specified type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="uri"></param>
		/// <param name="requestMethod"></param>
		/// <param name="requestData"></param>
		/// <param name="paramType"></param>
		/// <returns></returns>
		/// <exception cref="InvalidEnumArgumentException"><exception cref="ArgumentNullException"></exception></exception><exception cref="ArgumentException"></exception><exception cref="UriFormatException"></exception><exception cref="Non200HttpStatusException"></exception>
		public T Request<T>(string uri, HttpRequestMethod requestMethod, object requestData, RequestParamType paramType)
		{
			if (string.IsNullOrWhiteSpace(uri))
				throw new ArgumentException("uri must not be null, empty, or whitespace");
			if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
				throw new UriFormatException();
			if (!Enum.IsDefined(typeof(HttpRequestMethod), requestMethod))
				throw new InvalidEnumArgumentException("requestMethod", (int)requestMethod, typeof(HttpRequestMethod));
			if (requestData == null)
				throw new ArgumentNullException("requestData");
			if (!Enum.IsDefined(typeof(RequestParamType), paramType))
				throw new InvalidEnumArgumentException("requestMethod", (int)paramType, typeof(RequestParamType));

			var restRequest = new RestRequest(requestMethod.ToRestSharpRequestMethod());

			switch (paramType)
			{
				case RequestParamType.QueryString:
					restRequest.AddObject(requestData);
					break;

				case RequestParamType.RequestBody:
					restRequest.AddParameter("application/json; charset=utf-8", _serializer.Serialize(requestData), ParameterType.RequestBody);
					break;
				default:
					throw new NotSupportedException("Unsupported RequestParamType");
			}

			var response = CreateRestClient(uri).Execute(restRequest);

			if (response.StatusCode != HttpStatusCode.OK)
				throw new Non200HttpStatusException(response.Content, (int)response.StatusCode);

			return _serializer.Deserialize<T>(response.Content);
		}

		/// <summary>
		/// Null by default. Set this property to use a form of Authentication.
		/// </summary>
		public IAuthenticator Authenticator { get; set; }

		/// <summary>
		/// Create a new <see cref="RestClient"/> instance using the specified uri. 
		/// </summary>
		/// <param name="uri"></param>
		/// <returns></returns>
		private RestClient CreateRestClient(string uri)
		{
			var client = new RestClient(uri);

			if (Authenticator != null)
				client.Authenticator = Authenticator;

			return client;
		}
	}
}
