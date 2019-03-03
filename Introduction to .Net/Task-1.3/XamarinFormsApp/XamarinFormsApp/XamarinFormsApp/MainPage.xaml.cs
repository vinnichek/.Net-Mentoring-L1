using NetStandardClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinFormsApp
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private void HelloButton_Clicked(object sender, EventArgs e)
		{
			var name = NameEntry.Text;
			Label.Text = ClassWriter.WriteHello(name);
		}
	}
}
