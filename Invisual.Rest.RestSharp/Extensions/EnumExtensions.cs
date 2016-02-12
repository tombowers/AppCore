using RestSharp;
using System;

namespace Invisual.Rest.RestSharp.Extensions
{
	public static class EnumExtensions
	{
		public static Method ToRestSharpRequestMethod(this HttpRequestMethod method)
		{
			switch(method)
			{
				case HttpRequestMethod.Get:
					return Method.GET;
				case HttpRequestMethod.Post:
					return Method.POST;
				default:
					throw new NotSupportedException("Request method not supported.");
			}
		}
	}
}
