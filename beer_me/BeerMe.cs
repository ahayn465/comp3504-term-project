using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Util;
using Android.Widget;

namespace beer_me
{
	[Activity(Label = "BeerMe")]
	public class BeerMe : Activity, ILocationListener 
	{
		LocationManager locMgr;
		TextView latitude;
		TextView longitude;
		TextView status;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.BeerMe);
			this.latitude = FindViewById<TextView>(Resource.Id.latitude);
			this.longitude = FindViewById<TextView>(Resource.Id.longitude);
			this.status = FindViewById<TextView>(Resource.Id.status);

			locMgr = GetSystemService(Context.LocationService) as LocationManager;

		}

		protected override void OnResume()
		{
			base.OnResume();
			string Provider = LocationManager.GpsProvider;

			if (locMgr.IsProviderEnabled(Provider))
			{
				locMgr.RequestLocationUpdates(Provider, 2000, 1, this);
			}
			else
			{
				Log.Info((string)locMgr, Provider + " is not available. Does the device have location services enabled?");
			}
		}


		// ILocationListener implementation

		public void OnProviderEnabled(string provider)
		{
		   
		}

		public void OnProviderDisabled(string provider)
		{

		}

		public void OnStatusChanged(string provider, Availability status, Bundle extras)
		{
			latitude.Text = "Status: " + status;
		}

		public void OnLocationChanged(Android.Locations.Location location)
		{
			latitude.Text = "Latitude: " + location.Latitude;
			longitude.Text = "Longitude: " + location.Longitude;
 		}

		protected override void OnPause()
		{
			base.OnPause();
			locMgr.RemoveUpdates(this); 
		}

	}
}
