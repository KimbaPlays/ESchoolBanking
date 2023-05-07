using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;


namespace BankingApp.Services
{
    public class TransactionData
    {

        [JsonProperty("id_client_transaction")]
        public string id_client_transaction { get; set; }

        [JsonProperty("id_client")]
        public string id_client { get; set; }

        [JsonProperty("iban_payer")]
        public string iban_payer { get; set; }

        [JsonProperty("iban_recipient")]
        public string iban_recipient { get; set; }

        [JsonProperty("payment_description")]
        public string payment_description { get; set; }

        [JsonProperty("name_recipient")]
        public string name_recipient { get; set; }

        [JsonProperty("model")]
        public string model { get; set; }

        [JsonProperty("reference_number")]
        public string reference_number { get; set; }

        [JsonProperty("amount")]
        public string amount { get; set; }

        [JsonProperty("currency")]
        public string currency { get; set; }
        [JsonProperty("date_time")]
        public string date_time { get; set; }


    }
}