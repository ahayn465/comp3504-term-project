using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Net;
using System.Threading.Tasks;
using SQLite;

namespace beer_me
{
	public class BreweryDataService
	{
		List<Brewery> breweryList = new List<Brewery>();
		List<Brewery> closestBreweries = new List<Brewery>();
		private string googleMatrixApiString;
		JsonValue rawMatrixData;

		public BreweryDataService()
		{
			Console.WriteLine("Brewery service initialized");

		}

		private string setGoogleApiString( string gps, string destinations )
		{
			googleMatrixApiString = "https://maps.googleapis.com/maps/api/distancematrix/json?units=metric&origins="
				+ gps + "&destinations=place_id:" + destinations + "&key=AIzaSyCUi84JbqtkkTcMsbzOwus0aujOR3CwA5c";
			Console.WriteLine(googleMatrixApiString);
			return googleMatrixApiString;
		}

		public List<Brewery> getBreweryList()
		{
			Console.WriteLine("Getting brewery list");
			return this.breweryList;
		}

		public List<Brewery> getBreweriesToVisit()
		{
			Console.WriteLine("Getting closest brewery list");
			return this.closestBreweries;
		}

		public void setBreweryList(List<Brewery> theList)
		{
			this.breweryList = theList;
			Console.WriteLine("Cheers!! ------------ Breweries set!");

		}

		public IEnumerable<TableBrewery> queryBreweries(string path, string id)
		{
			Console.WriteLine("Wuerying ");
			var db = new SQLiteConnection(path);
			return db.Query<TableBrewery>("SELECT * FROM TableBrewery WHERE breweryId = ?", id);
		}

		public IEnumerable<TableBrewery> clearDatabase(string path)
		{
			var db = new SQLiteConnection(path);
			return db.Query<TableBrewery>("DROP TABLE TableBrewery");
		}

		// REST 

		public async Task<JsonValue> FetchDataAsync(string url)
		{
			Console.WriteLine("Fetching Data");
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


		// Internal async helper methods
		internal async Task<string> getBreweryDataAsync(string requestUrl)
		{
			try
			{
				string urlToFetch = requestUrl;
				JsonValue rawBreweryData = await FetchDataAsync(urlToFetch);
				string answer = generateBreweryList(rawBreweryData);
				return answer;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			return "ERROR";
		}

		internal async Task<string> getBeweryMatrixDataAsync(string gps, string destinations)
		{
			try
			{
				string urlToFetch = setGoogleApiString(gps, destinations);
				Console.WriteLine(urlToFetch);
				rawMatrixData = await FetchDataAsync(urlToFetch);

				Console.WriteLine(rawMatrixData.ToString());

				return "ok";
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			return "ERROR";
		}

		// Data related methods


		public List<Brewery> getListWithMatrixData( List<Brewery> breweriesToVisit )
		{
			Console.WriteLine("In Matrix Data");
			try
			{
				if (rawMatrixData != null && breweriesToVisit != null)
				{

					foreach (var b in breweriesToVisit)
						Console.WriteLine(b.ToString());
					Console.WriteLine(rawMatrixData.ToString()); 


					Console.WriteLine("MATRIX data");
					for (var i = 0; i < breweriesToVisit.Count; i++)
					{
						Console.WriteLine(breweriesToVisit[i].getPlaceId());

						breweriesToVisit[i].setPlaceIdAddress(rawMatrixData["destination_addresses"][i].ToString());
						breweriesToVisit[i].setDistance(rawMatrixData["rows"][0]["elements"][i]["distance"]["text"].ToString());
						breweriesToVisit[i].setTravelTime(rawMatrixData["rows"][0]["elements"][i]["duration"]["text"].ToString());
						breweriesToVisit[i].setTravelTimeRaw(rawMatrixData["rows"][0]["elements"][i]["duration"]["value"].ToString());

					}

					closestBreweries = breweriesToVisit;

					return breweriesToVisit;
				}
				else {
					Console.WriteLine("No Matrix Data");
				}

			}
			catch
			{
				Console.WriteLine("Something went wrong");
			}
			return null;
		}


		private string generateBreweryList(JsonValue breweryData)
		{
			if (breweryData != null)
			{
				foreach (var b in breweryData)
				{
					JsonValue brewery = (JsonValue)b;

					var newBrewery = new Brewery(brewery);

					breweryList.Add(newBrewery);
				}

				return "ok";
			}
			else {
				Console.WriteLine("No Data");
			}

			return "ERROR";
		}

		// SQLite

		//public string createDatabase(string path)
		//{
		//	try
		//	{
		//		var connection = new SQLiteAsyncConnection(path);
		//		connection.CreateTableAsync<TableBrewery>().ContinueWith(t =>
		//		{
		//			//do something here?
		//		});

		//		return "TABLES_CREATED";

		//	}
		//	catch (SQLiteException ex)
		//	{
		//		return ex.Message;
		//	}
		//}

		//public string insertUpdateData(TableBrewery data, string path)
		//{
		//	try
		//	{

		//		var db = new SQLiteAsyncConnection(path);
		//		var r = db.QueryAsync<TableBrewery>("select * from TableBrewery where breweryId = ?", data.ID);

		//		Console.Write(r);
		//		var res = db.InsertAsync(data);
		//		Console.WriteLine("Insert", res);
		//		return "Data added/updated";



		//	}
		//	catch (SQLiteException ex)
		//	{
		//		return ex.Message;
		//	}
		//}



	}
}
