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
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        static HttpClient client = new HttpClient();
        List<Sifrovanje> sifrovanja;
        List<Desifrovanje> desifrovanja;

        public AdminPage()
        {
            InitializeComponent();
            pozoviApi();


        }

        private async void pozoviApi()
        {
            sifrovanja = await GetSvaSifrovanja("http://localhost:50078/api/sifrovanje/sva-sifrovanja");
            //MessageBox.Show(res[0].Dubina.ToString());
            foreach (var item in sifrovanja)
            {
                DataGridAdminSifrovanja.Items.Add(item);
            }

            desifrovanja = await GetSvaDesifrovanja("http://localhost:50078/api/desifrovanje/sva-desifrovanja");
            //MessageBox.Show(res[0].Dubina.ToString());
            foreach (var item in desifrovanja)
            {
                DataGridAdminDesifrovanja.Items.Add(item);
            }
        }

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

        static async Task<List<Sifrovanje>> GetSvaSifrovanja(string putanja)
        {
            var jsonString = "";
            HttpResponseMessage response = await client.GetAsync(putanja);
            if (response.IsSuccessStatusCode)
            {
                jsonString = await response.Content.ReadAsStringAsync();
            }
            else MessageBox.Show("Nesto nije u redu.");
            var sifrovanje = JsonConvert.DeserializeObject<List<Sifrovanje>>(jsonString);

            return sifrovanje;
        }

        static async Task<List<Desifrovanje>> GetSvaDesifrovanja(string putanja)
        {
            var jsonString = "";
            HttpResponseMessage response = await client.GetAsync(putanja);
            if (response.IsSuccessStatusCode)
            {
                jsonString = await response.Content.ReadAsStringAsync();
            }
            else MessageBox.Show("Nesto nije u redu.");
            var desifrovanje = JsonConvert.DeserializeObject<List<Desifrovanje>>(jsonString);

            return desifrovanje;
        }
    }
}
