using System;
using System.Json;
using SQLite;

namespace beer_me
{
	public class Brewery : IComparable<Brewery>
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
		private String PlaceIdAddress;
		private string Distance = "";
		private string TravelTime = "";
		private string TravelTimeRaw = "";

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

			//if (newBrewery["placeIdAddress"] && newBrewery["placeIdAddress"].Count > 0)
			//	this.PlaceId = newBrewery["placeIdAddress"][0];
			

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

		public string getPlaceId()
		{
			if (this.PlaceId != "")
				return this.PlaceId;
			else return null;
		}

		public string getPlaceIdAddress()
		{
			if (this.PlaceIdAddress != "")
				return this.PlaceIdAddress;
			else return null;
		}

		public string getDistance()
		{
			return this.Distance;
		}

		public string getTravelTime()
		{
			return this.TravelTime;
		}

		public int getTravelTimeRaw()
		{
			return Int32.Parse(TravelTimeRaw);
		}


		public string getCity()
		{
			return City;
		}

		public void setTravelTime(string time)
		{
			Console.WriteLine(time);
			this.TravelTime = time;
		}

		public void setTravelTimeRaw(string time)
		{
			Console.WriteLine(time);
			this.TravelTimeRaw = time;
		}


		public void setDistance(string distance)
		{
			Console.WriteLine(distance);
			this.Distance = distance;
		}

		public void setPlaceIdAddress(string address)
		{
			Console.WriteLine(address);;
			this.PlaceIdAddress = address;
		}

		// IComparable interface

		public int CompareTo(Brewery other)
		{
			return Name.CompareTo(other.Name);
		}

	}
}
