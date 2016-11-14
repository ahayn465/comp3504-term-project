using System;
using SQLite;

namespace beer_me
{
	public class TableBrewery
	{
		[PrimaryKey, AutoIncrement]
		public string ID { get; set; }

		private string Name { get; set; }

		private string Description { get; set; }

		private string Address { get; set; }

		private string City { get; set; }

		private string Phone { get; set; }

		public override string ToString()
		{
			return string.Format("[Person: ID={0}, Name={1}, Address={2}]", ID, Name, Address);
		}
	}
}
