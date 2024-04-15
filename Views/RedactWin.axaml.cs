using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySqlConnector;
using SuperSayan.Models;

namespace SuperSayan.Views;

public partial class RedactWin : Window
{
    private readonly Player _player;
    private MySqlConnectionStringBuilder _ConnectionSB = new MySqlConnectionStringBuilder();
    public RedactWin(Player player)
    {
        _player = player;
        InitializeComponent();
        _ConnectionSB = new MySqlConnectionStringBuilder
        {
            Server = "localhost", 
            Database = "pro_11",
            UserID = "user_01", 
            Password = "user01pro",
        };
    }

    private void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        {
            string sql;
            sql = """
                  update players where PlayerID = @player
                  ( position, playername, weight, height, birthdate, begindate, team)
                  VALUES
                      (@pos, @planam, @we, @he, @bida, @beda, @t)
                  """;
            var posItem = Pos.SelectedItem as Position;
            var tItem = Team.SelectedItem as Team;
            using (var con = new MySqlConnection(_ConnectionSB.ConnectionString))
            {
                con.Open();
                using (var command = con.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@pos", posItem.PositionID);
                    command.Parameters.AddWithValue("@t", tItem.TeamID);
                    command.Parameters.AddWithValue("@planam", newPlayerName.Text);
                    command.Parameters.AddWithValue("@we", newWeight.Text);
                    command.Parameters.AddWithValue("@he", newWeight.Text);
                    command.Parameters.AddWithValue("@bida", BirthPicker.SelectedDate);
                    command.Parameters.AddWithValue("@beda", BeginPicker.SelectedDate);
                    command.Parameters.AddWithValue("@player", _player.PlayerID);
                    command.ExecuteNonQuery();
                }
                con.Close();
            }
        }
    }
}