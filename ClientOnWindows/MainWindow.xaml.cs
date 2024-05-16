using ClientOnWindows.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;


namespace ClientOnWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static HttpClient client = new HttpClient();
        string myToken;
        LoginResult loginRes=new LoginResult();
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


        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUserName.Text;
            string pass = txtPassword.Text;

          var resultString = await GetJwtTokenAsync(username, pass, "https://localhost:7102/api/Auth");
            if (resultString != null)
            {
                loginRes= JsonConvert.DeserializeObject<LoginResult>(resultString);

                lblUserName.Content = loginRes.claims.FirstOrDefault(x => x.Type == "Username").Value;
                lblFullName.Content = loginRes.claims.FirstOrDefault(x => x.Type == "FullName").Value;

                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Colors.Green;
                gbAuthenticatedUser.Background = brush;

                myToken=loginRes.TokenString;
                BtnSendRequest.IsEnabled = true;
            }
        }

        private async void BtnSendRequest_Click(object sender, RoutedEventArgs e)
        {
            // var res = JsonConvert.DeserializeObject<List<User>>(await SendRequestWithJwtToken(apiUrl, myToken));
            string apiUrl = "";
            if (cmbAction.SelectedIndex == 0) apiUrl = "https://localhost:7102/api/Articles";
            if (cmbAction.SelectedIndex == 1) apiUrl = "https://localhost:7102/api/Books";
            if (cmbAction.SelectedIndex == 2) apiUrl = "https://localhost:7102/api/Users";

            try
            {
                Result.Text = await SendRequestWithJwtToken(apiUrl, myToken);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }
    }
}