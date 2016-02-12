﻿namespace Invisual.Serialization
{
	public interface ISerializer
	{
		string Serialize(object data);
		T Deserialize<T>(string data);
	}
}
