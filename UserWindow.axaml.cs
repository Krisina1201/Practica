using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Demochka.Modelss;
using System;
using System.Linq;
namespace Demochka;

public partial class UserWindow : Window
{
    public UserWindow()
    {
        InitializeComponent();
        LoadUserData();
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

    public BagotskayaContext context = new BagotskayaContext();
    public bool isUpdate;


    private void LoadUserData()
    {
        if (!AuthService.IsAuthenticated)
        {
            ShowErrorDialog("������", "������������ �� �����������");
            this.Close();
            return;
        }

        int authUserId = AuthService.CurrentUserId;
        var userObject = context.Users.FirstOrDefault(e => e.UserId == authUserId);

        if (userObject == null)
        {
            ShowErrorDialog("������", "������ ������������ �� �������");
            return;
        }

        emailBox.Text = userObject.Email;
        phoneBox.Text = userObject.Phone;
        passportDataBox.Text = userObject.PassportData;
        passwordBox.Text = userObject.UserPassword;
        fioBox.Text = userObject.Fio;
        

        // ��������� ���� �� ���������
        SetFieldsEnabled(false);
        isUpdate = false;
    }

    private void SetFieldsEnabled(bool enabled)
    {
        emailBox.IsEnabled = enabled;
        phoneBox.IsEnabled = enabled;
        passportDataBox.IsEnabled = enabled;
        passwordBox.IsEnabled = enabled;
        fioBox.IsEnabled = enabled;
    }

    private void UpdateClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        isUpdate = !isUpdate;
        SetFieldsEnabled(isUpdate);
    }

    private void SaveChanges(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (!AuthService.IsAuthenticated)
        {
            ShowErrorDialog("������", "������������ �� �����������");
            return;
        }

        string email = emailBox.Text;
        string phone = phoneBox.Text;
        string passportData = passportDataBox.Text;
        string password = passwordBox.Text;
        string fio = fioBox.Text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowErrorDialog("������ ���������!", "���� ����� � ������ ������ ���� ���������");
            return;
        }

        try
        {
            int authUserId = AuthService.CurrentUserId;
            var existingUser = context.Users.FirstOrDefault(u => u.UserId == authUserId);

            if (existingUser == null)
            {
                ShowErrorDialog("������", "������������ �� ������");
                return;
            }

            existingUser.Email = email;
            existingUser.Phone = phone;
            existingUser.PassportData = passportData;
            existingUser.UserPassword = password;
            existingUser.Fio = fio;

            context.SaveChanges();
            ShowErrorDialog("�����", "������ ������� ���������!");

            SetFieldsEnabled(false);
            isUpdate = false;
        }
        catch (Exception ex)
        {
            ShowErrorDialog("������", $"��������� ������ ��� ����������\n{ex.Message}");
        }
    }

    private void OutRegClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        AuthService.Logout();
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }

    private void TicketClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        TicketWindow ticketWindow = new TicketWindow();
        ticketWindow.Show();
        this.Close();
    }

    private void BackClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }
}