using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Entities;

namespace Tests
{
	public class EntitiesTests
	{
		[Fact]
		public void CorrectInitialization_ContactInformation()
		{
			//Arrange:
			int id = 1;
			string mail = "";
			string phoneNumber = "";

			//Act:
			ContactInformation contactInformation = new(id, mail, phoneNumber);

			//Assert:
			Assert.True(contactInformation.Id > 0);
			Assert.Contains("@", contactInformation.Mail);
			Assert.EndsWith(".com", contactInformation.Mail);
			Assert.EndsWith(".dk", contactInformation.Mail);
			Assert.True(contactInformation.PhoneNumber.Length > 7);
			bool phoneContainsOnlyNumbers = true;
			for (int i = 0; i < contactInformation.PhoneNumber.Length; i++)
			{
				if (!Char.IsDigit(contactInformation.PhoneNumber[i]))
				{
					phoneContainsOnlyNumbers = false;
					break;
				}
			}
			Assert.True(phoneContainsOnlyNumbers);
		}


		[Fact]
		public void CorrectMutation_ContactInformation()
		{
			//Arrange:
			int id = 1;
			string mail = "vsyhler@gmail.com";
			string phoneNumber = "6942036";
			ContactInformation contactInformation = new(id, mail, phoneNumber);

			//Act:
			contactInformation.PhoneNumber = "65290675";

			//Assert:
			Assert.Equal("65290675", contactInformation.PhoneNumber);
		}
	}
}
