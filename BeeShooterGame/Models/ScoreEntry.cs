using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeShooterGame.Models
{
    public class ScoreEntry
    {
        // player name
        public string PlayerName { get; set; }
        // score value
        public int Score { get; set; }
        // timestamp of the score entry
        public DateTime Date { get; set; }
    }
}
