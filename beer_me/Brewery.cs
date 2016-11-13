using System;
using SQLite;

namespace beer_me
{
	public class Brewery
	{
		
		public int ID { get; set; }

		private string Name { get; set; }

		private string Address { get; set; }

		private string City { get; set; }

		private string Phone { get; set; }

		private int Lat { get; set; } 

		private int Long { get; set; }

		public Brewery(int id, string name, string address, string city, string phone, int lat, int longd)
		{
			this.ID = id;
			this.Name = name;
			this.Address = address;
			this.City = city;
			this.Phone = phone;
			this.Lat = lat;
			this.Long = longd;
		}

		public override string ToString()
		{
			return string.Format("[ID={0}, Name={1}, City={2}]", ID, Name, City);
		}
	}
}
