using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media.Imaging;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Platform;
using Demochka.Modelss;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.ComponentModel;
using Avalonia.Input;

namespace Demochka;

public partial class MainWindow : Window
{
    public class AirlineItem
    {
        public string FromCity { get; set; } = string.Empty;
        public string FromAiroport { get; set; } = string.Empty;
        public string ToAiroport { get; set; } = string.Empty;
        public string ToCity { get; set; } = string.Empty;
        public string TimeInFly { get; set; } = string.Empty; 
        public int? Cost { get; set; }
        public string DateFromFly { get; set; } = string.Empty; 
        public string DateToFly { get; set; } = string.Empty; 
        public string BackFly { get; set; } = string.Empty; 
        public int CountPassagers { get; set; }
        public string Airline { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public string FlightId { get; set; }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public BagotskayaContext context = new BagotskayaContext();

    public MainWindow()
    {
        InitializeComponent();
        this.DataContext = this;

        try
        {
            var countries = context.Cities.Select(e => e.CityNameRu).ToList();
            FromBox.ItemsSource = countries;
            ToBox.ItemsSource = countries;

            List<string> passagersCount = new List<string>
            {
                "1 пассажир", "2 пассажира", "3 пассажира", "4 пассажира", "5 пассажиров"
            };
            PassagersComboboox.ItemsSource = passagersCount;

            checkContentBlock.Text = "Введите данные о перелете, \nчтобы пими Ваня смог найти билеты для вас!";
        }
        catch (Exception ex)
        {
            ShowErrorDialog("Ошибка", $"Ошибка инициализации: {ex.Message}", "tysa.png");
        }
    }

    private void OnListBoxDoubleTapped(object sender, TappedEventArgs e)
    {
        if (sender is ListBox listBox)
        {
            var selectedItem = listBox.SelectedItem;
            AirlineItem airlineItem = new AirlineItem();

            if (selectedItem != null)
            {
                if (selectedItem is AirlineItem item)
                {
                    AddTickerWindow addTickerWindow = new AddTickerWindow(item);
                    addTickerWindow.Show();
                    this.Close();
                }
            }
        }
    }

    private void SwapCountry(object sender, RoutedEventArgs e)
    {
        var fromText = FromBox.SelectedItem as String;
        FromBox.SelectedItem = ToBox.SelectedItem;
        ToBox.SelectedItem = fromText;
    }

    private void ClickFindTicket(object sender, RoutedEventArgs e)
    {
        var from = FromBox.SelectedItem as string;
        var to = ToBox.SelectedItem as string;
        var toDate = ToDateBox.Text;
        var countPassagers = PassagersComboboox.SelectedItem as string;

        if (string.IsNullOrEmpty(from) ||
            string.IsNullOrEmpty(to) ||
            string.IsNullOrEmpty(toDate) ||
            string.IsNullOrEmpty(countPassagers))
        {
            ShowErrorDialog(title: "Ошибка!",
                imagePath: "tysa.png",
                message: "Заполните все поля, чтобы пикми Ваня \nсмог найти билеты для вас!");
        }
        else
        {
            LoadTicket(from, to, toDate, countPassagers);
            
        }
    }

    private void ShowErrorDialog(string title, string message, string imagePath)
    {
        try
        {
            var image = new Image
            {
                Source = new Bitmap(imagePath),
                Width = 80,
                Margin = new Thickness(0, 0, 10, 0)
            };

            var textBlock = new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Center
            };

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(20),
                Spacing = 10,
                Children = { textBlock, image }
            };

            var dialog = new Window
            {
                Title = title,
                Content = stackPanel,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            dialog.ShowDialog(this);
        }
        catch
        {
            var dialog = new Window
            {
                Title = title,
                Content = new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap },
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            dialog.ShowDialog(this);
        }
    }

    private void LoadTicket(string from, string to, string toDate, string countPassagers)
    {
        try
        {
            
            int passagersCount = int.Parse(countPassagers.Split(' ')[0]);

            var flights = context.Flights
                .Include(f => f.ArrivalAirportNavigation)
                .ThenInclude(a => a.City)
                .Include(f => f.DepartureAirportNavigation)
                .ThenInclude(a => a.City)
                .Include(f => f.Reservations)
                .Where(f => f.DepartureAirportNavigation.City.CityNameRu == from &&
                           f.ArrivalAirportNavigation.City.CityNameRu == to)
                .Include(e => e.AirlineCodeNavigation)
                .ToList();

            List<AirlineItem> itemList = new List<AirlineItem>();

            itemList = flights.Select(e => new AirlineItem
            {
                FromCity = e.DepartureAirportNavigation?.City?.CityNameRu ?? "Не указано",
                FromAiroport = e.DepartureAirport ?? "Не указано",
                ToCity = e.ArrivalAirportNavigation?.City?.CityNameRu ?? "Не указано",
                ToAiroport = e.ArrivalAirport ?? "Не указано",
                TimeInFly = FormatTimeSpan(e.ScheduledArrival - e.ScheduledDeparture),
                Cost = (e.Economprise ?? 0) * passagersCount,
                DateToFly = e.ScheduledDeparture.ToString("M") + " " + e.ScheduledDeparture.ToString("ddd"),
                DateFromFly = e.ScheduledArrival.ToString("M") + " " +  e.ScheduledArrival.ToString("ddd"),
                Airline = e.AirlineCodeNavigation.AirlineName,
                ArrivalTime = e.ScheduledArrival.ToString("t"),
                DepartureTime = e.ScheduledDeparture.ToString("t"),
                CountPassagers = passagersCount,
                FlightId = e.FlightId
            }).ToList();

            var i = itemList;

            if (itemList.Count() == 0)
            {
                ShowErrorDialog(title: "Ошибка",
                message: $"Пикми Ваня не нашел перелеты((",
                imagePath: "pickMeSad.png");
                InitialContentPanel.IsVisible = true;
                ListBoxTicket.IsVisible = false;
                
            }
            else
            {
                //if (DateTime.TryParse(toDate, out DateTime filterDate))
                //{
                //    itemList = itemList.Where(e =>
                //        DateTime.ParseExact(e.DateFly, "dd.MM.yyyy", CultureInfo.InvariantCulture).Date == filterDate.Date)
                //        .ToList();
                //}

                ListBoxTicket.ItemsSource = itemList;
                InitialContentPanel.IsVisible = false;
                ListBoxTicket.IsVisible = true;
            }

            
        }
        catch (Exception ex)
        {
            ShowErrorDialog(title: "Ошибка",
                message: $"Произошла ошибка: {ex.Message}",
                imagePath: "tysa.png");
        }
    }

    

    private string FormatTimeSpan(TimeSpan timeSpan)
    {
        return $"{(int)timeSpan.TotalHours}ч {timeSpan.Minutes}м";
    }
    

    private void ProfileClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {

        if (AuthService.IsAuthenticated)
        {
            UserWindow userWindow = new UserWindow();
            userWindow.Show();
            this.Close();
        } else
        {
            ProfileWindow profileWindow = new ProfileWindow();
            profileWindow.Show();
            this.Close();
        }
        
    }
}