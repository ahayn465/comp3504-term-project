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
		Button button;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			var docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
			var pathToDatabase = System.IO.Path.Combine(docsFolder, "db_sqlnet.db");

			SetContentView(Resource.Layout.Main);


			// Database connection 
			database = FindViewById<TextView>(Resource.Id.databaseConnectionTester);

			database.Text = string.Format("waiting...");

			var result = createDatabase(pathToDatabase);
			Console.WriteLine("Database connected with {0} result", result);

			collectViews();

			attachListeners();

		}


		private void collectViews()
		{
			button = FindViewById<Button>(Resource.Id.button1);
		}

		private void attachListeners()
		{
			button.Click += async (sender, e) =>
			{
				try
				{
					string url = "https://sheetsu.com/apis/v1.0/a05f04d4d9d2";
					rawBreweryData = await FetchDataAsync(url);
					database.Text = string.Format("Brewery data retrieved");
					populateBreweriesTable(rawBreweryData);
				}
				catch (Exception)
				{
					Console.WriteLine(e);
				}

			};
		}


		private void populateBreweriesTable(JsonValue breweryData)
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
	}
}

