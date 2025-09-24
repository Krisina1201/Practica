using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Demochka.Modelss;
using Microsoft.EntityFrameworkCore;
using static Demochka.MainWindow;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Demochka;

public partial class TicketWindow : Window
{
    public class TicketItem
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
        public string Airline { get; set; } = string.Empty;
        public string DepartureTime { get; set; } = string.Empty;
        public string ArrivalTime { get; set; } = string.Empty;
        public string Fio { get; set; } = string.Empty;
        public string PassportData { get; set; } = string.Empty;
        public string Baggage { get; set; } = string.Empty;
        public string FlightId { get; set; } = string.Empty;
    }

    public BagotskayaContext context = new BagotskayaContext();

    public TicketWindow()
    {
        InitializeComponent();

        TicketListBox = this.FindControl<ListBox>("TicketsListBox");

        LoadUserTickets();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void LoadUserTickets()
    {
        if (!AuthService.IsAuthenticated)
        {
            var emptyItem = new TicketItem
            {
                Fio = "Пользователь не аутентифицирован"
            };
            TicketListBox.ItemsSource = new List<TicketItem> { emptyItem };
            return;
        }

        var currentUserId = AuthService.CurrentUserId;

        var userTickets = context.PassengerFlights
            .Where(pf => pf.UserByueId == currentUserId)
            .Include(pf => pf.Passenger)
            .Include(pf => pf.Flight)
                .ThenInclude(f => f.ArrivalAirportNavigation)
                    .ThenInclude(a => a.City)
            .Include(pf => pf.Flight)
                .ThenInclude(f => f.DepartureAirportNavigation)
                    .ThenInclude(a => a.City)
            .Include(pf => pf.Flight)
                .ThenInclude(f => f.AirlineCodeNavigation)
            .Include(pf => pf.Flight)
                .ThenInclude(f => f.Bookings)
                    .ThenInclude(b => b.Passenger)
            .ToList();

        List<TicketItem> ticketItems = new List<TicketItem>();

        foreach (var passengerFlight in userTickets)
        {
            var flight = passengerFlight.Flight;

            var booking = flight.Bookings
                .FirstOrDefault(b => b.PassengerId == passengerFlight.PassengerId && b.FlightId == flight.FlightId);

            ticketItems.Add(new TicketItem
            {
                FromCity = flight.DepartureAirportNavigation?.City?.CityNameRu ?? "Не указано"/*,
                FromAiroport = flight.DepartureAirport ?? "Не указано",
                ToCity = flight.ArrivalAirportNavigation?.City?.CityNameRu ?? "Не указано",
                ToAiroport = flight.ArrivalAirport ?? "Не указано",
                TimeInFly = FormatTimeSpan(flight.ScheduledArrival - flight.ScheduledDeparture),
                Cost = flight.Economprise ?? flight.Bisnesprice ?? 0,
                DateToFly = flight.ScheduledDeparture.ToString("M") + " " + flight.ScheduledDeparture.ToString("ddd"),
                DateFromFly = flight.ScheduledArrival.ToString("M") + " " + flight.ScheduledArrival.ToString("ddd"),
                Airline = flight.AirlineCodeNavigation?.AirlineName ?? "Не указано",
                ArrivalTime = flight.ScheduledArrival.ToString("t"),
                DepartureTime = flight.ScheduledDeparture.ToString("t"),
                Fio = passengerFlight.Passenger?.FullName ?? "Не указано",
                PassportData = passengerFlight.Passenger?.PassportNumber ?? "Не указано",
                Baggage = booking?.LuggageCount?.ToString() ?? "Не указано",
                FlightId = flight.FlightId*/
            });
        }

        if (!ticketItems.Any() || ticketItems == null)
        {
            var emptyItem = new TicketItem
            {
                Fio = "У вас нет купленных билетов"
            };
            TicketListBox.ItemsSource = new List<TicketItem> { emptyItem };
        } else
        {
            TicketListBox.ItemsSource = new List<TicketItem>();
        }
    }

    private string FormatTimeSpan(TimeSpan timeSpan)
    {
        return $"{(int)timeSpan.TotalHours}ч {timeSpan.Minutes}м";
    }
}