using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeShooterGame.data
{
    public class GameRecord
    {
        // High score property
        public int HighScore { get; set; }
        // Longest game duration property
        public TimeSpan LongestGameDuration { get; set; }
    }
}
