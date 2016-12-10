using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Android.App;
using Android.Views;
using Android.Widget;

namespace beer_me
{
	public class ClosestBreweriesListAdapter:BaseAdapter<Brewery>
	{

		List<Brewery> breweries; 
		Activity context;

		public ClosestBreweriesListAdapter( Activity context, List<Brewery> breweries) :base()
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
			return position;
		}

		public Brewery getBreweryAtPostition(int position)
		{
			return breweries[position];
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var brewery = breweries[position];

			if (convertView == null)
			{
				convertView = context.LayoutInflater.Inflate(Android.Resource.Layout.ActivityListItem, null);
			}

			var resourceId = (int)typeof(Resource.Drawable).GetField(brewery.getImage()).GetValue(null);

			convertView.FindViewById<TextView>(Android.Resource.Id.Text1).Text = brewery.getName() + " - Only " +  brewery.getTravelTime().Replace("\"", "") + " away!";
			convertView.FindViewById<ImageView>(Android.Resource.Id.Icon).SetImageResource(resourceId);

			ImageView image = convertView.FindViewById<ImageView>(Android.Resource.Id.Icon);

			convertView.SetPadding(100, 100, 100, 100);
			image.SetMinimumWidth(100);
			image.SetMinimumHeight(100);

			return convertView;
		}
	}
}
