using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DataAccess;
using Entities;

namespace Tests
{
	public class DataAccessTests
	{
		[Fact]
		public void ConnectionToDB()
		{
			//Arrange:
			Repository repository;

			//Act:
			repository = new();

			//Assert:
			Assert.NotNull(repository);
		}

		[Fact]
		public void GetAllPersonsDataTest()
		{
			//Arrange:
			Repository repo = new();

			//Act:
			List<Person> people = repo.GetAllPersons();

			//Assert:
			Assert.True(people.Count > 0);
		}
	}
}
