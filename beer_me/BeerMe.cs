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

		TextView latitudeView;
		TextView longitudeView;
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
			Location lastKnown = locMgr.GetLastKnownLocation(provider);

			if (lastKnown != null)
			{
				updateUserLocation(lastKnown);
			}

			Console.WriteLine("Location manager: ", locMgr.ToString());
		}

		private bool generateViews()
		{
			try
			{
				this.latitudeView = FindViewById<TextView>(Resource.Id.latitude);
				this.longitudeView = FindViewById<TextView>(Resource.Id.longitude);
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

			latitudeView.Text = "Latitude: " + latitude;
			longitudeView.Text = "Longitude: " + longitude;
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
