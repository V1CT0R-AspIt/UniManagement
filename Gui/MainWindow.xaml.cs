using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataAccess;
using Entities;

namespace Gui
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Repository repo;

		public MainWindow()
		{
			InitializeComponent();
			try
			{
				//Initialize repo field:
				repo = new();
			}
			catch (Exception e)
			{

				MessageBox.Show($"Fejl under tilgang til data: {e.Message}", "Opstartsfejl", MessageBoxButton.OK, MessageBoxImage.Error);
				Close();
			}
			//Get all contact infos from database:
			List<ContactInformation> contactInformation = repo.GetAllContactInformation();
			List<Address> alladdresseswithpeople = repo.GetAllAddresses();
			List<Person> allpersons = repo.GetAllPersons();

			//Load infos into the listbox:
			PersonList.ItemsSource = allpersons;
			ContactList.ItemsSource = contactInformation;
			AddressList.ItemsSource = alladdresseswithpeople;
		}

		private void Add_New(object sender, RoutedEventArgs e)
		{
			//Mock input data:
			string fname = FNameBox.Text;
			string lname = LNameBox.Text;
			string mail = MailBox.Text;
			string phonenr = PhoneBox.Text;
			string stname = StNameBox.Text;
			string stnr = StNrBox.Text;
			string zip = ZipBox.Text;
			string city = CityBox.Text;
			string country = CountryBox.Text;
			//Make object to send to repository:
			Person newperson = new()
			{
				FirstName = fname,
				LastName = lname
			};
			ContactInformation newcontact = new()
			{
				Mail = mail,
				PhoneNumber = phonenr
			};
			Address newaddress = new()
			{
				StreetName = stname,
				StreetNumber = stnr,
				Zip = zip,
				City = city,
				Country = country
			};
			//Call the repository:
			repo.AddNewPerson(newperson);
			repo.AddNewContactInformation(newcontact);
			repo.AddNewAddress(newaddress);

			PersonList.ItemsSource = null;
			ContactList.ItemsSource = null;
			AddressList.ItemsSource = null;
			PersonList.ItemsSource = repo.GetAllPersons();
			ContactList.ItemsSource = repo.GetAllContactInformation();
			AddressList.ItemsSource = repo.GetAllAddresses();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}