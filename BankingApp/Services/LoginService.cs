using BankingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BankingApp.Services
{

    public class LoginService : ILoginRepository
    {
        public async Task<int> Login(string email, string password)
        {

            var client = new HttpClient();
            string url = "https://app.eschoolbank.com/api/mobile_app/login.php?email=" + email + "&password=" + password;
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                var userInfo = JsonConvert.DeserializeObject<LoginResult>(content);
                return userInfo.status == "200" ? userInfo.id_user : -1; ;

            }
            else
            {
                return -1;
            }


        }
    }
}
