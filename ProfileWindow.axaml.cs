using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
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
        authButton.Background = new SolidColorBrush(Colors.Pink);
        regButton.Background = new SolidColorBrush(Colors.White);
    }

    public BagotskayaContext context;

  
    private void AuthClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        AuthPanel.IsVisible = true;
        RegistrationPanel.IsVisible = false;

        authButton.Background = new SolidColorBrush(Colors.Pink);
        regButton.Background = new SolidColorBrush(Colors.White);
    }

    private void RegClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        AuthPanel.IsVisible = false;
        RegistrationPanel.IsVisible = true;

        authButton.Background = new SolidColorBrush(Colors.White);
        regButton.Background = new SolidColorBrush(Colors.Pink);
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
                AuthService.Login(checkUser.UserId);
                UserWindow userWindow = new UserWindow();
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

                AuthService.Login(user.UserId);

                UserWindow userWindow = new UserWindow();
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