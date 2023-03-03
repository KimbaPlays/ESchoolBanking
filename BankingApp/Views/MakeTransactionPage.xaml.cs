using Newtonsoft.Json;
using System.Net;
using System.Text;
using BankingApp.Models;
using BankingApp.Services;
using BankingApp.Views;
using System.Globalization;

namespace BankingApp.Views;

public partial class MakeTransactionPage : ContentPage
{
    // Variables for the Reference Number
    private int yearLastDigit;
    private int monthLastDigit;
    private int dayLastDigit;
    private Random random;

    //ID Client Variable for later
    string currentid_client = App.ActiveAccount.id_client_account.ToString();

    public MakeTransactionPage()
    {
        InitializeComponent();
    }

    //Get the Numbers for the Reference Number
    private DateTime GetDaTime()
    {
        DateTime now = DateTime.Now;

        yearLastDigit = now.Year % 10;
        monthLastDigit = now.Month % 10;
        dayLastDigit = now.Day % 10;

        random = new Random();

        return now;
    }

    // Generate the Reference Number
    public void GenerateReferenceNumber()
    {
        int random1 = random.Next(10);
        int random2 = random.Next(10);
        int random3 = random.Next(10);

        string referenceNumber = $"{yearLastDigit}{monthLastDigit}{dayLastDigit}-{random1}{random2}{random3}";

    }

    //Make the Transaction
    public async Task MakePaymentAsync(string currentid_client, string ibanPayer, string ibanRecipient, string nameRecipient, string paymentDescription, string model, string referenceNumber, DateTime dateTime, decimal amountValue)
    {
        // Create a new HttpClient instance
        HttpClient client = new HttpClient();

        // Set the API endpoint URL
        string url = $"https://app.eschoolbank.com/api/mobile_app/post_transaction.php?id_client={currentid_client}&iban_payer={ibanPayer}&iban_recipient={ibanRecipient}&name_recipient={nameRecipient}&payment_description={paymentDescription}&model={model}&reference_number={referenceNumber}&date_time={dateTime}&amount={amountValue}";

        // Create an anonymous object to hold the payment parameters
        var paymentData = new
        {
            currentid_client,
            ibanPayer,
            ibanRecipient,
            nameRecipient,
            paymentDescription,
            model,
            referenceNumber,
            dateTime,
            amountValue
        };

        // Serialize the payment data to JSON using Newtonsoft.Json
        string jsonPayload = JsonConvert.SerializeObject(paymentData);

        // Create a new HttpRequestMessage with the JSON payload and content type header
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        // Send the POST request to the API endpoint
        HttpResponseMessage response = await client.SendAsync(request);

        // Check if the response was successful (status code in the range 200-299)
        if (response.IsSuccessStatusCode)
        {
            // Payment was successful
            await DisplayAlert("Success!", "The transaction you made was successful.", "Ok.");
            await Navigation.PushAsync(new TransactionSuccess(paymentData));
        }
        else
        {
            // Payment failed
            await DisplayAlert("Something went wrong...", "The transaction did not go through... try again", "Ok.");
            Console.WriteLine($"Payment failed with status code {response.StatusCode}");
        }
    }

    public async void MakeTransactionButton(object sender, EventArgs e)
    {
     
        //Retrieve the payment data from other sources
        string iban = App.ActiveAccount.iban;

        // Generate a reference number
        GetDaTime();
        GenerateReferenceNumber();
        string referenceNumber = $"{yearLastDigit}{monthLastDigit}{dayLastDigit}-{random.Next(10)}{random.Next(10)}{random.Next(10)}";

        // Retrieve the payment data from the input labels
        string id_client = currentid_client;
        string ibanPayer = iban;
        string ibanRecipient = ibanRecipientEntry.Text;
        string nameRecipient = nameRecipientEntry.Text;
        string paymentDescription = paymentDescriptionEntry.Text;
        string model = "Yes";
        DateTime dateTime = DateTime.Now ;
        string amountValue = amountEntry.Text.ToString();

        // Validate the input data
        if (string.IsNullOrWhiteSpace(ibanRecipient))
        {
            await DisplayAlert("Error", "Please enter a recipient IBAN.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(nameRecipient))
        {
            await DisplayAlert("Error", "Please enter a recipient name.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(paymentDescription))
        {
            await DisplayAlert("Error", "Please enter a payment description.", "OK");
            return;
        }

        if (!decimal.TryParse(amountValue, NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal amount))
        {
            await DisplayAlert("Error", "Please enter a valid amount.", "OK");
            return;
        }


        // Call the MakePaymentAsync method with the payment data
        await MakePaymentAsync(id_client, ibanPayer, ibanRecipient, nameRecipient, paymentDescription, model, referenceNumber, dateTime, amount);
    }
}