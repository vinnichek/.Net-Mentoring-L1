using System;

namespace NetStandardClassLibrary
{
	public class ClassWriter
	{
		public static string WriteHello(string name)
		{
			string currentTime = DateTime.Now.ToLongTimeString();
			return $"{currentTime}: Hello, {name}!";
		}
	}
}
