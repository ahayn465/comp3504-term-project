
using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
		string breweryId;
		JsonValue rawBreweryData;
		BreweryDataService breweryDataService;

		TextView breweryName;
		TextView breweryDescription;



		protected override void OnCreate(Bundle savedInstanceState)
		{
			breweryDataService = new BreweryDataService();

			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.SingleBrewery);

			breweryId = Intent.GetStringExtra("breweryId") ?? "Error retrieving data";

			Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++  >>" + breweryId);
			this.findViews();

			GetBreweryDataAsync();
		}

		private void findViews()
		{
			this.breweryName = FindViewById<TextView>(Resource.Id.breweryName);
			this.breweryDescription = FindViewById<TextView>(Resource.Id.breweryDescription);
		}

		private void populateBrewery(Brewery brewery)
		{
			Console.WriteLine(brewery.toString());

			this.breweryName.Text = brewery.getName();
			this.breweryDescription.Text = brewery.getDescription();
		}


		private async void GetBreweryDataAsync()
		{
			try
			{
				string url = "http://blowfish.asba.development.c66.me/api/breweries/" + breweryId;
				rawBreweryData = await breweryDataService.FetchDataAsync(url);
				Console.WriteLine(rawBreweryData.GetType());
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
											 breweryData["phone"]);

				Console.WriteLine(newBrewery.getName());

				this.breweryName.Text = newBrewery.getName();
				this.breweryDescription.Text = newBrewery.getDescription();

			}
			else {
				Console.WriteLine("No Data");
			}

		}

		//async Task<Brewery> GetBreweryDataAsync()
		//{
		//	try
		//	{
		//		string url = "http://blowfish.asba.development.c66.me/api/breweries/" + breweryId;
		//		rawBreweryData = await breweryDataService.FetchDataAsync(url);

		//		foreach (var b in rawBreweryData)
		//		{
		//			JsonValue brewery = (JsonValue)b;
		//			Console.WriteLine(brewery["name"].ToString());

		//			var newBrewery = new Brewery(brewery["_id"],
		//										 brewery["name"],
		//										 brewery["description"],
		//										 brewery["address"],
		//										 brewery["city"],
		//										 brewery["phone"]);

		//			Console.WriteLine(newBrewery.ID);
		//			return newBrewery;
		//		}

		//	}
		//	catch (Exception e)
		//	{
		//		Console.WriteLine(e);
		//	}

		//	return null;
		//}
	}
}
