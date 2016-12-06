using System;
using System.Json;
using SQLite;

namespace beer_me
{
	public class Brewery
	{
		
		private String ID;
		private String Name;
		private String Description;
		private String Address;
		private String City;
		private String Phone;
		private String Image;
		private String Lat;
		private String Long;
		//private String Instagram = "";
		//private String Facebook = "";
		//private String Twitter = "";
		private String PlaceId;

		public Brewery( JsonValue newBrewery )
		{
			this.ID = newBrewery["_id"];
			this.Name = newBrewery["name"];
			this.Description = newBrewery["description"];
			this.Address = newBrewery["address"];
			this.City = newBrewery["city"];
			this.Phone = newBrewery["phone"];
			this.Image = newBrewery["image"];
			this.Lat = newBrewery["latd"];
			this.Long = newBrewery["longd"];

			if (newBrewery["placeId"].Count > 0)
				this.PlaceId = newBrewery["placeId"][0];

		}

		public override string ToString()
		{
			return string.Format("{0}", Name);
		}

		public string getId()
		{
			return this.ID;
		}

		public string getName()
		{
			return this.Name;
		}

		public string getLat()
		{
			return this.Lat;
		}

		public string getLong()
		{
			return this.Long;
		}

		public string getDescription()
		{
			return this.Description;
		}

		public string getImage()
		{
			return this.Image;	
		}

		public string toString()
		{
			return this.getName() + " " + this.getDescription();
		}
	}
}
