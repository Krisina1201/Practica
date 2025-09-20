using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Demochka.Modelss;

namespace Demochka;

public partial class UserWindow : Window
{
    public UserWindow()
    {
        InitializeComponent();
    }

    public UserWindow(User? userId)
    {
        InitializeComponent();
    }

    private void UpdateClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }

    private void TicketClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }
}