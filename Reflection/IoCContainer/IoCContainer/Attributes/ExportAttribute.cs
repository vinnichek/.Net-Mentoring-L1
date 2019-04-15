using System;

namespace IoCContainer.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ExportAttribute : Attribute
	{
		public Type Type { get; private set; }

		public ExportAttribute() { }

		public ExportAttribute(Type type)
		{
			Type = type;
		}
	}
}
