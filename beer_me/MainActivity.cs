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

		TextView database;
		JsonValue rawBreweryData;

		List<Brewery> breweries = new List<Brewery>();

		// Views
		ListView breweryListView;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			var docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
			var pathToDatabase = System.IO.Path.Combine(docsFolder, "db_sqlnet.db");

			SetContentView(Resource.Layout.Main);

			// Sqlite not being used yet
			//var result = createDatabase(pathToDatabase);
			//Console.WriteLine("Database connected with {0} result", result);

			collectViews();

			attachListeners();

			GetBreweryDataAsync();

		}


		private void collectViews()
		{
			breweryListView = FindViewById<ListView>(Resource.Id.breweryListView);
		}

		private void attachListeners()
		{
			
		}


		async Task<String> GetBreweryDataAsync()
		{
			try
			{
				string url = "https://sheetsu.com/apis/v1.0/a05f04d4d9d2";
				rawBreweryData = await FetchDataAsync(url);
				database.Text = string.Format("Brewery data retrieved");
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
					JsonValue brewery = (System.Json.JsonValue)b;

					var newBrewery = new Brewery(brewery["id"], 
					                             brewery["name"], 
					                             brewery["address"], 
					                             brewery["city"], 
					                             brewery["phone"], 
					                             brewery["latd"], 
					                             brewery["longd"]);
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
					database.Text = string.Format("Database connected, {0}", t);
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

