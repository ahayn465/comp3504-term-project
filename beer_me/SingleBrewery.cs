
using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace beer_me
{
	[Activity()]
	public class SingleBrewery : Activity
	{
		string breweryId;
		JsonValue rawBreweryData;
		BreweryDataService breweryDataService;
		Brewery brewery;

		TextView breweryName;
		TextView breweryDescription;
		ImageView breweryImage;
		Button mapView;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			breweryDataService = new BreweryDataService();

			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.SingleBrewery);

			breweryId = Intent.GetStringExtra("breweryId") ?? "Error retrieving data";

			findViews();

			GetBreweryDataAsync();
		}

		private void findViews()
		{
			this.breweryName = FindViewById<TextView>(Resource.Id.breweryName);
			this.breweryDescription = FindViewById<TextView>(Resource.Id.breweryDescription);
			this.breweryImage = FindViewById<ImageView>(Resource.Id.breweryImage);
			this.mapView = FindViewById<Button>(Resource.Id.mapView);

			this.mapView.Click += delegate
			{
				String geo = "geo:0,0?q=" + brewery.getName(); 
				var geoUri = Android.Net.Uri.Parse(geo);
				var mapIntent = new Intent(Intent.ActionView, geoUri);
				StartActivity(mapIntent);
			};
		}

		private void populateBrewery(Brewery brewery)
		{

			String imageString = brewery.getImage();

			this.breweryName.Text = brewery.getName();
			this.breweryDescription.Text = brewery.getDescription();


			var resourceId = (int)typeof(Resource.Drawable).GetField(imageString).GetValue(null);

			this.breweryImage.SetImageResource(resourceId);
		}


		private async void GetBreweryDataAsync()
		{
			try
			{
				string url = "http://blowfish.asba.development.c66.me/api/breweries/" + breweryId;
				rawBreweryData = await breweryDataService.FetchDataAsync(url);
				generateBrewery((System.Json.JsonObject)rawBreweryData);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		private void generateBrewery(JsonObject breweryData)
		{
			
			if (breweryData != null)
			{

				var newBrewery = new Brewery(breweryData["_id"],
											 breweryData["name"],
											 breweryData["description"],
											 breweryData["address"],
											 breweryData["city"],
											 breweryData["phone"], 
			                            	 breweryData["image"],
				                             breweryData["latd"],
				                             breweryData["longd"]);

				populateBrewery(newBrewery);

				//this.breweryName.Text = newBrewery.getName();
				//this.breweryDescription.Text = newBrewery.getDescription();
				this.brewery = newBrewery;

			}
			else {
				Console.WriteLine("No Data");
			}

		}

	}
}
