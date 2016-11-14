using System;
using SQLite;

namespace beer_me
{
	public class TableBrewery
	{
		[PrimaryKey, AutoIncrement]
		public string ID { get; set; }

		public string BreweryId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string Address { get; set; }

		public string City { get; set; }

		public string Phone { get; set; }

		public override string ToString()
		{
			return string.Format("[Person: ID={0}, Name={1}, Address={2}]", ID, Name, Address);
		}
	}
}
