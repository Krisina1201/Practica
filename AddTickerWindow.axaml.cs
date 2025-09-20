using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Demochka.Modelss;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using static Demochka.MainWindow;
using static System.Net.Mime.MediaTypeNames;

namespace Demochka;

public partial class AddTickerWindow : Window
{
    public AddTickerWindow()
    {
        InitializeComponent();
    }

    public ObservableCollection<Passenger> authPassager;
    public BagotskayaContext context = new BagotskayaContext();
    public string flightId;

    public AddTickerWindow(AirlineItem selectedItem)
    {
        InitializeComponent();
        airlaneTextBlock.Text = selectedItem.Airline;
        timeInFlightBlock.Text = selectedItem.TimeInFly;
        dateFromTextBlock.Text = selectedItem.DateFromFly;
        timeFromTextBlock.Text = selectedItem.DepartureTime;
        dateToTextBlock.Text = selectedItem.DateToFly;
        timeToBlock.Text = selectedItem.ArrivalTime;
        dateToTextBlock.Text = selectedItem.DateToFly;
        sityFromTextBlock.Text = selectedItem.FromCity;
        airoportFromTextBlock.Text = " (" + selectedItem.FromAiroport + ")";
        airoportToTextBlock.Text = " (" + selectedItem.ToAiroport + ")";
        sityToTextBlock.Text = selectedItem.ToCity;
        countPaccager.Text = selectedItem.CountPassagers.ToString() + PassagerWorld(selectedItem.CountPassagers);
        costTextBlock.Text = "Цена:" + selectedItem.Cost.ToString();

        authPassager = new ObservableCollection<Passenger>();

        flightId = selectedItem.FlightId;
        
    }

    private string PassagerWorld(int count) { if (count == 1) return " пассажир"; else if (count == 5) return " пассажиров"; else return " пассажира"; }

    private void AddPassagerClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        UpdatePeopleList();
        

        FIOTextBox.Text = string.Empty;
        PassportDataBox.Text = string.Empty;
        EmailTextBox.Text = string.Empty;
        PhoneTextBox.Text = string.Empty;
        checkBagage.IsChecked = false;

    }

    private void UpdatePeopleList()
    {
        var passportData = PassportDataBox.Text;
        var fio = FIOTextBox.Text;
        var email = EmailTextBox.Text;
        var phone = PhoneTextBox.Text;
        var bagage = checkBagage.IsChecked;

        string bagageString;

        if (passportData == null ) { ShowErrorDialog("Ошибка!", "Проверьте корректность паспортных данных"); return; }
        if (fio == null) { ShowErrorDialog("Ошибка!", "Заполните ваше ФИО"); return; }
        if (email == null || !email.Contains("@") || !email.Contains(".")) { ShowErrorDialog("Ошибка!", "Проверьте корректность почты"); return; }
        if (phone == null || phone.Length != 10) { ShowErrorDialog("Ошибка!", "Проверьте корректность Введенного номера телефона"); return; }

        if (bagage != null && bagage == true) bagageString = "1 чемодан"; else bagageString = "Ручная кладь";

        authPassager.Add(new Passenger
        {
            FullName = fio,
            Email = email,
            PhoneNumber = phone,
            PassportNumber = passportData,
            LuggageInfo = bagageString,
            CreatedAt = DateTime.Now,
            SalesInfo = "Первая покупка"
        });

        PassagersListBox.ItemsSource = authPassager;
    }


    private void ShowErrorDialog(string title, string message)
    {
        var dialog = new Window
        {
            Title = title,
            Content = new TextBlock { Text = message },
            SizeToContent = SizeToContent.WidthAndHeight,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };
        dialog.ShowDialog(this);
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }

    private void DeleteClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Passenger selectedPassager = (Passenger)(sender as Button).Tag;

        if (selectedPassager == null) { ShowErrorDialog("Ошибка!", "Произошла ошибка на стороне сервера"); return; }

        authPassager.RemoveAt(authPassager.IndexOf(selectedPassager));

        PassagersListBox.ItemsSource = authPassager;

        ShowErrorDialog("Успешное удаление!", $"Пассажир с именем {selectedPassager.FullName} удален");
    }


    private void ByeTicket(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try
        {
            if (authPassager.Count == 0)
            {
                ShowErrorDialog("Ошибка!", "Введите данные о пассажирах");
                return;
            }

            foreach (Passenger item in authPassager)
            {
                context.Passengers.Add(item);
            }

            context.SaveChanges();

            List<PassengerFlight> licttt = new List<PassengerFlight>();

            foreach (Passenger item in authPassager)
            {
                var passagerId = context.Passengers.FirstOrDefault(e => e.PassportNumber == item.PassportNumber).PassengerId;
                context.PassengerFlights.Add(new PassengerFlight
                {
                    PassengerId = passagerId,
                    CreatedAt = DateTime.Now,
                    FlightId = flightId
                });
            }

            context.SaveChanges();

            ShowErrorDialog("Успешно!", "Билет успешно оформлен");

            PassagersListBox.ItemsSource = new ObservableCollection<Passenger>();

        } catch(Exception ex)
        {
            ShowErrorDialog("Ошибка!", ex.ToString());
        }
        
    }

}              