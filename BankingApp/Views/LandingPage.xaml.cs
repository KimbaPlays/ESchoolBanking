namespace BankingApp;

public partial class LandingPage : ContentPage
{
	public LandingPage()
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
    private void GoToLogin(object sender, EventArgs e)
    {
        Navigation.PushAsync(new LoginPage());
    }
}