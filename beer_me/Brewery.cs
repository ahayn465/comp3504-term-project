using System;
using SQLite;

namespace beer_me
{
	public class Brewery:TableBrewery
	{

		public Brewery(string id, string name, string description, string address, string city, string phone)
		{
			this.ID = id;
			this.Name = name;
			this.Description = description;
			this.Address = address;
			this.City = city;
			this.Phone = phone;
		}

		public override string ToString()
		{
			return string.Format("{0} - {1}", Name, City);
		}

		public string getName()
		{
			return this.Name;
		}

		public string getDescription()
		{
			return this.Description;
		}

		public string toString()
		{
			return this.getName() + " " + this.getDescription();
		}
	}
}
