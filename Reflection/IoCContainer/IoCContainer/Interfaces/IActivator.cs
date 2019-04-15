using System;

namespace IoCContainer.Interfaces
{
	public interface IActivator
	{
		object CreateInstance(Type type, params object[] parameters);
	}
}
