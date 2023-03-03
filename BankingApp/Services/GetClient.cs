using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BankingApp.Services
{
    public class GetClient
    {
        public async Task<(int, string, string)> GetData(int id_user)
        {
            HttpClient client = new HttpClient();
            string url = $"https://app.eschoolbank.com/api/mobile_app/get_client.php?id_user={id_user}";
            var response = await client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();

            JObject result = JObject.Parse(responseString);
            if (result["status"].ToString() == "200")
            {
                JObject data = (JObject)result["data"];
                int id_client = (int)data["id_client"];
                string name = (string)data["name"];
                string surname = (string)data["surname"];

                return (id_client, name, surname);
            }
            else
            {
                return (-1, null, null); // Return some default values or null to indicate an error.
            }
        }
    }
}
