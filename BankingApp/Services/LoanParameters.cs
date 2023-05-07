using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;


namespace BankingApp.Services
{
    public class LoanData
    {

        [JsonProperty("id_client_loan")]
        public string id_client_loan { get; set; }

        [JsonProperty("id_client")]
        public string id_client { get; set; }

        [JsonProperty("id_loan_type")]
        public string id_loan_type { get; set; }

        [JsonProperty("amount")]
        public string amount { get; set; }

        [JsonProperty("active")]
        public string active { get; set; }

        [JsonProperty("loan_type")]
        public string loan_type { get; set; }

    }
}