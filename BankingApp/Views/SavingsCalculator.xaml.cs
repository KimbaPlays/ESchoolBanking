 using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Runtime.CompilerServices;
using static BankingApp.SavingsCalculator;

namespace BankingApp;
public partial class SavingsCalculator : ContentPage
{
    int selectedIndex = 0;
    public SavingsCalculator()
    {
        InitializeComponent();
        _ = new Entry { Keyboard = Keyboard.Numeric };
        BindingContext = this;
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
    //Calculadora de Conta Poupança 
    public ObservableCollection<MonthlyIncome> MonthlyIncomeCalculus { get; set; } = new();
    public class MonthlyIncome
    {
        public int Month { get; set; }
        public double Income { get; set; }

    }

    private void Calculate(object sender, EventArgs e)
    {
        MonthlyIncomeCalculus.Clear();

        MonthlyIncome monthi;
        int Months = int.Parse(MonthInput.Text);
        int Income = int.Parse(IncomeInput.Text);
        double Accumulated = 0;

        for (int i = 1; i <= Months; i++)
        {

            monthi = new MonthlyIncome();
            monthi.Month = i;

            //Input Validation (Inputs are empty)
            if (String.IsNullOrEmpty(MonthInput.Text) || String.IsNullOrEmpty(IncomeInput.Text))
            {
                DisplayAlert("Attention", "Please input values!", "Ok");
                return;
            }

            //Input Validation (Min/Max of Months)
            if (Months > 12 || Months < 1)
            {
                DisplayAlert("Attention", "Please input value of range 1-12", "Ok");
                return;
            }

            //Input Validation (Min of Investment)
            if (Income < 10)
            {
                DisplayAlert("Attention", "Please input minimum value of 10", "Ok");
                return;
            }

            //1ºMês - Meses Seguintes
            if (i == 1)
            {
                monthi.Income = (Income) * 1.10;
                Accumulated = (monthi.Income);
            }
            else if (i <= Months)
            {
                monthi.Income = Accumulated * 1.10;
                Accumulated = (monthi.Income);
            }

            if (selectedIndex == 0)
            {
                monthi.Income = monthi.Income;
            }
            if (selectedIndex == 1)
            {
                monthi.Income = monthi.Income * 1.05;
            }

            if (selectedIndex == 2)
            {
                monthi.Income = monthi.Income * 7.56;
            }

            //Arredondamento
            monthi.Income = Math.Round(monthi.Income, 2);

            //Próximo Mês
            MonthlyIncomeCalculus.Add(monthi);
        }
    }
    private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        selectedIndex = picker.SelectedIndex;
    }

}
    //Collection View Par Impar
    public class AlternateRowColour : DataTemplateSelector
    {

        public DataTemplate EvenTemplate { get; set; }
        public DataTemplate OddTemplate { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is MonthlyIncome mi)
            {
                if (mi.Month % 2 == 0)
                {
                    return EvenTemplate;
                }
                else
                {
                    return OddTemplate;
                }
            }
            throw new NotImplementedException("Something went wrong...");
        }

    }
