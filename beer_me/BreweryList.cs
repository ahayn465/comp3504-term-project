
using System;
using System.Collections.Generic;
using System.Json;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace beer_me
{
	[Activity(Label = "BreweryList")]
	public class BreweryList : Activity
	{
		JsonValue rawBreweryData;

		List<Brewery> breweries = new List<Brewery>();

		// Views
		ListView breweryListView;
		BreweryListAdapter breweryListViewAdapter;

		BreweryDataService breweryDataService;


		protected override void OnCreate(Bundle savedInstanceState)
		{

			breweryDataService = new BreweryDataService();

			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.BreweryList);

			breweryListView = FindViewById<ListView>(Resource.Id.breweryListView);

			breweries = breweryDataService.getBreweryList();

			Task<String> breweriesReady = GetBreweryDataAsync();
		}

		async Task<String> GetBreweryDataAsync()
		{
			try
			{
				string url = "http://blowfish.asba.development.c66.me/api/breweries";
				JsonValue rawBreweryData = await breweryDataService.FetchDataAsync(url);
				generateBreweryList(rawBreweryData);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			return "OK";
		}


		private void generateBreweryList(JsonValue breweryData)
		{
			if (breweryData != null)
			{
				foreach (var b in breweryData)
				{
					JsonValue brewery = (JsonValue)b;

					var newBrewery = new Brewery( brewery );

					breweries.Add(newBrewery);
				}

				breweryDataService.setBreweryList(breweries);

				makeTheList(breweries);
			}
			else {
				Console.WriteLine("No Data");
			}

		}

		private void makeTheList(List<Brewery> breweries)
		{

			// TODO move this to its own method
			breweryListViewAdapter = new BreweryListAdapter(this, breweries);
			breweryListView.Adapter = breweryListViewAdapter;
			breweryListView.FastScrollEnabled = true;
			breweryListView.ItemClick += breweryListView_ItemClick;
		}


		void breweryListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			var brewery = this.breweryListViewAdapter.getBreweryAtPostition(e.Position);

			var detailView = new Intent(this, typeof(SingleBrewery));
			detailView.PutExtra("breweryId", brewery.getId());
			StartActivity(detailView);

		}
	}
}
