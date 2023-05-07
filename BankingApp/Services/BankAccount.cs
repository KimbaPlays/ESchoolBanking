using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;


namespace BankingApp.Services
{

    //public class AccountBalance
    //{
    //    [JsonProperty("AccountBalance")]
    //    public string AccountBalance { get; set; }
    //}

    public class Account
    {
        [JsonProperty("iban")]
        public string iban { get; set; }

        [JsonProperty("account_type")]
        public string account_type { get; set; }

        [JsonProperty("id_account_type")]
        public int id_account_type { get; set; }

        [JsonProperty("id_client_account")]
        public int id_client_account { get; set; }
    }
}
