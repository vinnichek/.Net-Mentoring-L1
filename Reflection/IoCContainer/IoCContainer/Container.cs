using IoCContainer.Attributes;
using IoCContainer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IoCContainer
{
    public class Container
    {
		private readonly Dictionary<Type, Type> types;
		private readonly IActivator activator;

		public Container(IActivator activator)
		{
			this.activator = activator;
			types = new Dictionary<Type, Type>();
		}

		public void AddType(Type type)
		{
			types.Add(type, type);
		}

		public void AddType(Type baseType, Type type)
		{
			types.Add(baseType, type);
		}

		public void AddAssembly(Assembly assembly)
		{
			var types = assembly.GetTypes();

			foreach (var type in types)
			{
				var importConstructorAttributes = type.GetCustomAttribute<ImportConstructorAttribute>();

				if (importConstructorAttributes != null || HasImportedProperties(type))
				{
					AddType(type);
				}

				var exportAttributes = type.GetCustomAttributes<ExportAttribute>();

				foreach (var exportAttribute in exportAttributes)
				{
					if (exportAttribute.Type != null)
					{
						AddType(exportAttribute.Type, type);
					}
					else
					{
						AddType(type, type);
					}
				}
			}
		}

		public object CreateInstance(Type type)
		{
			return CreateInstanceWithDependencies(type);
		}

		public T CreateInstance<T>()
		{
			return (T)CreateInstanceWithDependencies(typeof(T));
		}

		private bool HasImportedProperties(Type type)
		{
			return type.GetProperties()
				.Where(p => p.GetCustomAttribute<ImportAttribute>() != null).Any();
		}

		private object CreateInstanceWithDependencies(Type type)
		{
			if (!types.ContainsKey(type))
			{
				throw new ArgumentException($"Can't create instance of {type.FullName}.");
			}

			var typeToGetInstance = types[type];
			var constructorInfo = GetConstructor(typeToGetInstance);
			var instance = ResolveConstructor(typeToGetInstance, constructorInfo);

			if (type.GetCustomAttribute<ImportConstructorAttribute>() != null)
			{
				return instance;
			}

			ResolveProperties(type, instance);

			return instance;
		}

		private ConstructorInfo GetConstructor(Type type)
		{
			var constructors = type.GetConstructors();

			if (constructors.Length == 0)
			{
				throw new ArgumentException($"No public constructors for {type.FullName}.");
			}

			return constructors.First();
		}

		private object ResolveConstructor(Type type, ConstructorInfo constructorInfo)
		{
			var constructorsParameters = constructorInfo.GetParameters();
			var resolvedParameters = new List<object>();

			Array.ForEach(constructorsParameters, p => resolvedParameters.Add(CreateInstanceWithDependencies(p.ParameterType)));

			return activator.CreateInstance(type, resolvedParameters.ToArray());
		}

		private void ResolveProperties(Type type, object instance)
		{
			var propertiesToResolve = type.GetProperties().Where(x => x.GetCustomAttribute<ImportAttribute>() != null);

			foreach (var property in propertiesToResolve)
			{
				property.SetValue(instance, CreateInstanceWithDependencies(property.PropertyType));
			}
		}
	}
}
