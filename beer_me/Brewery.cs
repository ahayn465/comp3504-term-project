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

		public Brewery(string id, string name, string description, string address, string city, string phone, string image)
		{
			this.ID = id;
			this.Name = name;
			this.Description = description;
			this.Address = address;
			this.City = city;
			this.Phone = phone;
			this.Image = image;
		}

		public override string ToString()
		{
			return string.Format("{0} - {1}", Name, City);
		}

		public string getId()
		{
			return this.ID;
		}

		public string getName()
		{
			return this.Name;
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
