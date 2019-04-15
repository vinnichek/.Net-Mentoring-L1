using IoCContainer.Interfaces;
using System;
using System.Linq;
using System.Reflection.Emit;

namespace IoCContainer.Activators
{
	public class EmitActivator : IActivator
	{
		public object CreateInstance(Type type, params object[] parameters)
		{
			var types = parameters.Select(p => p.GetType()).ToArray();
			var method = new DynamicMethod(string.Empty, type, types);
			var ilGenerator = method.GetILGenerator();

			for (int i = 0; i < parameters.Length; i++)
			{
				ilGenerator.Emit(OpCodes.Ldarg, i);
			}

			var ctor = type.GetConstructor(types);

			ilGenerator.Emit(OpCodes.Newobj, ctor);
			ilGenerator.Emit(OpCodes.Ret);

			return method.Invoke(null, parameters);
		}
	}
}
