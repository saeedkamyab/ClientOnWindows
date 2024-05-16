using ClientOnWindows.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Windows;
using System.Windows.Documents;


namespace ClientOnWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static HttpClient client = new HttpClient();
        public MainWindow()
        {
            InitializeComponent();
        }

        static async Task<string> GetJwtTokenAsync(string username, string password, string authUrl)
        {
            Login loginModel = new Login();
            loginModel.UserName = username;
            loginModel.Password = password;


            var content = new StringContent(JsonConvert.SerializeObject(loginModel), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(authUrl, content);
            response.EnsureSuccessStatusCode();

            // دریافت و پردازش توکن JWT
            var responseString = await response.Content.ReadAsStringAsync();
            var jwtToken = responseString;

            return jwtToken;
        }
        static async Task<string> SendRequestWithJwtToken(string apiUrl, string token)
        {
            // افزودن توکن به Header درخواست
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // ارسال درخواست به API و دریافت پاسخ
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            // پردازش و چاپ پاسخ
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        private async void BtnLoadData_Click(object sender, RoutedEventArgs e)
        {
            string apiUrl = "https://localhost:7102/api/Users";
            string myToken = await GetJwtTokenAsync("admin", "1234", "https://localhost:7102/api/Auth");

            var personList = JsonConvert.DeserializeObject<List<User>>(await SendRequestWithJwtToken(apiUrl, myToken));

            foreach (var item in personList)
            {
                ListData.Items.Add("User Name: " + item.UserName + "  Full Name: " + item.FullName + "  F Name: " + item.FName);
            }

        }
    }
}