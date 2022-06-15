using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Entities;

namespace DataAccess
{
	public class Repository
	{
		private readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=UniDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
		public Repository()
		{
			//Test if application can contact database server:
			SqlConnection connection = new(connectionString);
			connection.Open();
			connection.Close();
		}
		public List<Person> GetAllPersons()
		{
			// First, get all the contact infos, because they are aggregated from person:
			List<ContactInformation> contactInformations = GetAllContactInformation();

			// Make the list that the method returns:
			List<Person> persons = new();

			// Make a connection to the DB and open it:
			SqlConnection connection = new(connectionString);
			connection.Open();

			// Make the SQl query:
			string sql = "SELECT * FROM Persons";

			// Make the command object:
			SqlCommand command = new(sql, connection);

			// Execute query and save the returned data in a variable:
			SqlDataReader reader = command.ExecuteReader();

			// Convert reader data to C# objects. For each row in the reader:
			while (reader.Read())
			{
				int id = (int)reader[0];
				string firstname = (string)reader[1];
				string lastname = (string)reader[2];

				// This is the contactinfo FK
				int contactInformation_FKid = (int)reader[3];
				// This is the address FK
				int address_FKid = (int)reader[4];

				// The aggregated contact information object. Initialize to null:
				ContactInformation contactInformation = null;

				// Loop through all the contact information objects in the contact informations list, that we got before from the database:
				for (int i = 0; i < contactInformations.Count; i++)
				{
					// If there is a match in the retrieved person row's FK value,
					if (contactInformation_FKid == contactInformations[i].Id)
					{
						// then assign the object from the list, to the property on the person object, thereby making the OOP aggregation:
						contactInformation = contactInformations[i];

						// Break out of the loop, because there' no reason to continue:
						break;
					}
				}

				// Assign all the retrieved values to the person object:
				Person p = new()
				{
					Id = id,
					FirstName = firstname,
					LastName = lastname,
					Contactinformation = contactInformation,
					AddressFK = address_FKid
				};

				// Add the retrieved person to the list of persons:
				persons.Add(p);
			}

			// Return the list of Persons:
			return persons;
		}
		public List<ContactInformation> GetAllContactInformation()
		{
			//Make the list that the method returns:
			List<ContactInformation> contactInformations = new();

			//Make a connection to the DB and open it:
			SqlConnection connection = new(connectionString);
			connection.Open();

			//Make the SQL query:
			string sql = "SELECT * FROM ContactInformations";

			//Make the command object:
			SqlCommand command = new(sql, connection);

			//Execute query and save the returned data in a variable
			SqlDataReader reader = command.ExecuteReader();

			//Convert reader data to C# objects. for each row in the reader:
			while (reader.Read())
			{
				//Extract database from the reader to C# variables:
				int id = (int)reader[0];
				string phone = (string)reader[1];
				string mail = (string)reader[2];

				//Create a new object:
				ContactInformation ci = new()
				{
					Id = id,
					PhoneNumber = phone,
					Mail = mail
				};

				//Add to the list:
				contactInformations.Add(ci);
			}

			//Allways remember to close the connection:
			connection.Close();

			//Return the list of contact infos, if any:
			return contactInformations;
		}
		public List<Address> GetAllAddresses()
		{
			//Make the list that the method returns:
			List<Address> addresses = new();
			List<Person> persons = GetAllPersons();

			//Make a connection to the DB and open it:
			SqlConnection connection = new(connectionString);
			connection.Open();

			//Make the SQL query:
			string sql = "SELECT * FROM Addresses";

			//Make the command object:
			SqlCommand command = new(sql, connection);

			//Execute query and save the returned data in a variable
			SqlDataReader reader = command.ExecuteReader();

			//Convert reader data to C# objects. for each row in the reader:
			while (reader.Read())
			{
				//Extract database from the reader to C# variables:
				int addressId = (int)reader[0];
				string streetName = (string)reader[1];
				string streetNumber = (string)reader[2];
				string zip = (string)reader[3];
				string city = (string)reader[4];
				string country = (string)reader[5];

				//Create a new object:
				Address adr = new()
				{
					Id = addressId,
					StreetName = streetName,
					StreetNumber = streetNumber,
					Zip = zip,
					City = city,
					Country = country,
					People = new()
				};

				//Add to the list:
				addresses.Add(adr);
			}

			//Allways remember to close the connection:
			connection.Close();

			Aggregate(persons, addresses);

			//Return the list of contact infos, if any:
			return addresses;
		}
		private void Aggregate(List<Person> persons, List<Address> addresses)
		{
			//Loop over all persons:
			for (int i = 0; i < persons.Count; i++)
			{
				//Check whether or not the person has an address:
				if(persons[i].AddressFK != 0)
				{
					//If true, then loop over all addresses:
					for (int j = 0; j < addresses.Count; j++)
					{
						//Find the right match between person and address.
						//NOTE: there could be more than 1 persons living at the
						//address, therefore we shall not break out of the inner loop:
						if(persons[i].AddressFK == addresses[j].Id)
						{
							//Add the person to the list of people living at the address:
							addresses[j].People.Add(persons[i]);
						}
					}
				}
			}
		}

		public void AddNewPerson(Person personToAdd)
		{
			//Make the list that the method returns:
			List<Person> Persons = new();

			//Make a connection to the DB and open it:
			SqlConnection connection = new(connectionString);
			connection.Open();

			//Make the SQL query:
			string sql = $"INSERT INTO Persons (FirstName, LastName) VALUES('{personToAdd.FirstName}', '{personToAdd.LastName}')";

			//Make the command object:
			SqlCommand command = new(sql, connection);

			//Execute the command:
			command.ExecuteNonQuery();

			//Close the connection:
			connection.Close();
		}
		public void AddNewContactInformation(ContactInformation contactInformationToAdd)
		{
			//Make the list that the method returns:
			List<ContactInformation> contactInformations = new();

			//Make a connection to the DB and open it:
			SqlConnection connection = new(connectionString);
			connection.Open();

			//Make the SQL query:
			string sql = $"INSERT INTO ContactInformations (Mail, PhoneNumber) VALUES('{contactInformationToAdd.Mail}', '{contactInformationToAdd.PhoneNumber}')";

			//Make the command object:
			SqlCommand command = new(sql, connection);

			//Execute the command:
			command.ExecuteNonQuery();

			//Close the connection:
			connection.Close();
		}
		public void AddNewAddress(Address addressToAdd)
		{
			//Make the list that the method returns:
			List<Address> addresses = new();

			//Make a connection to the DB and open it:
			SqlConnection connection = new(connectionString);
			connection.Open();

			//Make the SQL query:
			string sql = $"INSERT INTO ContactInformations (StreetName, StreetNumber, Zip, City, Country) VALUES('{addressToAdd.StreetName}', '{addressToAdd.StreetNumber}', '{addressToAdd.Zip}', '{addressToAdd.City}', '{addressToAdd.Country}')";

			//Make the command object:
			SqlCommand command = new(sql, connection);

			//Execute the command:
			command.ExecuteNonQuery();

			//Close the connection:
			connection.Close();
		}
	}
}
