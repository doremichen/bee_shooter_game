using BeeShooterGame.Toast;
using System.IO;
using System.Text.Json;

namespace BeeShooterGame.Helpers
{
    /// <summary>
    /// Provides save and load functionality for the scoreboard JSON file.
    /// </summary>
    public static class ScoreBoardManager
    {
        // Path to the scoreboard file: "scoreboard.json"
        private const string ScoreBoardFilePath = "scoreboard.json";
        // maximum number of scores to keep: 5
        private const int MaxScores = 5;
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions { WriteIndented = true };

        /// <summary>
        /// Save the scoreboard to a JSON file.
        /// </summary>
        /// <param name="scoreBoard">The scoreboard to save.</param>
        private static void SaveScoreBoard(Models.ScoreBoard scoreBoard)
        {
            try
            {
                if (scoreBoard == null) throw new ArgumentNullException(nameof(scoreBoard));
                var json = JsonSerializer.Serialize(scoreBoard, JsonOptions);
                File.WriteAllText(ScoreBoardFilePath, json);
            }
            catch (Exception ex)
            {
                // show toast error type message
                ToastManager.Show($"Error saving scoreboard: {ex.Message}", ToastType.Error);
                Console.WriteLine($"Error saving scoreboard: {ex}");
            }
        }

        /// <summary>
        /// Load the scoreboard from a JSON file.
        /// Returns a new scoreboard if the file does not exist or on error.
        /// </summary>
        public static Models.ScoreBoard LoadScoreBoard()
        {
            try
            {
                if (!File.Exists(ScoreBoardFilePath))
                {
                    return new Models.ScoreBoard();
                }

                var json = File.ReadAllText(ScoreBoardFilePath);
                var scoreBoard = JsonSerializer.Deserialize<Models.ScoreBoard>(json, JsonOptions);
                return scoreBoard ?? new Models.ScoreBoard();
            }
            catch (Exception ex)
            {
                // show toast error type message
                ToastManager.Show($"Error loading scoreboard: {ex.Message}", ToastType.Error);
                Console.WriteLine($"Error loading scoreboard: {ex}");
                return new Models.ScoreBoard();
            }
        }

        /// <summary>
        /// Add a score to the scoreboard and persist the top scores.
        /// </summary>
        /// <param name="playerName">The name of the player.</param>
        /// <param name="score">The score to add.</param>
        public static void AddScore(string playerName, int score)
        {
            if (string.IsNullOrWhiteSpace(playerName))
            {
                playerName = "Unknown";
            }

            var scoreBoard = LoadScoreBoard();
            scoreBoard.AddScore(playerName, score);
            // Keep only the top scores
            scoreBoard.Scores = scoreBoard.GetTopScores(MaxScores);
            SaveScoreBoard(scoreBoard);
            ToastManager.Show($"Score added: {playerName} - {score}", ToastType.Success, ToastPosition.Center);
        }
    }
}
