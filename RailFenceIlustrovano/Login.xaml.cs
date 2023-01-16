using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        static HttpClient client = new HttpClient();
        public static string podaciOKorisniku;
        static string role;
        
        public Login()
        {
            InitializeComponent();
            
        }

        public class LoginData
        {
            public string Email { get; set; }

            public string Password { get; set; }
        }

    static async Task LoginUser(string email, string pass, string putanja)
        {
            LoginData loginData = new LoginData()
            {
                Email = email,
                Password = pass
            };

            string json = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            if (email.Length != 0 && pass.Length != 0 && pass.Length >= 8)
            {
                try
                {
                    var response = await client.PostAsync(putanja, content);
                    if (response.IsSuccessStatusCode)
                    {
                        podaciOKorisniku = await response.Content.ReadAsStringAsync();
                    }

                    JObject jsonPodaci = JObject.Parse(Login.podaciOKorisniku);
                    string token = (string)jsonPodaci["token"];

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    //desifrovanje vrednosti claimova iz tokena
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(token);
                    var tokenS = handler.ReadToken(token) as JwtSecurityToken;
                    var claims = tokenS.Claims;

                    var myId = int.Parse(claims.FirstOrDefault(x => x.Type == "nameid")?.Value);
                    role = claims.FirstOrDefault(x => x.Type == "role")?.Value;
                    var username = claims.FirstOrDefault(x => x.Type == "uniquename")?.Value;
                }
                catch (Exception)
                {
                    MessageBox.Show("Neuspesan zahtev za logovanje. Pokusajte ponovo.");
                }
            }
            else MessageBox.Show("Molimo vas, popunite sva polja!");

        }


        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            var email = emailTxt.Text;
            var password = passwordTxt.Password;

            await LoginUser(email, password, "http://localhost:50078/api/Account/login");

            //this.NavigationService.Navigate(new Iscrtavanje());
            if (role == "Korisnik")
            {
                this.NavigationService.Navigate(new Iscrtavanje());
            }
            else if (role == "Admin")
            {
                this.NavigationService.Navigate(new AdminPage());

            }
            else MessageBox.Show("Neuspesno logovanje. Uloga nije u redu.");
        }
    }
}
