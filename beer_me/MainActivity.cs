using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Locations;

using Android.Util;
using System.Diagnostics.Contracts;

namespace beer_me
{
	[Activity(MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity, ILocationListener
	{
		Context context;

		TextView coordinates;
		TextView statusView;
		Button locateBreweries;
		Button findBrewButton;
		Button breweryListButton;

		LocationManager locMgr;
		double latitude;
		double longitude;
		string provider;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Main);

			bool viewsReady = generateViews();

			if (viewsReady)
			{
				connectListeners();
			}

			locMgr = GetSystemService(Context.LocationService) as LocationManager;
			provider = LocationManager.GpsProvider;
			Location lastKnown = locMgr.GetLastKnownLocation(provider);

			if (lastKnown != null)
			{
				updateUserLocation(lastKnown);
			}

		}

		private bool generateViews()
		{
			try
			{
				findBrewButton = FindViewById<Button>(Resource.Id.findBrewButton);
				breweryListButton = FindViewById<Button>(Resource.Id.breweryListButton);
				coordinates = FindViewById<TextView>(Resource.Id.coordinates);
				statusView = FindViewById <TextView> (Resource.Id.status);

				statusView.Text = "Hey! Welcome to Local Brew \U0001F37A";
				// \U0001F37A is cheers

				return true;
			}
			catch (Exception)
			{
				Console.WriteLine("There was an error collecting views");
				return false;
			}
		}

		private void connectListeners()
		{
			findBrewButton.Click += delegate {
				statusView.Text = "Finding your local brew";	
			};

			breweryListButton.Click += delegate {
				Intent goToBreweryList = new Intent(this, typeof(BreweryList));
				StartActivity(goToBreweryList);
			};
		}

		private void updateUserLocation(Location userLocation)
		{
			latitude = userLocation.Latitude;
			longitude = userLocation.Longitude;

			coordinates.Text = latitude + "," + longitude;
		}


		// Lifecycle methods
		protected override void OnResume()
		{
			base.OnResume();
			provider = LocationManager.GpsProvider;

			if (locMgr.IsProviderEnabled(provider))
			{
				Console.WriteLine("Provider enabled", provider.ToString());
				locMgr.RequestLocationUpdates(provider, 2000, 1, this);
			}
			else
			{

				Console.WriteLine("Provider not enabled");
				Log.Info((string)locMgr, provider + " is not available. Does the device have location services enabled?");
			}
		}

		protected override void OnPause()
		{
			base.OnPause();
			locMgr.RemoveUpdates(this);
		}


		// ILocationListener implementation

		public void OnProviderEnabled(string provider)
		{
			Console.WriteLine(provider, "enabled");
		}

		public void OnProviderDisabled(string provider)
		{
			Console.WriteLine(provider, "disabled");
		}

		public void OnStatusChanged(string provider, Availability status, Bundle extras)
		{
			Console.WriteLine("Status Changed");
			statusView.Text = "Status: " + status;
		}

		public void OnLocationChanged(Android.Locations.Location location)
		{
			Console.WriteLine("Location Changed");
			updateUserLocation(location);
		}

		// end ILocationListener interface

	}
}

