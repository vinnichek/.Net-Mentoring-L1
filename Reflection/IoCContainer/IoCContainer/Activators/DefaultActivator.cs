using System;
using IoCContainer.Interfaces;

namespace IoCContainer.Activators
{
	public class DefaultActivator : IActivator
	{
		public object CreateInstance(Type type, params object[] parameters)
		{
			return Activator.CreateInstance(type, parameters);
		}
	}
}
