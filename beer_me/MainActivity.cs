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
	[Activity(Label = "ASBA Brewery Members", MainLauncher = true, Icon = "@mipmap/icon")]
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

			// Sqlite not being used yet
			var docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
			pathToDatabase = System.IO.Path.Combine(docsFolder, "db_sqlnet.db");
			var result = breweryDataService.createDatabase(pathToDatabase);
			Console.WriteLine("----- Database connected with {0} result", result);

			breweryListView = FindViewById<ListView>(Resource.Id.breweryListView);

			GetBreweryDataAsync();

		}

		async Task<String> GetBreweryDataAsync()
		{
			try
			{
				string url = "http://blowfish.asba.development.c66.me/api/breweries";
				rawBreweryData = await breweryDataService.FetchDataAsync(url);
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
			Console.WriteLine(breweryData.ToString());
			if (breweryData != null)
			{
				foreach (var b in breweryData)
				{
					JsonValue brewery = (System.Json.JsonValue)b;
					Console.WriteLine(brewery.ToString());

					var newBrewery = new Brewery(brewery["_id"],
												 brewery["name"],
												 brewery["description"],
												 brewery["address"],
												 brewery["city"],
												 brewery["phone"]);

					var dbBrewery = new TableBrewery
					{
						BreweryId = brewery["_id"],
						Name = brewery["name"],
						Description = brewery["description"],
						Address = brewery["address"],
						City = brewery["city"],
						Phone = brewery["phone"]
					};
					breweries.Add(newBrewery);
					breweryDataService.insertUpdateData(dbBrewery, pathToDatabase);
				}
			}

			// TODO move this to its own method
			breweryListViewAdapter = new BreweryListAdapter(this, breweries);
			breweryListView.Adapter = breweryListViewAdapter;
			breweryListView.FastScrollEnabled = true;
			breweryListView.ItemClick += breweryListView_ItemClick;

			var brew = breweryDataService.queryBreweries(pathToDatabase, brewery.ID);
			//TODO clean up the database and prevent duplicate entries
			foreach (var b in brew)
			{
				Console.WriteLine(b.Name);
			}
		}


		void breweryListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			var brewery = this.breweryListViewAdapter.getBreweryAtPostition(e.Position);
			Toast.MakeText(this, brewery.Name, ToastLength.Short).Show();

			var detailView = new Intent(this, typeof(SingleBrewery));
			detailView.PutExtra("breweryId", brewery.ID);
			StartActivity(detailView);
		
		}

	


	}
}

