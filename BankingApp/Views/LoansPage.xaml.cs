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

namespace BankingApp.Views;
[XamlCompilation(XamlCompilationOptions.Skip)]
public partial class LoansPage : ContentPage, INotifyPropertyChanged
{
    private ObservableCollection<LoanData> _loans = new ObservableCollection<LoanData>();

    public ObservableCollection<LoanData> Loans
    {
        get { return _loans; }
        set
        {
            _loans = value;
            OnPropertyChanged(nameof(Loans));
        }
    }

    public Account SelectedAccount { get; }

    public LoansPage(Account _selectedAccount)
	{
        InitializeComponent();
        Title = "Transactions";
        BindingContext = this;
        SelectedAccount = _selectedAccount;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetLoansAsync();
    }

    private async Task GetLoansAsync()
    {
        string iban = App.ActiveAccount.iban;
        int id_client = App.ActiveAccount.id_client_account;
        string url = $"https://app.eschoolbank.com/api/mobile_app/get_client_loans.php?id_client={id_client}";

        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            string json = await response.Content.ReadAsStringAsync();
            LoansResponse responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject<LoansResponse>(json);
            Loans = responseObj.data;
        }
        else
        {
            Debug.WriteLine("Error: Unable to get loan data.");
        }
    }


    //More Transactions -> Updates
    public new event PropertyChangedEventHandler PropertyChanged;

    protected override void OnPropertyChanged(string propertyName)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == "Loans")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Loans"));
        }
    }
    public class LoansResponse
    {
        public ObservableCollection<LoanData> data { get; set; }

    }
}