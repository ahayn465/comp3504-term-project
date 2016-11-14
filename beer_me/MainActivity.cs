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
using System.Linq;

namespace beer_me
{
	[Activity(Label = "beer_me", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{

		JsonValue rawBreweryData;

		List<Brewery> breweries = new List<Brewery>();
		string pathToDatabase;

		// Views
		ListView breweryListView;
		BreweryListAdapter breweryListViewAdapter;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Main);

			// Sqlite not being used yet
			var docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
			pathToDatabase = System.IO.Path.Combine(docsFolder, "db_sqlnet.db");
			var result = createDatabase(pathToDatabase);
			Console.WriteLine("----- Database connected with {0} result", result);

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

					var dbBrewery = new TableBrewery { 
						BreweryId = brewery["_id"],
					 	Name = brewery["name"],
						Description = brewery["description"],
						Address = brewery["address"],
						City = brewery["city"],
						Phone = brewery["phone"]
					};
					breweries.Add(newBrewery);
					insertUpdateData(dbBrewery, pathToDatabase);
				}
			}
			breweryListViewAdapter = new BreweryListAdapter(this, breweries);
			breweryListView.Adapter = breweryListViewAdapter;
			breweryListView.FastScrollEnabled = true;
			breweryListView.ItemClick += breweryListView_ItemClick;
		}


		void breweryListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			var brewery = this.breweryListViewAdapter.getBreweryAtPostition(e.Position);
			Toast.MakeText(this, brewery.Name, ToastLength.Short).Show();
			var brew = queryBreweries(pathToDatabase, brewery.ID);

			var l = brew.Count();
			Console.WriteLine(l);
			foreach (var b in brew)
			{
				Console.WriteLine(b.Name);
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

		// SQLite

		private string createDatabase(string path)
		{
			try
			{
				var connection = new SQLiteAsyncConnection(path);
				connection.CreateTableAsync<TableBrewery>().ContinueWith(t =>
				{
					Console.WriteLine("-----Tables created" + path);
				});

				return "Database created";

			}
			catch (SQLiteException ex)
			{
				return ex.Message;
			}
		}

		private string insertUpdateData(TableBrewery data, string path)
		{
			try
			{
				
				var db = new SQLiteAsyncConnection(path);
				var r = db.QueryAsync<TableBrewery>("select * from TableBrewery where breweryId = ?", data.ID);

				Console.Write(r);
				var res = db.InsertAsync(data);
				Console.WriteLine("Insert", res);
				return "Data added/updated";



			}
			catch (SQLiteException ex)
			{
				return ex.Message;
			}
		}

		public static IEnumerable<TableBrewery> queryBreweries(string path, string id)
		{
			
			var db = new SQLiteConnection(path);
			return db.Query<TableBrewery>("select * from TableBrewery where breweryId = ?", id);
		}

	}
}

