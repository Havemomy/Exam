using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace SuperSayan.Views;

public partial class EnterWin : Window
{
    public static int Role; 
    public EnterWin()
    {
        InitializeComponent();
    }



    private void AdmButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Role = 1;
        var MainWin = new MainWin();
        MainWin.Show();
        this.Close();
    }

    private void ManButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Role = 2;
        var MainWin = new MainWin();
        MainWin.Show();
        this.Close();
    }
}