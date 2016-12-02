using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace beer_me
{
	[Activity(MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		Button findBrewButton;
		Button breweryListButton;


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Main);

			findBrewButton = FindViewById<Button>(Resource.Id.findBrewButton);
			breweryListButton = FindViewById<Button>(Resource.Id.breweryListButton);

			findBrewButton.Click += delegate {
				Intent goToFindBrew = new Intent(this, typeof(BeerMe));
				StartActivity(goToFindBrew);
			};

			breweryListButton.Click += delegate {
				Intent goToFindBrew = new Intent(this, typeof(BreweryList));
				StartActivity(goToFindBrew);
			};

		}

	}
}

