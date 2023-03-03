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
using System.Globalization;

namespace BankingApp.Views;

public partial class TransactionSuccess : ContentPage
{
    private dynamic _paymentData;

    public TransactionSuccess(dynamic paymentData)
    {
        InitializeComponent();
        _paymentData = paymentData;
        this.BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        idClientLabel.Text = "ID Client: " + _paymentData.currentid_client;
        ibanPayerLabel.Text = "IBAN Payer: " + _paymentData.ibanPayer;
        ibanRecipientLabel.Text = "IBAN Recipient: " + _paymentData.ibanRecipient;
        nameRecipientLabel.Text = "Name Recipient: " + _paymentData.nameRecipient;
        paymentDescriptionLabel.Text = "Payment Description: " + _paymentData.paymentDescription;
        modelLabel.Text = "Model: " + _paymentData.model;
        referenceNumberLabel.Text = "Reference Number: " + _paymentData.referenceNumber;
        dateTimeLabel.Text = "Date Time: " + _paymentData.dateTime.ToString();
        amountLabel.Text = "Amount: " + _paymentData.amountValue.ToString("C", CultureInfo.CurrentCulture);
    }

    protected override bool OnBackButtonPressed()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            var result = await this.DisplayAlert("Alert!", "Do you want to screenshot a receipt?", "Yes", "No");
            if (!result)
            {
                // Pass the account object to the MenuPage constructor
                await Navigation.PopToRootAsync();
            }
        });

        return true;
    }
}