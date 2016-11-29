using System;
using System.Collections.Generic;

using Android.App;
using Android.Widget;
using Android.OS;

using SQLite;
using System.Json;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using Android.Content;

namespace beer_me
{
	[Activity(MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{

		JsonValue rawBreweryData;

		List<Brewery> breweries = new List<Brewery>();
		string pathToDatabase;

		// Views
		ListView breweryListView;
		BreweryListAdapter breweryListViewAdapter;

		BreweryDataService breweryDataService;


		protected override void OnCreate(Bundle savedInstanceState)
		{
			breweryDataService = new BreweryDataService();

			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Main);

			// Create SQLite database to cache brewery data 
			//var docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
			//pathToDatabase = System.IO.Path.Combine(docsFolder, "db_sqlnet.db");
			//var result = breweryDataService.createDatabase(pathToDatabase);
			//Console.WriteLine("------------------------- {0} ", result);

			breweryListView = FindViewById<ListView>(Resource.Id.breweryListView);


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

					var newBrewery = new Brewery(brewery["_id"],
												 brewery["name"],
												 brewery["description"],
												 brewery["address"],
												 brewery["city"],
												 brewery["phone"], 
					                             brewery["image"],
					                             brewery["latd"],
											 	 brewery["longd"]);

					breweries.Add(newBrewery);
				}
			}
			else {
				Console.WriteLine("No Data");
			}

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

