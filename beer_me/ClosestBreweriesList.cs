
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace beer_me
{
	[Activity(Label = "ClosestBreweriesList")]
	public class ClosestBreweriesList : Activity
	{
		BreweryDataService breweryDataService;
		ListView closestBreweriesListView;
		List<Brewery> matrixList = new List<Brewery>();
		List<Brewery> breweries = new List<Brewery>();
		List<Brewery> breweriesToVisit = new List<Brewery>();


		string userCoordinates;
		string googleMatrixApiDestinations;

		// Views
		ClosestBreweriesListAdapter breweryListViewAdapter;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.ClosestBreweriesList);

			userCoordinates = Intent.GetStringExtra("coords") ?? "Data not available";

			closestBreweriesListView = FindViewById<ListView>(Resource.Id.closestBreweriesListView);

			breweryDataService = new BreweryDataService();
			setUpData();

		}

		private async void setUpData()
		{
			string asbaApiUrl = "http://blowfish.asba.development.c66.me/api/breweries";
			var result = await breweryDataService.getBreweryDataAsync(asbaApiUrl);

			if (result != "ERROR")
			{
				breweries = breweryDataService.getBreweryList();
				if (breweries != null)
				{
					Console.WriteLine(breweries.Count);
					foreach (var brewery in breweries)
					{
						if (brewery.getCity() == "Calgary")
						{
							if (brewery.getPlaceId() != null && brewery.getPlaceId() != "")
							{
								Console.WriteLine(brewery.getPlaceId());
								googleMatrixApiDestinations += brewery.getPlaceId() + "|place_id:";
								breweriesToVisit.Add(brewery);
							}
						}

					}
					googleMatrixApiDestinations = googleMatrixApiDestinations.Remove(googleMatrixApiDestinations.Length - 10);
					var result2 = await breweryDataService.getBeweryMatrixDataAsync(userCoordinates, googleMatrixApiDestinations);
					if (result2 == "ok")
					{
						Console.WriteLine(result2);
						matrixList = breweryDataService.getListWithMatrixData(breweriesToVisit);
						makeTheList(matrixList);
					}
				}
			}
		}

		private void makeTheList(List<Brewery> breweries)
		{
			breweries.Sort((a, b) => a.getTravelTimeRaw().CompareTo(b.getTravelTimeRaw()));
			breweryListViewAdapter = new ClosestBreweriesListAdapter(this, breweries);
			closestBreweriesListView.Adapter = breweryListViewAdapter;
			closestBreweriesListView.FastScrollEnabled = true;
			closestBreweriesListView.ItemClick += closestBreweriesListView_ItemClick;
		}

		void closestBreweriesListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			var brewery = this.breweryListViewAdapter.getBreweryAtPostition(e.Position);

			String geo = "geo:0,0?q=" + brewery.getName();
			var geoUri = Android.Net.Uri.Parse(geo);
			var mapIntent = new Intent(Intent.ActionView, geoUri);
			StartActivity(mapIntent);

		}
	}
}
