using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Services
{
    public class CardData
      {
        [JsonProperty("id_client_account")]
        public string id_client_account { get; set; }

        [JsonProperty("card_number")]
        public string card_number { get; set; }

        [JsonProperty("valid_month")]
        public int valid_month { get; set; }
        
        [JsonProperty("valid_year")]
        public int valid_year { get; set; }

        [JsonProperty("card_type")]
        public string card_type { get; set; }
    }
}
