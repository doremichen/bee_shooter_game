using BeeShooterGame.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BeeShooterGame.Views
{
    /// <summary>
    /// Interaction logic for LeaderboardWindow.xaml
    /// </summary>
    public partial class LeaderboardWindow : Window
    {
        public LeaderboardWindow()
        {
            InitializeComponent();

            // Load the leaderboard data when the window is initialized
            LoadLeaderboardData();
        }

        private void LoadLeaderboardData()
        {
            // load the leaderboard data from the ScoreBoard model
            var scoreBoard = ScoreBoardManager.LoadScoreBoard();
            // new list to hold the score entries
            var rankedList = scoreBoard.Scores.Select((entry, index) => new
            {
                Rank = index + 1,
                entry.PlayerName,
                entry.Score,
                entry.Date
            }).ToList();

            // bind the ranked list to the DataGrid
            LeaderboardDataGrid.ItemsSource = rankedList;

        }
    }
}
