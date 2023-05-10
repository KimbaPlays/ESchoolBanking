using Newtonsoft.Json.Linq;
using System.Xml;
using System.Diagnostics;
using Newtonsoft.Json;
using BankingApp.Services;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Maui.Controls.Xaml;
using System.Collections.ObjectModel;
using BankingApp.Models;
using System.Transactions;

namespace BankingApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class ChooseActiveAccountPage : ContentPage
    {
        private int id_user;
        public ObservableCollection<Account> _accounts = new ObservableCollection<Account>();
        public Account _selectedAccount;
        //public AccountBalance _accountBalance;
        public ChooseActiveAccountPage(int id_user)
        {
            InitializeComponent();
            this.id_user = id_user;
            GetClientData();

        }

        //Welcome Name + Surname
        private async void GetClientData()
        {
            var getClient = new GetClient();
            (int id_client, string name, string surname) = await getClient.GetData(id_user);

            if (id_client != -1)
            {
                string clientId = id_client.ToString();

                nameLabel.Text = name;
                surnameLabel.Text = surname;
                GetAccounts(id_client);

            }
            else
            {
                Debug.WriteLine("Error: Unable to get client data.");
            }
        }


        // Choose Active Account

        private void GetAccounts(int id_client)
        {
            HttpClient client = new HttpClient();
            int clientId = id_client;
            string url = $"https://app.eschoolbank.com/api/mobile_app/get_client_accounts.php?id_client={clientId}";

            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                
                    string json = response.Content.ReadAsStringAsync().Result;
                    ChooseActiveAccountsResponse responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject<ChooseActiveAccountsResponse>(json);
                    _accounts = responseObj.data;
                    accountsListView.ItemsSource = _accounts;

            }
            else
            {
                Debug.WriteLine("Error: Unable to get client data.");
            }

        }

        private void OnAccountSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _selectedAccount = e.SelectedItem as Account;
        }

        private void OnContinueButtonClicked(object sender, EventArgs e)
        {
            if (_selectedAccount != null)
            {
                App.ActiveAccount = _selectedAccount;
                Navigation.PushAsync(new MenuPage(_selectedAccount));
            }
            else
            {
                DisplayAlert("Error", "Please select an account.", "OK");
            }
        }

        private void SetActiveAccount(Account account)
        {
            App.ActiveAccount = account;
        }
    }
    //private void GetAccountBalance()
    //{
    //    string iban = App.ActiveAccount.iban;
    //    string url = $"https://app.eschoolbank.com/api/mobile_app/get_account_balance.php?iban={iban}";

    //    HttpClient client = new HttpClient();
    //    HttpResponseMessage response = await client.GetAsync(url);

    //    if (response.IsSuccessStatusCode)
    //    {
    //        string json = await response.Content.ReadAsStringAsync();
    //        GetBalanceResponse responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject<GetBalanceResponse>(json);
    //        AccountBalance = responseObj.data;
    //    }
    //    else
    //    {
    //        Debug.WriteLine("Error: Unable to get loan data.");
    //    }
    //}


    public class ChooseActiveAccountsResponse
    {
        public ObservableCollection<Account> data { get; set; }
    }
    //public class GetBalanceResponse
    //{
    //    public ObservableCollection<AccountBalance> data { get; set; }
    //}

}




