using System;
using SQLite;

namespace beer_me
{
	public class Brewery
	{
		
		public string ID { get; set; }

		private string Name { get; set; }

		private string Description { get; set; }

		private string Address { get; set; }

		private string City { get; set; }

		private string Phone { get; set; }

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
