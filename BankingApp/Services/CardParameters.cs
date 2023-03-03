using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace BankingApp.Services
{
    public class CardData
    {
        [JsonProperty("id_client_card")]
        public string id_client_card { get; set; }
        
        [JsonProperty("id_client")]
        public string id_client { get; set; }
        
        [JsonProperty("id_card_type")]
        public string id_card_type { get; set; }
        
        [JsonProperty("card_number")]
        public string card_number { get; set; }
        
        [JsonProperty("valid_month")]
        public string valid_month { get; set; }
       
        [JsonProperty("valid_year")]
        public string valid_year { get; set; }
        
        [JsonProperty("active")]
        public string active { get; set; }
        
        [JsonProperty("card_type")]
        public string card_type { get; set; }
    }

}
