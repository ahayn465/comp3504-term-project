
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace beer_me
{
	[Activity(Label = "Brewery Detailed View")]
	public class SingleBrewery : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			string breweryId = Intent.GetStringExtra("breweryId") ?? "Error retrieving data";

			Console.WriteLine(breweryId);
		}
	}
}
