using System;
using SQLite;

namespace beer_me
{
	public class Brewery
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		private string Name { get; set; }

		private string Address { get; set; }

		private string City { get; set; }

		private string Phone { get; set; }

		private int Lat { get; set; } 

		private int Long { get; set; }

		public override string ToString()
		{
			return string.Format("[Person: ID={0}, Name={1}, Address={2}]", ID, Name, Address);
		}
	}
}
