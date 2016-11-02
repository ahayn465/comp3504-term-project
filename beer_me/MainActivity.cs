using System;
using System.Collections.Generic;

using Android.App;
using Android.Widget;
using Android.OS;

using SQLite;

namespace beer_me
{
	[Activity(Label = "beer_me", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);


            // create DB path
            var docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
			var pathToDatabase = System.IO.Path.Combine(docsFolder, "db_sqlnet.db");


			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			TextView database = FindViewById<TextView>(Resource.Id.databaseConnectionTester);

			database.Text = string.Format("waiting...");

			var result = createDatabase(pathToDatabase);
			Console.WriteLine("Database connected with {0} result", result);

		}

		private string createDatabase( string path )
		{
			try
			{
				var connection = new SQLiteAsyncConnection(path);
				connection.CreateTableAsync<Brewery>();

				TextView database = FindViewById<TextView>(Resource.Id.databaseConnectionTester);
				database.Text = string.Format("Database connected");
				return "Database created";
				
			}
			catch(SQLiteException ex)
			{
				return ex.Message;
			}
		}


	}
}

