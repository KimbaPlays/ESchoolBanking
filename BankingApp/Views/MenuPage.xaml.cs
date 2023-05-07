using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using BankingApp.Models;
using Microsoft.Maui.Controls;
using BankingApp.Services;
using Microsoft.Maui.Controls.Xaml;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using BankingApp.Views;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace BankingApp
{
    public partial class MenuPage : ContentPage, INotifyPropertyChanged
    {
       //Transactions
        private ObservableCollection<TransactionData> _transactions = new ObservableCollection<TransactionData>();
        public ObservableCollection<TransactionData> Transactions
        {
            get { return _transactions; }
            set
            {
                _transactions = value;
                OnPropertyChanged(nameof(Transactions));
            }
        }

        //Cards
        private ObservableCollection<CardData> _cards = new ObservableCollection<CardData>();
        public ObservableCollection<CardData> Cards
        {
            get { return _cards; }
            set
            {
                _cards = value;
                OnPropertyChanged(nameof(Cards));
            }
        }
        //Selected Card
        private CardData _selectedCard;
        public CardData SelectedCard
        {
            get { return _selectedCard; }
            set { _selectedCard = value; OnPropertyChanged(nameof(SelectedCard)); OnPropertyChanged(nameof(IsCardSelected)); }
        }
        public bool IsCardSelected
        {
            get { return SelectedCard != null; }
        }

        //Active Account
        public Account SelectedAccount { get; }
        
        //Account Balance
        public class AccountBalance
        {

            [JsonProperty("status")]
            public string status { get; set; }

            [JsonProperty("msg")]
            public string msg { get; set; }

            [JsonProperty("data")]
            public string data { get; set; }

            
        }
        public MenuPage(Account _selectedAccount)
        {
            InitializeComponent();
            Title = "Transactions";
            BindingContext = this;
            SelectedAccount = _selectedAccount;
        }

        //When the screen appears
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await GetTransactionsAsync();
            await GetBalanceAsync();
            await GetCardsAsync();
        }

        //Confirm you want to go back
        protected override bool OnBackButtonPressed()
        {
            MainThread.BeginInvokeOnMainThread(async () => {
                var result = await this.DisplayAlert("Alert!", "Do you really want to choose another account?", "Yes", "No");
                if (result) await this.Navigation.PopAsync(); 
            });

            return true;
        }

        //Get Transactions from Account
        private async Task GetTransactionsAsync()
        {
            string iban = App.ActiveAccount.iban;
            string url = $"https://app.eschoolbank.com/api/mobile_app/get_client_account_transactions.php?iban={iban}";

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                TransactionsResponse responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject<TransactionsResponse>(json);
                Transactions = responseObj.data;
            }
            else
            {
                Debug.WriteLine("Error: Unable to get transactions data.");
            }
        }

        //Get Account Balance
        private async Task GetBalanceAsync()
        {
            string iban = App.ActiveAccount.iban;
            string url = $"https://app.eschoolbank.com/api/mobile_app/get_account_balance.php?iban={iban}";

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();              

                JObject resultX = JObject.Parse(responseString);
                JObject data = (JObject)resultX["data"];
                string account_balance = (string)data["account_balance"];
                balanceLabel.Text = account_balance + "€";

            }
            else
            {
                Debug.WriteLine("Error: Unable to get balance data.");
            }
        }

        //Get Client Cards

        private async Task GetCardsAsync()
        {
            int id_client = App.ActiveAccount.id_client_account;
            string url = $"https://app.eschoolbank.com/api/mobile_app/get_client_cards.php?id_client={id_client}";

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                CardsResponse responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject<CardsResponse>(json);
                Cards = responseObj.data;
            }
            else
            {
                Debug.WriteLine("Error: Unable to get Card data.");
            }
        }


        //More Transactions -> Updates
        public new event PropertyChangedEventHandler PropertyChanged;

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == "Transactions")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Transactions"));
            }
            else
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class TransactionsResponse
        {
            public ObservableCollection<TransactionData> data { get; set; }

        }

        public class CardsResponse
        {
            public ObservableCollection<CardData> data { get; set; }

        }
        private void GotoTransactionPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MakeTransactionPage());
        }
        private void GotoLoanPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LoansPage(SelectedAccount));
        }
    }
}