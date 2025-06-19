using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeShooterGame.Models
{
    public class ScoreBoard
    {
        // List to hold score entries
        public List<ScoreEntry> Scores { get; set; }
        // Constructor to initialize the score list
        public ScoreBoard()
        {
            Scores = new List<ScoreEntry>();
        }
        // Method to add a new score entry
        public void AddScore(string playerName, int score)
        {
            Scores.Add(new ScoreEntry
            {
                PlayerName = playerName,
                Score = score,
                Date = DateTime.Now
            });
        }
        // Method to get the top scores
        public List<ScoreEntry> GetTopScores(int count)
        {
            return Scores.OrderByDescending(s => s.Score).Take(count).ToList();
        }
    }
}
