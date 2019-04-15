using IocContainer.Test.TestData.Interfaces;
using IoCContainer.Attributes;

namespace IocContainer.Test.TestData
{
	[Export(typeof(ICustomerDAL))]
	public class CustomerDAL : ICustomerDAL
	{
		public CustomerDAL() { }
	}
}
