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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new Registracija());
        }

        //    static HttpClient client = new HttpClient();

        //    public class Sifrovanje
        //    {
        //        public string RecZaSifrovanje { get; set; }

        //        public int Dubina { get; set; }

        //        public string SifrovanaRec { get; set; }

        //        public int? AppUserId { get; set; }

        //    }

        //    Brush roze = new SolidColorBrush(Color.FromRgb(255, 200, 200));
        //    Brush siva = new SolidColorBrush(Color.FromRgb(155, 200, 200));
        //    Random r = new Random();
        //    Brush customColor;

        //    //private async void Button_Click(object sender, RoutedEventArgs e)
        //    //{
        //    //    //List<Sifrovanje> proba = await GetSifrovanjeFromApi("http://localhost:50078/api/Sifrovanje/sva-sifrovanja");
        //    //    //MessageBox.Show(proba[0].Dubina.ToString());
        //    //    //await CreateSifrovanje("http://localhost:50078/api/Sifrovanje/add-sifrovanje");
        //    //}

        //    static async Task<List<Sifrovanje>> GetSifrovanjaFromApi(string putanja)
        //    {
        //        var jsonString = "";
        //        HttpResponseMessage response = await client.GetAsync(putanja);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            jsonString = await response.Content.ReadAsStringAsync();
        //        }
        //        else MessageBox.Show("Nesto nije u redu.");
        //        var sifrovanje = JsonConvert.DeserializeObject<List<Sifrovanje>>(jsonString);

        //        return sifrovanje;
        //    }

        //    static async Task CreateSifrovanje(int dubina, string recZaSifrovanje, 
        //        string sifrovanaRec, string putanja)
        //    {
        //        Sifrovanje sifrovanje = new Sifrovanje()
        //        {
        //            RecZaSifrovanje = recZaSifrovanje,
        //            Dubina = dubina,
        //            SifrovanaRec = sifrovanaRec,
        //            //AppUserId = 1
        //        };

        //        string json = JsonConvert.SerializeObject(sifrovanje);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");
        //        var response = await client.PostAsync(putanja, content);
        //        MessageBox.Show("RADI");
        //    }




        //    private Grid IscrtajMatricu(int rows, int cols)
        //    {
        //        ScrollViewer scroll = new ScrollViewer() 
        //        { 
        //            Height = 500
        //        };

        //        var grid = new Grid()
        //        {
        //            Background = siva,
        //        };

        //        for (int i = 0; i < rows; i++)
        //        {
        //            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

        //            for (int j = 0; j < cols; j++)
        //            {
        //                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
        //            }
        //        }
        //        scroll.Content = grid;
        //        DonjiStek.Children.Add(scroll);

        //        for(int i = 0; i<rows; i++)
        //        {
        //            for(int j = 0; j<cols; j++)
        //            {
        //                Rectangle rect = new Rectangle()
        //                {
        //                    MinWidth = 100,
        //                    MinHeight = 100,
        //                    Fill = roze,
        //                    Margin = new Thickness(10)
        //                };
        //                Grid.SetRow(rect, i);
        //                Grid.SetColumn(rect, j);
        //                grid.Children.Add(rect);
        //            }
        //        }

        //        return grid;
        //    }

        //    //Izvrsavanje sifrovanja
        //    private async void SifrirajBtn_Click(object sender, RoutedEventArgs e)
        //    {
        //        //ako je vec bilo nekog sifrovanja (koje se odvija unutar stackPanela DonjiStek), obrisi taj sadrzaj
        //        if(DonjiStek.Children.Count > 0)
        //        {
        //            foreach (UIElement control in DonjiStek.Children)
        //            {
        //                DonjiStek.Children.Remove(control);
        //                break;
        //            }
        //        }
        //        int dubina = int.Parse(dubinaTxt.Text);
        //        string recZaSifrovanje = recZaSifrovanjeTxt.Text;

        //        string sifrovanaRec = Sifriraj(recZaSifrovanje, dubina);

        //        //prikazuje textBox koji je bio hidden i unosi sifrovanu rec
        //        sifrovanaRecTxt.Text = sifrovanaRec;
        //        sifrovanaRecTxt.Visibility = System.Windows.Visibility.Visible;
        //        sifrovanaRecLabel.Visibility = System.Windows.Visibility.Visible;

        //        await CreateSifrovanje(dubina, recZaSifrovanje, sifrovanaRec, "http://localhost:50078/api/Sifrovanje/add-sifrovanje");
        //    }

        //    //Izvrsavanje desifrovanja
        //    private void DesifrirajBtn_Click(object sender, RoutedEventArgs e)
        //    {
        //        int dubina = int.Parse(dubina2Txt.Text);
        //        string recZaDesifrovanje = recZaDesifrovanjeTxt.Text;

        //        string desifrovanaRec = Desifriraj(recZaDesifrovanje, dubina);

        //        //prikazuje textBox koji je bio hidden i unosi sifrovanu rec
        //        desifrovanaRecTxt.Text = desifrovanaRec;
        //        desifrovanaRecTxt.Visibility = System.Windows.Visibility.Visible;
        //        desifrovanaRecLabel.Visibility = System.Windows.Visibility.Visible;
        //    }

        //    //****************************KOD ALGORITMA*********************************

        //    private char[][] BuildCleanMatrix(int rows, int cols)
        //    {
        //        //pravi/alocira praznu matricu
        //        char[][] result = new char[rows][];
        //        for (int row = 0; row < result.Length; row++)
        //        {
        //            result[row] = new char[cols];
        //        }
        //        return result;
        //    }

        //    private string BuildStringFromMatrix(char[][] matrix)
        //    {
        //        //od unete matrice pravi string, tj. konacni tekst.
        //        string result = string.Empty;

        //        //matrix.Length kaze koliko ima redova
        //        //matric[row].Length kaze koliko je svaki red dug, tj. koliko ima kolona
        //        for (int row = 0; row < matrix.Length; row++)
        //        {
        //            for (int col = 0; col < matrix[row].Length; col++)
        //            {
        //                if (matrix[row][col] != '\0')
        //                {
        //                    //sve karaktere cuvamo unutar stringa result
        //                    result += matrix[row][col];
        //                }
        //            }
        //        }

        //        return result;
        //    }

        //    private char[][] Transpose(char[][] matrix)
        //    {
        //        //ovde prvo pozvali kolone, pa redove (?)
        //        char[][] result = BuildCleanMatrix(matrix[0].Length, matrix.Length);

        //        for (int row = 0; row < matrix.Length; row++)
        //        {
        //            for (int col = 0; col < matrix[row].Length; col++)
        //            {
        //                result[col][row] = matrix[row][col];
        //            }
        //        }

        //        return result;
        //    }

        //    private string Sifriraj(string clearText, int key)
        //    {
        //        string result = string.Empty;
        //        //duzina matrice ista kao duzina teksta, jer u jednoj celiji/koloni, po jedno slovo
        //        char[][] matrix = BuildCleanMatrix(key, clearText.Length);

        //        int rowIncrement = 1;
        //        var grid = IscrtajMatricu(key, clearText.Length); //rows and cols

        //        for (int row = 0, col = 0; col < matrix[row].Length; col++)
        //        {
        //            DodajSlovaNaMatricu(matrix, clearText, grid);

        //            //ako se stigne do poslednjeg reda, mnozi sa -1 da bi redovi isli 0, 1, 2, 3 - 2, 1, 0
        //            // i ako bude -1, znaci da je na redu nulti red, tkd tu opet treba *-1 i ode na prvi red
        //            if (
        //                row + rowIncrement == matrix.Length ||
        //                row + rowIncrement == -1
        //                )
        //            {
        //                rowIncrement *= -1;
        //            }
        //            //redom ubacuje slovo po slovo iz plaintexta U MATRICU, 
        //            //i to redosledom (ako je length 3 ) 0,0 - 1,1 - 2, 2 --1, 3 - 0, 4 itd
        //            matrix[row][col] = clearText[col];
        //            row += rowIncrement;
        //        }
        //        result = BuildStringFromMatrix(matrix);

        //        return result;
        //    }

        //    private void DodajSlovaNaMatricu(char[][] matrix, string clearText, Grid grid)
        //    {
        //        int rowIncrement = 1;
        //        for (int row = 0, col = 0; col < matrix[row].Length; col++)
        //        {
        //            //ako se stigne do poslednjeg reda, mnozi sa -1 da bi redovi isli 0, 1, 2, 3 - 2, 1, 0
        //            // i ako bude -1, znaci da je na redu nulti red, tkd tu opet treba *-1 i ode na prvi red
        //            if (
        //                row + rowIncrement == matrix.Length ||
        //                row + rowIncrement == -1
        //                )
        //            {
        //                rowIncrement *= -1;
        //            }
        //            //redom ubacuje slovo po slovo iz plaintexta U MATRICU, i to redosledom (ako je length 3 ) 0,0 - 1,1 - 2, 2 --1, 3 - 0, 4 itd
        //            TextBox tb = new TextBox()
        //            {
        //                Text = (clearText[col]).ToString()
        //            };

        //            Grid.SetRow(tb, row);
        //            Grid.SetColumn(tb, col);
        //            grid.Children.Add(tb);
        //            row += rowIncrement;
        //        }
        //    }

        //    private string Desifriraj(string cipherText, int key)
        //    {
        //        string result = string.Empty;

        //        char[][] matrix = BuildCleanMatrix(key, cipherText.Length);

        //        int rowIncrement = 1;
        //        int textIdx = 0;

        //        for (int selectedRow = 0; selectedRow < matrix.Length; selectedRow++)
        //        {
        //            for (int row = 0, col = 0; col < matrix[row].Length; col++)
        //        {
        //            if (row + rowIncrement == matrix.Length ||
        //                row + rowIncrement == -1)
        //            {
        //                rowIncrement *= -1;
        //            }

        //                //ako se poklapaju row i selectedRow, tj* hvata podatke iz tog celog reda i upisuje ih tim redom
        //                //obezbedjuje da se prolazi samo kroz mesta gde imamo podatke u matrici
        //                if (row == selectedRow)
        //                {
        //                    //Console.WriteLine(matrix[row][col]);
        //                    matrix[row][col] = cipherText[textIdx++];
        //                }
        //                row += rowIncrement;
        //            }
        //        }

        //        matrix = Transpose(matrix);
        //        result = BuildStringFromMatrix(matrix);

        //        return result;
        //    }


        //}
    }
}
