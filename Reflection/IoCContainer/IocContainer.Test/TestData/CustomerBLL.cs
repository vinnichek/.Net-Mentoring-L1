using IocContainer.Test.TestData.Interfaces;
using IoCContainer.Attributes;

namespace IocContainer.Test.TestData
{
	[ImportConstructor]
	public class CustomerBLLConstructor
	{
		public CustomerBLLConstructor(ICustomerDAL dal, Logger logger) { }
	}

	public class CustomerBLLProperties
	{
		[Import]
		public ICustomerDAL CustomerDAL { get; set; }

		[Import]
		public Logger Logger { get; set; }
	}
}
