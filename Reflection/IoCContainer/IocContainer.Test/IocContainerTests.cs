using IocContainer.Test.TestData;
using IocContainer.Test.TestData.Interfaces;
using IoCContainer;
using IoCContainer.Activators;
using NUnit.Framework;
using System.Reflection;

namespace Tests
{
	public class IocContainerTests
	{
		private Container container;

		[SetUp]
		public void ContainerTestsInitialize()
		{
			container = new Container(new DefaultActivator());
		}

		[Test]
		public void CreateInstance_WhenAssemblyAdded_ConstructorInjection()
		{
			container.AddAssembly(Assembly.GetExecutingAssembly());

			var customerConstructor = container.CreateInstance(typeof(CustomerBLLConstructor));
			var customerConstructorGeneric = container.CreateInstance<CustomerBLLConstructor>();

			Assert.IsNotNull(customerConstructor);
			Assert.IsTrue(customerConstructor.GetType() == typeof(CustomerBLLConstructor));

			Assert.IsNotNull(customerConstructorGeneric);
			Assert.IsTrue(customerConstructorGeneric.GetType() == typeof(CustomerBLLConstructor));
		}

		[Test]
		public void CreateInstance_WhenAssemblyAdded_PropertyInjection()
		{
			container.AddAssembly(Assembly.GetExecutingAssembly());

			var customerProperty = container.CreateInstance(typeof(CustomerBLLProperties));
			var customerPropertyGeneric = container.CreateInstance<CustomerBLLProperties>();

			Assert.IsNotNull(customerProperty);
			Assert.IsTrue(customerProperty.GetType() == typeof(CustomerBLLProperties));

			Assert.IsNotNull(customerPropertyGeneric);
			Assert.IsTrue(customerPropertyGeneric.GetType() == typeof(CustomerBLLProperties));
		}

		[Test]
		public void CreateInstance_WhenTypesAdded_PropertyInjection()
		{
			container.AddType(typeof(CustomerBLLProperties));
			container.AddType(typeof(Logger));
			container.AddType(typeof(ICustomerDAL), typeof(CustomerDAL));

			var customerProperty = (CustomerBLLProperties)container.CreateInstance(typeof(CustomerBLLProperties));
			var customerPropertyGeneric = container.CreateInstance<CustomerBLLProperties>();

			Assert.IsNotNull(customerProperty.Logger);
			Assert.IsTrue(customerProperty.Logger.GetType() == typeof(Logger));

			Assert.IsNotNull(customerPropertyGeneric.Logger);
			Assert.IsTrue(customerPropertyGeneric.Logger.GetType() == typeof(Logger));

			Assert.IsNotNull(customerProperty.CustomerDAL);
			Assert.IsTrue(customerProperty.CustomerDAL.GetType() == typeof(CustomerDAL));

			Assert.IsNotNull(customerPropertyGeneric.CustomerDAL);
			Assert.IsTrue(customerPropertyGeneric.CustomerDAL.GetType() == typeof(CustomerDAL));

			Assert.IsNotNull(customerProperty);
			Assert.IsTrue(customerProperty.GetType() == typeof(CustomerBLLProperties));

			Assert.IsNotNull(customerPropertyGeneric);
			Assert.IsTrue(customerPropertyGeneric.GetType() == typeof(CustomerBLLProperties));
		}

		[Test]
		public void CreateInstance_WithEmitActivator_ConstructorInjection()
		{
			container = new Container(new EmitActivator());

			container.AddType(typeof(CustomerBLLConstructor));
			container.AddType(typeof(Logger));
			container.AddType(typeof(ICustomerDAL), typeof(CustomerDAL));

			var customerConstructor = container.CreateInstance<CustomerBLLConstructor>();

			Assert.IsNotNull(customerConstructor);
			Assert.IsTrue(customerConstructor.GetType() == typeof(CustomerBLLConstructor));
		}
	}
}