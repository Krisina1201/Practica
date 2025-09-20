using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Demochka.Modelss;
using System;
using System.Linq;

namespace Demochka;

public partial class ProfileWindow : Window
{
    public ProfileWindow()
    {
        InitializeComponent();
        AuthPanel.IsVisible = true;
        RegistrationPanel.IsVisible = false;

        context = new BagotskayaContext();
    }

    public BagotskayaContext context;

    private void AuthClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        AuthPanel.IsVisible = true;
        RegistrationPanel.IsVisible = false;
    }

    private void RegClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        AuthPanel.IsVisible = false;
        RegistrationPanel.IsVisible = true;

    }

    private void AuthClickDB(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        string email = EmailTextA.Text;
        string password = PasswordTextA.Text;

        if (email == null || password == null)
        {
            ShowErrorDialog("Ошибка", "Все поля должны быть заполнены");
        } else
        {
            var checkUser = context.Users.FirstOrDefault(e => e.Email == email && e.UserPassword == password);

            if (checkUser == null) 
            {
                ShowErrorDialog("Ошибка", "Пользователь с такими данными не найден");
            } else
            {
                UserWindow userWindow = new UserWindow(checkUser);
                userWindow.Show();
                this.Close();
            }
        }
    }

    private void RegClickDB(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        string email = EmailTextR.Text;
        string password = PasswordTextR.Text;
        string fio = FioTextR.Text;

        if (email == null || password == null || fio == null)
        {
            ShowErrorDialog("Ошибка", "Все поля должны быть заполнены");
        } else
        {
            try
            {
                int userId = context.Users.ToList().Count();

                User user = new User
                {
                    UserId = userId + 1,
                    Fio = fio,
                    UserPassword = password,
                    Email = email,
                    Role = "юзер"
                };

                context.Users.Add(user);

                context.SaveChanges();

                UserWindow userWindow = new UserWindow(user);
                userWindow.Show();
                this.Close();
            } catch (Exception eq)
            {
                ShowErrorDialog("Ошибка", $"Произошла ошибка при сохранении, попробуйте позже\n {eq}");
            }
        }
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
}