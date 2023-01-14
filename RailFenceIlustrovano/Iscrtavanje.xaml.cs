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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace RailFenceIlustrovano
{
    public partial class Iscrtavanje : Page
    {
        static HttpClient client = new HttpClient();
        int[] pozicijeSlovaUKoloniSif;
        int[] pozicijeSlovaUVrstiSif;

        int[] pozicijeSlovaUKoloniDesif;
        int[] pozicijeSlovaUVrstiDesif;

        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer timer2 = new DispatcherTimer();
        static int brojacSif = 0;
        static int brojacDes = 0;

        static int myId;
        Grid grid;
        Random r = new Random();

        public Iscrtavanje()
        {
            InitializeComponent();

            JObject jsonPodaci = JObject.Parse(Login.podaciOKorisniku);
            string token = (string)jsonPodaci["token"];

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //desifrovanje vrednosti claimova iz tokena
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;
            var claims = tokenS.Claims;

            myId = int.Parse(claims.FirstOrDefault(x => x.Type == "nameid")?.Value);
            var role = claims.FirstOrDefault(x => x.Type == "role")?.Value;
            var username = claims.FirstOrDefault(x => x.Type == "uniquename")?.Value;
        }

        Brush plava = new SolidColorBrush(Color.FromRgb(189, 224, 254));
        Brush siva = new SolidColorBrush(Color.FromRgb(155, 200, 200));

        //*********************************RAD SA APIJEM*********************************************************
        public class Sifrovanje
        {
            public string RecZaSifrovanje { get; set; }

            public int Dubina { get; set; }

            public string SifrovanaRec { get; set; }

            public int? AppUserId { get; set; }

        }

        public class Desifrovanje
        {
            public string RecZaDesifrovanje { get; set; }

            public int Dubina { get; set; }

            public string DesifrovanaRec { get; set; }

            public int? AppUserId { get; set; }

        }

        static async Task<List<Sifrovanje>> GetMojaSifrovanja(int id, string putanja)
        {
            var jsonString = "";
            HttpResponseMessage response = await client.GetAsync(putanja + "?id=" + id);
            if (response.IsSuccessStatusCode)
            {
                jsonString = await response.Content.ReadAsStringAsync();
            }
            else MessageBox.Show("Preuzimanje podataka iz baze o vasim sifrovanjima nije uspelo.");
            var sifrovanje = JsonConvert.DeserializeObject<List<Sifrovanje>>(jsonString);

            return sifrovanje;
        }

        static async Task<List<Desifrovanje>> GetMojaDesifrovanja(int id, string putanja)
        {
            var jsonString = "";
            HttpResponseMessage response = await client.GetAsync(putanja + "?id=" + id);
            if (response.IsSuccessStatusCode)
            {
                jsonString = await response.Content.ReadAsStringAsync();
            }
            else MessageBox.Show("Preuzimanje podataka iz baze o vasim desifrovanjima nije uspelo.");
            var desifrovanje = JsonConvert.DeserializeObject<List<Desifrovanje>>(jsonString);

            return desifrovanje;
        }


        static async Task CreateSifrovanje(int dubina, string recZaSifrovanje,
            string sifrovanaRec, string putanja)
        {
            Sifrovanje sifrovanje = new Sifrovanje()
            {
                RecZaSifrovanje = recZaSifrovanje,
                Dubina = dubina,
                SifrovanaRec = sifrovanaRec,
                AppUserId = 1 //hardkodirano, ali na backendu uzima vrednost trenutnog usera
            };

            string json = JsonConvert.SerializeObject(sifrovanje);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(putanja, content);
                MessageBox.Show("Uspesno dodato sifrovanje");
            }
            catch (Exception)
            {
                MessageBox.Show("Dodavanje sifrovanja u bazu nije uspelo. Pokusajte ponovo.");
            }

        }

        static async Task CreateDesifrovanje(int dubina, string recZaDesifrovanje,
            string desifrovanaRec, string putanja)
        {
            Desifrovanje desifrovanje = new Desifrovanje()
            {
                RecZaDesifrovanje = recZaDesifrovanje,
                Dubina = dubina,
                DesifrovanaRec = desifrovanaRec,
                AppUserId = 1 //hardkodirano, ali na backendu uzima vrednost trenutnog usera
            };

            string json = JsonConvert.SerializeObject(desifrovanje);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(putanja, content);
                MessageBox.Show("Uspesno dodato desifrovanje");
            }
            catch (Exception)
            {
                MessageBox.Show("Dodavanje desifrovanja u bazu nije uspelo. Pokusajte ponovo.");
            }
        }

        //****************************KRAJ SEKCIJE RAD SA API-JEM**************************************

        private Grid IscrtajMatricu(int rows, int cols)
        {
            ScrollViewer scroll = new ScrollViewer()
            {
                Height = 380,
            };

            grid = new Grid()
            {
                Height = 380,
                Width = 1800
            };

            for (int i = 0; i < rows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

                for (int j = 0; j < cols; j++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                }
            }
            scroll.Content = grid;
            DonjiStek.Children.Add(scroll);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Rectangle rect = new Rectangle()
                    {
                        Width = 150,
                        Height = 100,
                        Fill = plava,
                        Margin = new Thickness(10)
                    };
                    Grid.SetRow(rect, i);
                    Grid.SetColumn(rect, j);
                    grid.Children.Add(rect);
                }
            }

            return grid;
        }

        //Izvrsavanje sifrovanja
        private async void SifrirajBtn_Click(object sender, RoutedEventArgs e)
        {
            //ako je vec bilo nekog sifrovanja (koje se odvija unutar stackPanela DonjiStek), obrisi taj sadrzaj
            if (DonjiStek.Children.Count > 0)
            {
                foreach (UIElement control in DonjiStek.Children)
                {
                    DonjiStek.Children.Remove(control);
                    break;
                }
                //ako je bilo desifrovanja, pa predjemo na sifrovanje, brise tekst iz texbox-ova
                recZaDesifrovanjeTxt.Text = "";
                dubina2Txt.Text = "";
                desifrovanaRecTxt.Visibility = Visibility.Hidden;
                desifrovanaRecLabel.Visibility = Visibility.Hidden;
            }
            int dubinaSifrovanja = int.Parse(dubinaTxt.Text);
            string recZaSifrovanje = recZaSifrovanjeTxt.Text;

            if (recZaSifrovanje.Length != 0 && dubinaSifrovanja > 1)
            {
                string sifrovanaRec = Sifriraj(recZaSifrovanje, dubinaSifrovanja);
                //prikazuje textBox koji je bio hidden i unosi sifrovanu rec
                sifrovanaRecTxt.Text = sifrovanaRec;
                sifrovanaRecTxt.Visibility = Visibility.Visible;
                sifrovanaRecLabel.Visibility = Visibility.Visible;
                await CreateSifrovanje(dubinaSifrovanja, recZaSifrovanje, sifrovanaRec, "http://localhost:50078/api/Sifrovanje/add-sifrovanje");
            }
            else
            {
                MessageBox.Show("Molim vas, unesite obe vrednosti. Dubina mora biti veca od 1.");
            }

            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
            
        }

        //Izvrsavanje desifrovanja
        private async void DesifrirajBtn_Click(object sender, RoutedEventArgs e)
        {
            //ako je vec bilo nekog sifrovanja (koje se odvija unutar stackPanela DonjiStek), obrisi taj sadrzaj
            if (DonjiStek.Children.Count > 0)
            {
                foreach (UIElement control in DonjiStek.Children)
                {
                    DonjiStek.Children.Remove(control);
                    break;
                }
                //ako je bilo sifrovanja, pa predjemo na desifrovanje, brise tekst iz texbox-ova
                recZaSifrovanjeTxt.Text = "";
                dubinaTxt.Text = "";
                sifrovanaRecTxt.Visibility = Visibility.Hidden;
                sifrovanaRecLabel.Visibility = Visibility.Hidden;
            }

            int dubinaDesifrovanja = int.Parse(dubina2Txt.Text);
            string recZaDesifrovanje = recZaDesifrovanjeTxt.Text;

            if (recZaDesifrovanje.Length != 0 && dubinaDesifrovanja > 1)
            {
                string desifrovanaRec = Desifriraj(recZaDesifrovanje, dubinaDesifrovanja);

                //prikazuje textBox koji je bio hidden i unosi sifrovanu rec
                desifrovanaRecTxt.Text = desifrovanaRec;
                desifrovanaRecTxt.Visibility = Visibility.Visible;
                desifrovanaRecLabel.Visibility = Visibility.Visible;

                await CreateDesifrovanje(dubinaDesifrovanja, recZaDesifrovanje, desifrovanaRec, "http://localhost:50078/api/Desifrovanje/add-desifrovanje");
            }
            else MessageBox.Show("Molim vas, unesite obe vrednosti. Dubina mora biti veca od 1.");
        }

        //Odjavljivanje
        private void Odjava_Click(object sender, RoutedEventArgs e)
        {
            if (client.DefaultRequestHeaders.Authorization != null)
            {
                client.DefaultRequestHeaders.Remove("Authorization");
                client.DefaultRequestHeaders.Authorization = null;
                client.Dispose();
                this.NavigationService.Navigate(new Login());
            }
            else
            {
                MessageBox.Show("Odjavljivanje neuspesno.");
            }
        }


        private async void PrikaziSifrovanjaBtn_Click(object sender, RoutedEventArgs e)
        {
            DataGridDesifrovanja.Visibility = System.Windows.Visibility.Hidden;
            DataGridSifrovanja.Visibility = System.Windows.Visibility.Visible;
            List<Sifrovanje> mojaSifrovanja = await GetMojaSifrovanja(myId, "http://localhost:50078/api/Sifrovanje/moja-sifrovanja");
            foreach (var item in mojaSifrovanja)
            {
                DataGridSifrovanja.Items.Add(item);
            }
        }

        private async void PrikaziDesifrovanjaBtn_Click(object sender, RoutedEventArgs e)
        {
            DataGridSifrovanja.Visibility = System.Windows.Visibility.Hidden;
            DataGridDesifrovanja.Visibility = System.Windows.Visibility.Visible;
            List<Desifrovanje> mojaDesifrovanja = await GetMojaDesifrovanja(myId, "http://localhost:50078/api/Desifrovanje/moja-desifrovanja");
            foreach (var item in mojaDesifrovanja)
            {
                DataGridDesifrovanja.Items.Add(item);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var customColor = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 255)));
            //var customColor = new SolidColorBrush(Color.FromRgb(95, 232, 193));

            var elem = grid.Children
              .Cast<Rectangle>()
              .First(rr => Grid.GetRow(rr) == pozicijeSlovaUVrstiDesif[brojacSif] && Grid.GetColumn(rr) == pozicijeSlovaUKoloniDesif[brojacSif]);
            elem.Fill = customColor;

            brojacSif++;
            if (brojacSif == pozicijeSlovaUKoloniDesif.Length)
            {
                timer.Stop();
                brojacSif = 0;
            }
        }

        private void Timer_Tick_Des(object sender, EventArgs e)
        {

            var customColor = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 255)));
            //var customColor = new SolidColorBrush(Color.FromRgb(95, 232, 193));

            var elem = grid.Children
              .Cast<Rectangle>()
              .First(rr => Grid.GetRow(rr) == pozicijeSlovaUVrstiSif[brojacDes] && Grid.GetColumn(rr) == pozicijeSlovaUKoloniSif[brojacDes]);
            elem.Fill = customColor;

            brojacDes++;
            if (brojacDes == pozicijeSlovaUKoloniSif.Length)
            {
                timer2.Stop();
                brojacDes = 0;
            }
        }

        //***************************************KOD ALGORITMA***********************************************

        private char[][] BuildCleanMatrix(int rows, int cols)
        {
            //pravi/alocira praznu matricu
            char[][] result = new char[rows][];
            for (int row = 0; row < result.Length; row++)
            {
                result[row] = new char[cols];
            }
            return result;
        }

        private string BuildStringFromMatrix(char[][] matrix)
        {
            //od unete matrice pravi string, tj. konacni tekst.
            string result = string.Empty;

            //matrix.Length kaze koliko ima redova
            //matric[row].Length kaze koliko je svaki red dug, tj. koliko ima kolona
            for (int row = 0; row < matrix.Length; row++)
            {
                for (int col = 0; col < matrix[row].Length; col++)
                {
                    if (matrix[row][col] != '\0')
                    {
                        //sve karaktere cuvamo unutar stringa result
                        result += matrix[row][col];
                    }
                }
            }

            return result;
        }

        private char[][] Transpose(char[][] matrix)
        {
            //prvo pozvali kolone, pa redove 
            char[][] result = BuildCleanMatrix(matrix[0].Length, matrix.Length);

            for (int row = 0; row < matrix.Length; row++)
            {
                for (int col = 0; col < matrix[row].Length; col++)
                {
                    result[col][row] = matrix[row][col];
                }
            }

            return result;
        }

        private string Sifriraj(string clearText, int key)
        {
            string result = string.Empty;
            //duzina matrice ista kao duzina teksta, jer u jednoj celiji/koloni, po jedno slovo
            char[][] matrix = BuildCleanMatrix(key, clearText.Length);

            int rowIncrement = 1;
            var grid = IscrtajMatricu(key, clearText.Length); //rows and cols

            DodajSlovaNaMatricu(matrix, clearText, grid);
            for (int row = 0, col = 0; col < matrix[row].Length; col++)
            {

                //ako se stigne do poslednjeg reda, mnozi sa -1 da bi redovi isli 0, 1, 2, 3 - 2, 1, 0
                // i ako bude -1, znaci da je na redu nulti red, tkd tu opet treba *-1 i ode na prvi red
                if (
                    row + rowIncrement == matrix.Length ||
                    row + rowIncrement == -1
                    )
                {
                    rowIncrement *= -1;
                }
                //redom ubacuje slovo po slovo iz plaintexta U MATRICU, 
                //i to redosledom (ako je length 3 ) 0,0 - 1,1 - 2, 2 --1, 3 - 0, 4 itd
                matrix[row][col] = clearText[col];
                row += rowIncrement;
            }
            result = BuildStringFromMatrix(matrix);
            pozicijeDes(clearText, matrix);
            return result;
        }

        private void DodajSlovaNaMatricu(char[][] matrix, string clearText, Grid grid)
        {
            int rowIncrement = 1;
            pozicijeSlovaUKoloniSif = new int[clearText.Length];
            pozicijeSlovaUVrstiSif = new int[clearText.Length];
            for (int row = 0, col = 0; col < matrix[row].Length; col++)
            {
                pozicijeSlovaUKoloniSif[col] = col;
                pozicijeSlovaUVrstiSif[col] = row;

                //ako stigne do poslednjeg reda, mnozi sa -1 da bi redovi isli 0, 1, 2, 3, pa - 2, 1, 0
                // a ako bude -1, znaci da je na redu nulti red, pa treba da se mnozi sa -1 da ode na prvi red
                if (
                    row + rowIncrement == matrix.Length ||
                    row + rowIncrement == -1
                    )
                {
                    rowIncrement *= -1;
                }
                //redom ubacuje slovo po slovo iz plaintexta u matricu,
                //i to redosledom (ako je length 3 ) 0,0 - 1,1 - 2, 2 --1, 3 - 0, 4 itd.
                TextBox tb = new TextBox()
                {
                    Text = (clearText[col]).ToString().ToUpper(),
                    Margin = new Thickness(10),
                    FontSize = 24,
                    Background= Brushes.Transparent,
                    Width = 150,
                    Height = 100
                };

                Grid.SetRow(tb, row);
                Grid.SetColumn(tb, col);
                grid.Children.Add(tb);
                row += rowIncrement;
            }
        }


        private string Desifriraj(string cipherText, int key)
        {
            //int br = 0;
            string result = string.Empty;

            char[][] matrix = BuildCleanMatrix(key, cipherText.Length);

            int rowIncrement = 1;
            int textIdx = 0;

            var grid = IscrtajMatricu(key, cipherText.Length); //rows and cols
            DodajSlovaNaMatricu(matrix, cipherText, grid);

            for (int selectedRow = 0; selectedRow < matrix.Length; selectedRow++)
            {
                for (int row = 0, col = 0; col < matrix[row].Length; col++)
                {
                    if (row + rowIncrement == matrix.Length ||
                        row + rowIncrement == -1)
                    {
                        rowIncrement *= -1;
                    }

                    //ako se poklapaju row i selectedRow -> hvata podatke iz tog celog reda i upisuje ih tim redom
                    //obezbedjuje da se prolazi samo kroz mesta gde imamo podatke u matrici
                    if (row == selectedRow)
                    {
                        matrix[row][col] = cipherText[textIdx++];
                    }
                    row += rowIncrement;
                }
            }

            matrix = Transpose(matrix);
            result = BuildStringFromMatrix(matrix);

            timer2.Interval = new TimeSpan(0, 0, 1);
            timer2.Tick += Timer_Tick_Des;
            timer2.Start();

            return result;
        }

        private void pozicijeDes(string cipherText, char[][] matrix)
        {
            int br = 0;
            int textIdx = 0;
            int rowIncrement = 1;
            pozicijeSlovaUKoloniDesif = new int [matrix[0].Length];
            pozicijeSlovaUVrstiDesif = new int [matrix[0].Length];
            for (int selectedRow = 0; selectedRow < matrix.Length; selectedRow++)
            {
                for (int row = 0, col = 0; col < matrix[row].Length; col++)
                {
                    if (row + rowIncrement == matrix.Length ||
                        row + rowIncrement == -1)
                    {
                        rowIncrement *= -1;
                    }

                    //ako se poklapaju row i selectedRow - hvata podatke iz tog celog reda i upisuje ih tim redom
                    //obezbedjuje da se prolazi samo kroz mesta gde imamo podatke u matrici
                    if (row == selectedRow)
                    {
                        matrix[row][col] = cipherText[textIdx++];

                        if (br < matrix[0].Length)
                        {
                            pozicijeSlovaUKoloniDesif[br] = col;
                            pozicijeSlovaUVrstiDesif[br] = row;
                            br++;
                        }
                    }

                    row += rowIncrement;
                }
            }
        }
    }
}

