using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Demochka.Modelss;

public partial class BagotskayaContext : DbContext
{
    public BagotskayaContext()
    {
    }

    public BagotskayaContext(DbContextOptions<BagotskayaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Airline> Airlines { get; set; }

    public virtual DbSet<Airplane> Airplanes { get; set; }

    public virtual DbSet<AirplaneModel> AirplaneModels { get; set; }

    public virtual DbSet<Airport> Airports { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<Passenger> Passengers { get; set; }

    public virtual DbSet<PassengerFlight> PassengerFlights { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=79.174.88.58;Port=16639;Database=Bagotskaya;Username=Bagotskaya;Password=Bagotskaya123.");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("en_US.UTF-8")
            .HasPostgresExtension("pg_stat_statements");

        modelBuilder.Entity<Airline>(entity =>
        {
            entity.HasKey(e => e.AirlineCode).HasName("airlines_pkey");

            entity.ToTable("airlines");

            entity.Property(e => e.AirlineCode)
                .HasMaxLength(10)
                .HasColumnName("airline_code");
            entity.Property(e => e.AirlineName)
                .HasMaxLength(100)
                .HasColumnName("airline_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.EstablishmentDate).HasColumnName("establishment_date");
            entity.Property(e => e.FoundationDate).HasColumnName("foundation_date");
        });

        modelBuilder.Entity<Airplane>(entity =>
        {
            entity.HasKey(e => e.AirplaneId).HasName("airplanes_pkey");

            entity.ToTable("airplanes");

            entity.Property(e => e.AirplaneId)
                .HasMaxLength(20)
                .HasColumnName("airplane_id");
            entity.Property(e => e.AirlineCode)
                .HasMaxLength(10)
                .HasColumnName("airline_code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.ModelId).HasColumnName("model_id");
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("registration_date");

            entity.HasOne(d => d.AirlineCodeNavigation).WithMany(p => p.Airplanes)
                .HasForeignKey(d => d.AirlineCode)
                .HasConstraintName("airplanes_airline_code_fkey");

            entity.HasOne(d => d.Model).WithMany(p => p.Airplanes)
                .HasForeignKey(d => d.ModelId)
                .HasConstraintName("airplanes_model_id_fkey");
        });

        modelBuilder.Entity<AirplaneModel>(entity =>
        {
            entity.HasKey(e => e.ModelId).HasName("airplane_models_pkey");

            entity.ToTable("airplane_models");

            entity.Property(e => e.ModelId).HasColumnName("model_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.MaxPassengers).HasColumnName("max_passengers");
            entity.Property(e => e.ModelName)
                .HasMaxLength(100)
                .HasColumnName("model_name");
        });

        modelBuilder.Entity<Airport>(entity =>
        {
            entity.HasKey(e => e.AirportCode).HasName("airports_pkey");

            entity.ToTable("airports");

            entity.HasIndex(e => e.IataCode, "airports_iata_code_key").IsUnique();

            entity.HasIndex(e => e.IcaoCode, "airports_icao_code_key").IsUnique();

            entity.Property(e => e.AirportCode)
                .HasMaxLength(10)
                .HasColumnName("airport_code");
            entity.Property(e => e.AirportName)
                .HasMaxLength(200)
                .HasColumnName("airport_name");
            entity.Property(e => e.AirportNameRu)
                .HasMaxLength(200)
                .HasColumnName("airport_name_ru");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IataCode)
                .HasMaxLength(3)
                .HasColumnName("iata_code");
            entity.Property(e => e.IcaoCode)
                .HasMaxLength(4)
                .HasColumnName("icao_code");
            entity.Property(e => e.Latitude)
                .HasPrecision(9, 6)
                .HasColumnName("latitude");
            entity.Property(e => e.Longitude)
                .HasPrecision(9, 6)
                .HasColumnName("longitude");

            entity.HasOne(d => d.City).WithMany(p => p.Airports)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("airports_city_id_fkey");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("bookings_pkey");

            entity.ToTable("bookings");

            entity.HasIndex(e => e.TicketNumber, "bookings_ticket_number_key").IsUnique();

            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.BookingTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("booking_time");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.FlightId)
                .HasMaxLength(20)
                .HasColumnName("flight_id");
            entity.Property(e => e.LuggageCount)
                .HasDefaultValue(0)
                .HasColumnName("luggage_count");
            entity.Property(e => e.PassengerId).HasColumnName("passenger_id");
            entity.Property(e => e.PaymentDeadline)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("payment_deadline");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Pending'::character varying")
                .HasColumnName("payment_status");
            entity.Property(e => e.SeatClass)
                .HasMaxLength(20)
                .HasColumnName("seat_class");
            entity.Property(e => e.TicketNumber)
                .HasMaxLength(50)
                .HasColumnName("ticket_number");
            entity.Property(e => e.TicketPrice)
                .HasPrecision(10, 2)
                .HasColumnName("ticket_price");

            entity.HasOne(d => d.Flight).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.FlightId)
                .HasConstraintName("bookings_flight_id_fkey");

            entity.HasOne(d => d.Passenger).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.PassengerId)
                .HasConstraintName("bookings_passenger_id_fkey");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("cities_pkey");

            entity.ToTable("cities");

            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CityName)
                .HasMaxLength(100)
                .HasColumnName("city_name");
            entity.Property(e => e.CityNameRu)
                .HasMaxLength(100)
                .HasColumnName("city_name_ru");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("country_code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Timezone)
                .HasMaxLength(50)
                .HasColumnName("timezone");

            entity.HasOne(d => d.CountryCodeNavigation).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountryCode)
                .HasConstraintName("cities_country_code_fkey");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryCode).HasName("countries_pkey");

            entity.ToTable("countries");

            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("country_code");
            entity.Property(e => e.CountryName)
                .HasMaxLength(100)
                .HasColumnName("country_name");
            entity.Property(e => e.CountryNameRu)
                .HasMaxLength(100)
                .HasColumnName("country_name_ru");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.FlightId).HasName("flights_pkey");

            entity.ToTable("flights");

            entity.Property(e => e.FlightId)
                .HasMaxLength(20)
                .HasColumnName("flight_id");
            entity.Property(e => e.ActualArrival)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("actual_arrival");
            entity.Property(e => e.ActualDeparture)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("actual_departure");
            entity.Property(e => e.AirlineCode)
                .HasMaxLength(10)
                .HasColumnName("airline_code");
            entity.Property(e => e.AirplaneId)
                .HasMaxLength(20)
                .HasColumnName("airplane_id");
            entity.Property(e => e.ArrivalAirport)
                .HasMaxLength(10)
                .HasColumnName("arrival_airport");
            entity.Property(e => e.Bisnesprice).HasColumnName("bisnesprice");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DepartureAirport)
                .HasMaxLength(10)
                .HasColumnName("departure_airport");
            entity.Property(e => e.Economprise).HasColumnName("economprise");
            entity.Property(e => e.PassengerCount)
                .HasDefaultValue(0)
                .HasColumnName("passenger_count");
            entity.Property(e => e.ScheduledArrival)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("scheduled_arrival");
            entity.Property(e => e.ScheduledDeparture)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("scheduled_departure");

