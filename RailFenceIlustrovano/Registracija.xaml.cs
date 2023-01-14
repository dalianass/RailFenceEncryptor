using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace RailFenceIlustrovano
{
    /// <summary>
    /// Interaction logic for Registracija.xaml
    /// </summary>
    public partial class Registracija : Page
    {
        static HttpClient client = new HttpClient();

        public Registracija()
        {
            InitializeComponent();
        }

        public class RegisterData
        {
            public string UserName { get; set; }

            public string UserSurname { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }

        }

        static async Task RegisterUser(string username, string usersurname, string email, string pass, string putanja)
        {
            RegisterData registerData = new RegisterData()
            {
                UserName = username,
                UserSurname = usersurname,
                Email = email,
                Password = pass
            };

            string json = JsonConvert.SerializeObject(registerData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            if (username.Length != 0 && username.Length != 0 && email.Length != 0 && pass.Length != 0 && pass.Length >= 8)
            {
                try
                {
                    var response = await client.PostAsync(putanja, content);
                    MessageBox.Show("Uspesno ste se registrovali");
                }
                catch (Exception)
                {
                    MessageBox.Show("Neuspesno registrovanje. Pokusajte ponovo.");
                }
            }
            else MessageBox.Show("Molimo vas, popunite sva polja!");

        }

        private async void RegistracijaBtn_Click(object sender, RoutedEventArgs e)
        {
            var ime = imeTxt.Text;
            var prezime = prezimeTxt.Text;
            var email = emailTxt.Text;
            var password = passwordTxt.Password;
            await RegisterUser(ime, prezime, email, password, "http://localhost:50078/api/Account/register");

            this.NavigationService.Navigate(new Login());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Login());
        }
    }
}
