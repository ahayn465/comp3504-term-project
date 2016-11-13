using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Android.App;
using Android.Views;
using Android.Widget;

namespace beer_me
{
	public class BreweryListAdapter:BaseAdapter<Brewery>
	{

		List<Brewery> breweries; 
		Activity context;

		public BreweryListAdapter( Activity context, List<Brewery> breweries) :base()
		{
			this.breweries = breweries;
			this.context = context;
		}

		public override Brewery this[int position]
		{
			get
			{
				return breweries[position];
			}
		}

		public override int Count
		{
			get
			{
				return breweries.Count;
			}
		}

		public override long GetItemId(int position)
		{
			return breweries[position].ID;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var brewery = breweries[position];

			if (convertView == null)
			{
				convertView = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
			}
			convertView.FindViewById<TextView>(Android.Resource.Id.Text1).Text = brewery.ToString();
			return convertView;
		}
	}
}
