using System;
using SQLite;

namespace beer_me
{
	public class Brewery
	{
		
		public string ID { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string Address { get; set; }

		public string City { get; set; }

		public string Phone { get; set; }

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
	}
}
