using BeeShooterGame.Toast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BeeShooterGame.Helpers
{
    /**
     * Provide save and load functionality for the scoreboard from jason file
     */
    public class ScoreBoardManager
    {
        // Path to the scoreboard file: "scoreboard.json"
        private const string ScoreBoardFilePath = "scoreboard.json";
        // maximum number of scores to keep: 5
        private const int MaxScores = 5;

        /**
         * Save the scoreboard to a JSON file
         * @param scoreBoard The scoreboard to save
         */
        private static void SaveScoreBoard(Models.ScoreBoard scoreBoard)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(scoreBoard, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(ScoreBoardFilePath, json);
            }
            catch (Exception ex)
            {
                // show toast error type message
                ToastManager.Show("Error saving scoreboard: " + ex.Message, ToastType.Error);
                Console.WriteLine($"Error saving scoreboard: {ex.Message}");
            }
        }

        /**
         * Load the scoreboard from a JSON file
         * @return The loaded scoreboard or a new scoreboard if the file does not exist
         */
        public static Models.ScoreBoard LoadScoreBoard()
        {
            try
            {
                if (System.IO.File.Exists(ScoreBoardFilePath))
                {
                    var json = System.IO.File.ReadAllText(ScoreBoardFilePath);
                    var scoreBoard = System.Text.Json.JsonSerializer.Deserialize<Models.ScoreBoard>(json);
                    return scoreBoard ?? new Models.ScoreBoard();
                }
                else
                {
                    return new Models.ScoreBoard(); // Return a new scoreboard if the file does not exist
                }
            }
            catch (Exception ex)
            {
                // show toast error type message
                ToastManager.Show("Error loading scoreboard: " + ex.Message, ToastType.Error);
                Console.WriteLine($"Error loading scoreboard: {ex.Message}");
                return new Models.ScoreBoard(); // Return a new scoreboard in case of error
            }   
        }

        /**
         * Add a score to the scoreboard and save it
         * @param playerName The name of the player
         * @param score The score to add
         */
        public static void AddScore(string playerName, int score)
        {
            var scoreBoard = LoadScoreBoard();
            scoreBoard.AddScore(playerName, score);
            // Keep only the top scores
            scoreBoard.Scores = scoreBoard.GetTopScores(MaxScores);
            SaveScoreBoard(scoreBoard);
            // Show a toast message
            ToastManager.Show($"Score added: {playerName} - {score}", ToastType.Success, ToastPosition.Center);
        }
    }
}
