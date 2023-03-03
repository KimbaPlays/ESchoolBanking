using BankingApp.Services;
using BankingApp.Views;
using BankingApp.Models;


namespace BankingApp;

public partial class App : Application
{
    public static Account ActiveAccount { get; set; }

    public App()
    {
        InitializeComponent();

        Application.Current.UserAppTheme = AppTheme.Dark;

        App.Current.MainPage = new AppShell();
    }
}
