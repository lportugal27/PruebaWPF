using Newtonsoft.Json;
using PruebaWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace PruebaWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool IsBusy { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            IsBusy = false;
        }
        public async Task<List<UserModel>> GetItemsAsync(string path)
        {
            
            List<UserModel> items = new List<UserModel>();
            try
            {
                Uri uri = new Uri(string.Format("https://api.github.com/users", string.Empty));
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "C# App");
                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    items = JsonConvert.DeserializeObject<List<UserModel>>(content);
                }
                else
                {
                    LabelResults.Content = "HttpStatus Code Error";
                }
            }
            catch (WebException)
            {
                LabelResults.Content = "Exception: Debe conectarse a una red Wi-Fi o tener datos móviles.";
            }
            catch (Exception ex)
            {
                LabelResults.Content = "Exception: " + ex.Message;
            }
            return items;
        }
        
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            LabelResults.Content = "";
            input.Text = "";
            if (IsBusy == false)
            {
                IsBusy = true;
                List<UserModel> items = await GetItemsAsync("https://api.github.com/users");
                string message = "";
                foreach (var item in items)
                {
                    message += "id:" + item.id + ",";
                    message += "login:" + item.login + ",";
                    message += "node_id:" + item.node_id + "|";
                }
                input.Text = message;
                IsBusy = false;
            }
            else
            {
                LabelResults.Content = "Ya se le dio clic";
            }
        }
    }
}
