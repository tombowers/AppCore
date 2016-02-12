using System;

namespace Invisual.Serialization.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class CustomNameAttribute : Attribute
	{
		public CustomNameAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; private set; }
	}
}