            entity.HasOne(d => d.AirlineCodeNavigation).WithMany(p => p.Flights)
                .HasForeignKey(d => d.AirlineCode)
                .HasConstraintName("flights_airline_code_fkey");

            entity.HasOne(d => d.Airplane).WithMany(p => p.Flights)
                .HasForeignKey(d => d.AirplaneId)
                .HasConstraintName("flights_airplane_id_fkey");

            entity.HasOne(d => d.ArrivalAirportNavigation).WithMany(p => p.FlightArrivalAirportNavigations)
                .HasForeignKey(d => d.ArrivalAirport)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_flights_arrival_airport");

            entity.HasOne(d => d.DepartureAirportNavigation).WithMany(p => p.FlightDepartureAirportNavigations)
                .HasForeignKey(d => d.DepartureAirport)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_flights_departure_airport");
        });

        modelBuilder.Entity<Passenger>(entity =>
        {
            entity.HasKey(e => e.PassengerId).HasName("passengers_pkey");

            entity.ToTable("passengers");

            entity.HasIndex(e => e.PassportNumber, "passengers_passport_number_key").IsUnique();

            entity.Property(e => e.PassengerId).HasColumnName("passenger_id");
            entity.Property(e => e.BuyerUser)
                .HasColumnType("character varying")
                .HasColumnName("buyer_user");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(200)
                .HasColumnName("full_name");
            entity.Property(e => e.LuggageInfo).HasColumnName("luggage_info");
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(50)
                .HasColumnName("passport_number");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.SalesInfo).HasColumnName("sales_info");
        });

        modelBuilder.Entity<PassengerFlight>(entity =>
        {
            entity.HasKey(e => e.PassengerFlightId).HasName("passenger_flights_pkey");

            entity.ToTable("passenger_flights");

            entity.HasIndex(e => new { e.PassengerId, e.FlightId }, "passenger_flights_passenger_id_flight_id_key").IsUnique();

            entity.Property(e => e.PassengerFlightId).HasColumnName("passenger_flight_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.FlightId)
                .HasMaxLength(20)
                .HasColumnName("flight_id");
            entity.Property(e => e.PassengerId).HasColumnName("passenger_id");
            entity.Property(e => e.UserByueId).HasColumnName("user_byue_id");

            entity.HasOne(d => d.Flight).WithMany(p => p.PassengerFlights)
                .HasForeignKey(d => d.FlightId)
                .HasConstraintName("passenger_flights_flight_id_fkey");

            entity.HasOne(d => d.Passenger).WithMany(p => p.PassengerFlights)
                .HasForeignKey(d => d.PassengerId)
                .HasConstraintName("passenger_flights_passenger_id_fkey");

            entity.HasOne(d => d.UserByue).WithMany(p => p.PassengerFlights)
                .HasForeignKey(d => d.UserByueId)
                .HasConstraintName("passenger_flights_users_fk");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("reservations_pkey");

            entity.ToTable("reservations");

            entity.Property(e => e.ReservationId).HasColumnName("reservation_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.FlightId)
                .HasMaxLength(20)
                .HasColumnName("flight_id");
            entity.Property(e => e.PassengerId).HasColumnName("passenger_id");
            entity.Property(e => e.PaymentTerm).HasColumnName("payment_term");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.ReservationTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("reservation_time");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Active'::character varying")
                .HasColumnName("status");

            entity.HasOne(d => d.Flight).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.FlightId)
                .HasConstraintName("reservations_flight_id_fkey");

            entity.HasOne(d => d.Passenger).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.PassengerId)
                .HasConstraintName("reservations_passenger_id_fkey");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("sales_pkey");

            entity.ToTable("sales");

            entity.Property(e => e.SaleId).HasColumnName("sale_id");
            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.PaymentAmount)
                .HasPrecision(10, 2)
                .HasColumnName("payment_amount");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("payment_date");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasColumnName("payment_method");
            entity.Property(e => e.TransactionReference)
                .HasMaxLength(100)
                .HasColumnName("transaction_reference");

            entity.HasOne(d => d.Booking).WithMany(p => p.Sales)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("sales_booking_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("newtable_pk");

            entity.ToTable("users");

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("nextval('newtable_user_id_seq'::regclass)")
                .HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Fio)
                .HasColumnType("character varying")
                .HasColumnName("fio");
            entity.Property(e => e.PassportData)
                .HasColumnType("character varying")
                .HasColumnName("passport_data");
            entity.Property(e => e.Phone)
                .HasColumnType("character varying")
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasColumnType("character varying")
                .HasColumnName("role");
            entity.Property(e => e.TicketId).HasColumnName("ticket_id");
            entity.Property(e => e.UserPassword)
                .HasColumnType("character varying")
                .HasColumnName("user_password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
