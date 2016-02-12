namespace Invisual.Rest
{
	public interface IRestClient
	{
		T Request<T>(string uri, HttpRequestMethod requestMethod);
		T Request<T>(string uri, HttpRequestMethod requestMethod, object requestData);
		T Request<T>(string uri, HttpRequestMethod requestMethod, object requestData, RequestParamType paramType);
	}
}
