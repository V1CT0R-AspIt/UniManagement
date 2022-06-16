using System;

namespace Entities
{
	public class ContactInformation
	{
		private int id;
		private string phoneNumber;
		private string mail;

		public ContactInformation()
		{

		}

		public ContactInformation(int id, string phoneNumber, string mail)
		{
			Id = id;
			PhoneNumber = phoneNumber;
			Mail = mail;
		}

		public int Id
		{
			get => id;
			set
			{
				if(value < 0)
				{
					throw new ArgumentOutOfRangeException();
				}
				id = value;
			}
		}
		public string PhoneNumber
		{
			get => phoneNumber;
			set
			{
				phoneNumber = value;
			}
		}
		public string Mail
		{
			get => mail;
			set
			{
				mail = value;
			}
		}
	}
}
