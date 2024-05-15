using ClientOnWindows.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Windows;


namespace ClientOnWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnLoadData_Click(object sender, RoutedEventArgs e)
        {
            Login loginModel = new Login();
            loginModel.UserName = "admin";
            loginModel.Password = "1234";
            var client = new HttpClient { BaseAddress = new Uri("https://localhost:7102/api/") };
            string myToken = "";



            var jsonBody = JsonConvert.SerializeObject(loginModel);

            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = client.PostAsync("Auth", content).Result;
            if (response.IsSuccessStatusCode)
            {
                myToken = response.Content.ReadAsStringAsync().Result;
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,loginModel.UserName),
                    new Claim(ClaimTypes.Name,loginModel.UserName),
                    new Claim("AccessToken",myToken),
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

            }




            // Create HttpClient

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", myToken);
          
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + myToken);
            
            
            // Assign default header (Json Serialization)
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApiConstant.JsonHeader));

            // Make an API call and receive HttpResponseMessage
            var responseMessage = client.GetAsync("https://localhost:7102/api/Users").Result;

            // Convert the HttpResponseMessage to string
            var resultArray = responseMessage.Content.ReadAsStringAsync().Result;

            // Deserialize the Json string into type using JsonConvert
            var personList = JsonConvert.DeserializeObject<List<User>>(resultArray);

            if (personList != null)
                foreach (var item in personList)
                {
                    ListData.Items.Add("User Name: " + item.UserName + "  Full Name: " + item.FullName + "  F Name: " + item.FName);
                }

        }

        //private void BtnLoadData_Click(object sender, RoutedEventArgs e)
        //{

        //    // Create HttpClient
        //    var client = new HttpClient { BaseAddress = new Uri("https://localhost:7102/api/") };

        //    // Assign default header (Json Serialization)
        //    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApiConstant.JsonHeader));

        //    // Make an API call and receive HttpResponseMessage
        //    var responseMessage =  client.GetAsync("Users", HttpCompletionOption.ResponseContentRead).Result;

        //    // Convert the HttpResponseMessage to string
        //    var resultArray = responseMessage.Content.ReadAsStringAsync().Result;

        //    // Deserialize the Json string into type using JsonConvert
        //    var personList = JsonConvert.DeserializeObject<List<User>>(resultArray);

        //    foreach(var item in personList)
        //    {
        //        ListData.Items.Add("User Name: "+item.UserName+ "  Full Name: " + item.FullName+ "  F Name: "+item.FName);
        //    }

        //}
    }
}