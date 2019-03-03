using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;

namespace XamarinNativeApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

			Button btn = FindViewById<Button>(Resource.Id.writeHelloButton);
			btn.Click += this.Btn_Click;
        }

		private void Btn_Click(object sender, EventArgs e)
		{
			EditText editTextName = this.FindViewById<EditText>(Resource.Id.editTextName);
			TextView textViewHello = this.FindViewById<TextView>(Resource.Id.textViewHello);

			textViewHello.Text = $"Hello, {editTextName.Text}!";
		}
	}
}