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

namespace BankingApp
{
    public partial class MenuPage : ContentPage, INotifyPropertyChanged
    {
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

        public Account SelectedAccount { get; }

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


        //More Transactions -> Updates
        public new event PropertyChangedEventHandler PropertyChanged;

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == "Transactions")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Transactions"));
            }
        }
        public class TransactionsResponse
        {
            public ObservableCollection<TransactionData> data { get; set; }

        }
        private void GotoTransactionPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MakeTransactionPage());
        }
    }
}