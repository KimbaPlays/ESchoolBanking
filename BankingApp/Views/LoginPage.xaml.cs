using BankingApp.Models;
using BankingApp.Services;

namespace BankingApp.Views;

public partial class LoginPage : ContentPage
{
    readonly ILoginRepository _loginRepository = new LoginService();
    public LoginPage()
    {
        InitializeComponent();
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        InternetFailure.IsVisible = accessType != NetworkAccess.Internet && accessType != NetworkAccess.ConstrainedInternet;
        Connectivity.ConnectivityChanged += ConnectivityChanged;
    }

   
    //Alerta de falta de internet
    void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {

        InternetFailure.IsVisible = e.NetworkAccess != NetworkAccess.Internet && e.NetworkAccess != NetworkAccess.ConstrainedInternet;
        if (InternetFailure.IsVisible)
        {
            DisplayAlert("Attention", "You must be connected to the internet", "Ok");
        }
    }

    //Botão Sign In

    private async void LoginButton(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Attention", "You must input a username and password.", "Ok");
            return;
        }

        int id_user = await _loginRepository.Login(username, password);
        if (id_user != -1)
        {
            var page = new ChooseActiveAccountPage(id_user);
            await Navigation.PushAsync(new NavigationPage(page));
        }
        else
        {
            await DisplayAlert("Warning", "Username or password is incorrect.", "Ok");
        }
    }
}

