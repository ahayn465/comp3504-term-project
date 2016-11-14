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

using Newtonsoft.Json;

namespace beer_me
{
	[Activity(Label = "beer_me", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{

		JsonValue rawBreweryData;

		List<Brewery> breweries = new List<Brewery>();

		// Views
		ListView breweryListView;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Main);

			// Sqlite not being used yet
			//var docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
			//var pathToDatabase = System.IO.Path.Combine(docsFolder, "db_sqlnet.db");
			//var result = createDatabase(pathToDatabase);
			//Console.WriteLine("Database connected with {0} result", result);

			breweryListView = FindViewById<ListView>(Resource.Id.breweryListView);

			GetBreweryDataAsync();

		}


		async Task<String> GetBreweryDataAsync()
		{
			try
			{
				string url = "http://blowfish.asba.development.c66.me/api/breweries";
				rawBreweryData = await FetchDataAsync(url);
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
					breweries.Add(newBrewery);
				}
			}
			breweryListView.Adapter = new BreweryListAdapter(this, breweries);
			breweryListView.FastScrollEnabled = true;
		}


		// REST 

		private async Task<JsonValue> FetchDataAsync(string url)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
			request.ContentType = "application/json";
			request.Method = "GET";

			using (WebResponse response = await request.GetResponseAsync())
			{
				using (Stream stream = response.GetResponseStream())
				{
					JsonValue jsonDoc = await Task.Run(() => JsonValue.Load(stream));
					return jsonDoc;
				}
			}
		}

		// SQLite

		private string createDatabase(string path)
		{
			try
			{
				var connection = new SQLiteAsyncConnection(path);
				connection.CreateTableAsync<TableBrewery>().ContinueWith(t =>
				{
					//do something
				});

				return "Database created";

			}
			catch (SQLiteException ex)
			{
				return ex.Message;
			}
		}

	}
}

