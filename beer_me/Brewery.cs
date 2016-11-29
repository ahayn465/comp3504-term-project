using System;
using SQLite;

namespace beer_me
{
	public class Brewery
	{
		private string ID;
		private String Name;
		private String Description;
		private String Address;
		private String City;
		private String Phone;
		private String Image;
		private String Lat;
		private String Long;

		public Brewery(string id, string name, string description, string address, string city, string phone, string image, string latd, string longd)
		{
			Console.WriteLine("Constructing", name);

			this.ID = id;
			this.Name = name;
			this.Description = description;
			this.Address = address;
			this.City = city;
			this.Phone = phone;
			this.Image = image;
			this.Lat = latd;
			this.Long = longd;
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
