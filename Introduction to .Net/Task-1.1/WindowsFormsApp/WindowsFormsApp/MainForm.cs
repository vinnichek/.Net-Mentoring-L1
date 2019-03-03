using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}
		
		private void writeHelloButton_Click(object sender, EventArgs e)
		{ 
			var name = this.textBoxForName.Text;

			MessageBox.Show($"Hello, {name}!");
		}
	}
}
