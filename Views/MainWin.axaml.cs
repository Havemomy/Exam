using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySqlConnector;
using SuperSayan.Models;

namespace SuperSayan.Views;

public partial class MainWin : Window
{
    private MySqlConnectionStringBuilder _ConnectionSB = new MySqlConnectionStringBuilder();
    private List<Player> _players;
    private List<Position> _positions;
    public MainWin()
    {
        InitializeComponent();

        if (EnterWin.Role == 1)
        {
            AddButton.IsVisible = false;
            AddButton2.IsVisible = false;
            AddButton3.IsVisible = false;
        }
        else
        {
            RedButton.IsVisible = false;
            RedButton2.IsVisible = false;
            RedButton3.IsVisible = false;
            DelButton.IsVisible = false;
            DelButton2.IsVisible = false;
            DelButton3.IsVisible = false;
        }
        _ConnectionSB = new MySqlConnectionStringBuilder
        {
            Server = "localhost", 
            Database = "pro_11",
            UserID = "user_01", 
            Password = "user01pro",
        };
        FillTable();
        FillingCombobox();
    }

    public void FillTable()
    {
        _players = new List<Player>();
        string sql;
        sql = """
              select  PositionID, PositionName, PlayerName, Weight, Height, BirthDate, BeginDate, TeamID, TeamName from players
              join supersayan.positions p on p.PositionID = players.Position
              join supersayan.teams t on t.TeamID = players.Team
              """;
        using (var con = new MySqlConnection(_ConnectionSB.ConnectionString))
        {
            con.Open();
            using (var command = con.CreateCommand())
            {
                command.CommandText = sql;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _players.Add(new Player()
                            {
                                PlayernName = reader.GetString("PlayerName"),
                                Weight = reader.GetDecimal("Weight"),
                                Height = reader.GetDecimal("Height"),
                                BirthDate = reader.GetDateOnly("BirthDate"),
                                BeginDate = reader.GetDateOnly("BeginDate"),
                                Position = reader.GetInt32("PositionID"),
                                PositionName  = reader.GetString("PlayerName"),
                                TeamName  = reader.GetString("TeamName")
                            }
                        );
                    }
                    con.Close();
                }
            }
            DataGrid.ItemsSource = _players;
        }
    }

    public void FillingCombobox()
    {
        _positions = new List<Position>();
        string sql;
        sql = """
                select * from positions
              """;
        using (var con = new MySqlConnection(_ConnectionSB.ConnectionString))
        {
            con.Open();
            using (var command = con.CreateCommand())
            {
                command.CommandText = sql;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _positions.Add(new Position()
                            {
                                PositionID = reader.GetInt32("PositionID"),
                                PositionName = reader.GetString("PositionName"),
                            }
                        );
                    }
                    con.Close();
                }
            }
            
            FillComboBox.ItemsSource = _positions;
        }
    }

    public void Search()
    {
        var searchTextInfo = _players.Where(x =>
            x.PlayernName.Contains(SerTextBox.Text, StringComparison.OrdinalIgnoreCase)).ToList();
        DataGrid.ItemsSource = searchTextInfo;
    }

    private void SerTextBox_OnTextChanging(object? sender, TextChangingEventArgs e)
    {
        Search();
    }

    private void RedButton_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void DelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        string sql;
        sql = """ delete from players where PlayerID = @player """;
        var player = DataGrid.SelectedItem as Player;
        using (var con = new MySqlConnection(_ConnectionSB.ConnectionString))
        {
            con.Open();
            using (var command = con.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.AddWithValue("player", player.PlayerID);
                command.ExecuteNonQuery();
            }
            con.Close();
        }
        _players.Remove(player);
        DataGrid.ItemsSource = _players;
        Sucsess();
    }

    private void AddButton_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    public  async void Sucsess()
    {
        Label.IsVisible = true;
        await Task.Delay(TimeSpan.FromSeconds(1));
        Label.IsVisible = false;
    }
}