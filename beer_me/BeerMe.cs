using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Util;
using Android.Widget;
using System.Text;
using System;

namespace beer_me
{
	[Activity(Label = "BeerMe")]
	public class BeerMe : Activity, ILocationListener 
	{
		LocationManager locMgr;
		double latitude;
		double longitude;

		TextView statusView;
		Button locateBreweries;

		string provider;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.BeerMe);
			bool viewsReady = generateViews();

			if (viewsReady)
			{
				connectListeners();
			}

			locMgr = GetSystemService(Context.LocationService) as LocationManager;
			provider = LocationManager.GpsProvider;
			Boolean isEnabled = locMgr.IsProviderEnabled(LocationManager.GpsProvider);

			Location lastKnown = locMgr.GetLastKnownLocation(provider);

			if (lastKnown != null)
			{
				updateUserLocation(lastKnown);
			}

			Console.WriteLine("Location manager: ", locMgr);
		}

		private bool generateViews()
		{
			try
			{
				this.statusView = FindViewById<TextView>(Resource.Id.status);
				this.locateBreweries = FindViewById<Button>(Resource.Id.locateBreweries);
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
			this.locateBreweries.Click += delegate
			{
				this.statusView.Text = "Finding you a local brew!";
			};
		}

		private void updateUserLocation(Location userLocation)
		{
			latitude = userLocation.Latitude;
			longitude = userLocation.Longitude;
		}



		// Lifecycle methods
		protected override void OnResume()
		{
			base.OnResume();
			provider = LocationManager.GpsProvider;

			if (locMgr.IsProviderEnabled(provider))
			{
				Console.WriteLine("Provider enabled", provider.ToString()	);
				locMgr.RequestLocationUpdates(provider, 2000, 1, this);
			}
			else
			{

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
			Console.WriteLine("Status changed");
		}

		public void OnLocationChanged(Android.Locations.Location location)
		{
			Console.WriteLine("Location Changed");
			updateUserLocation(location);
 		}

		// end ILocationListener interface


	}
}
