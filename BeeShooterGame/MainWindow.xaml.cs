using BeeShooterGame.data;
using BeeShooterGame.Toast;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BeeShooterGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // DispatcherTimer for game loop
        private DispatcherTimer _gameLoopTimer;
        // Player object
        private Models.Player _player;
        // list bullet objects
        private List<Models.Bullet> _bullets = new List<Models.Bullet>();
        // list enemy objects
        private List<Models.Enemy> _enemies = new List<Models.Enemy>();
        // random number generator for enemy spawning
        private Random _random = new Random();
        // enemy spawn counter
        private int _enemySpawnCounter = 0;
        // score variable (if needed in the future)
        private int _score = 0;
        // player life variable
        private int _playerLives = 3; // Assuming player starts with 3 lives
        // game over flag
        private bool _gameOver = false;
        // elapsed time variable
        private TimeSpan _elapsedTime = TimeSpan.Zero;
        // high score variable
        private int _highScore = 0; // track high score
        // longest time survived variable
        private TimeSpan _longestTimeSurvived = TimeSpan.Zero; // track longest time survived

        // enemy speed level
        private int _enemySpeedLevel = 1; // Initial enemy speed level
        // enemy speed increment value
        private double _enemySpeedIncrement = 0.5; // Speed increment value for each level

        // save file path for high score and longest time survived
        private const string SaveFilePath = "game_record.json"; // Path to save game record



        public MainWindow()
        {
            InitializeComponent();
            // Load game record from file
            LoadGameRecord(); // Load high score and longest time survived from file
            
        }

        private void InitGame()
        {
            // Initialize player
            _player = new Models.Player
            {
                X = 200, // Initial X position
                Y = 500  // Initial Y position
            };

            // Add player sprite to the canvas
            GameCanvas.Children.Add(_player.Sprite);
            // update player position
            _player.UpdatePosition();

            // Initialize game loop timer
            _gameLoopTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16.66) // Approximately 60 FPS
            };

            // Subscribe to the game loop event
            _gameLoopTimer.Tick += GameLoop();

            // Start the game loop
            _gameLoopTimer.Start();

        }

        private EventHandler GameLoop()
        {
            return (sender, e) =>
            {
                // Check if the game is over
                if (_gameOver)
                {
                    return; // Exit the game loop if the game is over
                }

                // Update elapsed time
                _elapsedTime += _gameLoopTimer.Interval; // Increment elapsed time by the timer interval

                // Update show elapsed time in a TextBlock if needed
                TimeText.Text = $"Time: {_elapsedTime.Minutes:00}:{_elapsedTime.Seconds:00}";

                // check the enemy speed level and adjust enemy speed accordingly
                int mintesPassed = (int)_elapsedTime.TotalMinutes; // Get total minutes passed
                if (mintesPassed > _enemySpeedLevel) // Example: increase speed after 60 seconds
                {
                    // Toast message to the user about enemy speed increase
                    ToastManager.Show($"Enemy speed increased to level {mintesPassed}!", ToastType.Info,ToastPosition.Center);
                    _enemySpeedLevel = mintesPassed; // Increase enemy speed level
                    foreach (var enemy in _enemies)
                    {
                        enemy.Speed += _enemySpeedIncrement; // Increase enemy speed
                    }
                }

                // check if the player is out of bounds
                if (_player.X < 0)
                {
                    _player.X = 0; // Prevent player from going out of bounds on the left
                }
                else if (_player.X > GameCanvas.ActualWidth - _player.Sprite.Width)
                {
                    _player.X = GameCanvas.ActualWidth - _player.Sprite.Width; // Prevent player from going out of bounds on the right
                }
                if (_player.Y < 0)
                {
                    _player.Y = 0; // Prevent player from going out of bounds on the top
                }
                else if (_player.Y > GameCanvas.ActualHeight - _player.Sprite.Height)
                {
                    _player.Y = GameCanvas.ActualHeight - _player.Sprite.Height; // Prevent player from going out of bounds on the bottom
                }


                // Spawn new enemies every 100 ticks (approximately every 1.6 seconds)
                _enemySpawnCounter++;
                if (_enemySpawnCounter >= 100)
                {
                    // Reset the spawn counter
                    _enemySpawnCounter = 0;
                    // Spawn a new enemy at a random X position
                    double enemyX = _random.Next(0, (int)(GameCanvas.ActualWidth - 40)); // Assuming enemy width is 40
                    Models.Enemy newEnemy = new Models.Enemy(enemyX, 0); // Start at the top of the canvas
                    _enemies.Add(newEnemy); // Add to the list of enemies
                    GameCanvas.Children.Add(newEnemy.Sprite); // Add enemy sprite to the canvas
                    newEnemy.UpdatePosition(); // Update enemy position on canvas
                }

                // Update game state here
                // For example, you can move the player or check for collisions
                // _player.MoveLeft(); // Example of moving the player left
                // _player.UpdatePosition(); // Update player position on canvas
                // Update bullets
                for (int i = _bullets.Count - 1; i >= 0; i--)
                {
                    Models.Bullet bullet = _bullets[i];
                    bullet.MoveUp(); // Move the bullet up
                    bullet.UpdatePosition(); // Update bullet position on canvas
                    // Check if the bullet is out of bounds
                    if (bullet.Y < 0)
                    {
                        GameCanvas.Children.Remove(bullet.Sprite); // Remove bullet sprite from canvas
                        _bullets.RemoveAt(i); // Remove bullet from the list
                    }

                    for (int j = _enemies.Count - 1; j >= 0; j--)
                    {
                        Models.Enemy enemy = _enemies[j];
                        // Check if the bullet collides with the enemy
                        if (bullet.IntersectsWith(enemy))
                        {
                            // Remove both bullet and enemy from the game
                            GameCanvas.Children.Remove(bullet.Sprite);
                            GameCanvas.Children.Remove(enemy.Sprite);
                            _bullets.RemoveAt(i);
                            _enemies.RemoveAt(j);

                            // add score increment logic here if needed
                            _score += 10; // Increment score by 10 for each enemy hit (optional)
                            // update TextBlock score display if you have one
                            ScoreText.Text = $"Score: {_score}"; // Assuming you have a TextBlock named ScoreTextBlock

                            break; // Exit the inner loop since the bullet is removed
                        }
                    }
                }

                // Update enemies
                for (int i = _enemies.Count - 1; i >= 0; i--)
                {
                    Models.Enemy enemy = _enemies[i];
                    enemy.MoveDown(); // Move the enemy down
                    enemy.UpdatePosition(); // Update enemy position on canvas
                    // Check if the enemy is out of bounds
                    if (enemy.Y > GameCanvas.ActualHeight)
                    {
                        GameCanvas.Children.Remove(enemy.Sprite); // Remove enemy sprite from canvas
                        _enemies.RemoveAt(i); // Remove enemy from the list

                        // Decrease player lives if an enemy goes out of bounds
                        _playerLives--; // Decrease player lives
                        // update player lives display if you have one
                        LivesText.Text = $"Lives: {_playerLives}";

                        // Check if player has no lives left
                        if (_playerLives <= 0)
                        {
                            GamOver();
                            return; // Exit the game loop
                        }
                        else
                        {
                            continue;
                        }     
                    }

                    // Check for collision with player
                    if (_player.IntersectsWith(enemy))
                    {
                        // Handle player collision with enemy
                        _playerLives--; // Decrease player lives
                        // update player lives display if you have one
                        LivesText.Text = $"Lives: {_playerLives}";

                        GameCanvas.Children.Remove(enemy.Sprite); // Remove enemy sprite from canvas
                        _enemies.RemoveAt(i); // Remove enemy from the list
                        // Check if player has no lives left
                        if (_playerLives <= 0)
                        {
                            GamOver();
                            return; // Exit the game loop
                        }
                    }
                }

            };
        }

        private void GamOver()
        {
            _gameOver = true; // Set game over flag

            UpdateGameRecord();

            // Save game record to a file
            SaveGameRecord(); // Save high score and longest time survived to file

            // Game over is visible to the user
            GameOverText.Visibility = Visibility.Visible;
            RestartButton.Visibility = Visibility.Visible; // Show restart button
            _gameLoopTimer.Stop(); // Stop the game loop timer
            MessageBox.Show("Game Over!"); // Show game over message
        }

        /**
         * Update game record based on current score and elapsed time
         */
        private void UpdateGameRecord()
        {
            // Check if the current time survived is longer than the longest time survived
            if (_elapsedTime > _longestTimeSurvived)
            {
                _longestTimeSurvived = _elapsedTime; // Update longest time survived
                // Optionally, you can save the longest time to a file or database

                // Display longest time survived to the user
                LongestTimeText.Text = $"Longest Time: {_longestTimeSurvived.Minutes:00}:{_longestTimeSurvived.Seconds:00}";
            }

            // Check if the current score is higher than the high score
            if (_score > _highScore)
            {
                _highScore = _score; // Update high score
                // Optionally, you can save the high score to a file or database

                // Display high score to the user
                HighScoreText.Text = $"High Score: {_highScore}";
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the game is over
            if (_gameOver)
            {
                return; // Ignore key presses if the game is over
            }


            // Handle player movement based on key presses
            switch (e.Key)
            {
                    case Key.Left:
                        _player.MoveLeft();
                        break;
                    case Key.Right:
                        _player.MoveRight();
                        break;
                    case Key.Up:
                        _player.MoveUp();
                        break;
                    case Key.Down:
                        _player.MoveDown();
                        break;
                    case Key.Space:
                        Shoot();
                        break;
                    // Add more cases for other keys if needed
            }
            // Update player position after movement
            _player.UpdatePosition();

        }

        private void Shoot()
        {
            // Create a new bullet at the player's position
            Models.Bullet bullet = new Models.Bullet(_player.X + _player.Sprite.Width / 2 - 4, _player.Y - 20);
            _bullets.Add(bullet);
            // Add bullet sprite to the canvas
            GameCanvas.Children.Add(bullet.Sprite);
            // update bullet position
            bullet.UpdatePosition();

            // paly sound effect for shooting
            var soundPlayer = new System.Media.SoundPlayer("Resources/gun-gunshot-01.wav");
            soundPlayer.Play();

        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset game state
            _gameOver = false;
            _playerLives = 3; // Reset player lives
            _score = 0; // Reset score
            _elapsedTime = TimeSpan.Zero; // Reset elapsed time

            // Update UI elements
            InitUI();
            // Clear existing bullets and enemies
            _bullets.Clear();
            _enemies.Clear();
            GameCanvas.Children.Clear(); // Clear the canvas
            // Reinitialize the game
            InitGame();
            // Hide game over text and restart button
            GameOverText.Visibility = Visibility.Hidden;
            RestartButton.Visibility = Visibility.Hidden;

        }

        private void InitUI()
        {
            LivesText.Text = $"Lives: {_playerLives}"; // Update lives display
            ScoreText.Text = $"Score: {_score}"; // Update score display
            TimeText.Text = $"Time: 00:00"; // Reset elapsed time display
            LongestTimeText.Text = $"Longest Time: {_longestTimeSurvived.Minutes:00}:{_longestTimeSurvived.Seconds:00}"; // Reset longest time survived display
            HighScoreText.Text = $"High Score: {_highScore}"; // Reset high score display
        }

        /**
         * Save game record to a json file
         */
        private void SaveGameRecord()
        {             
            var gameRecord = new
            {
                HighScore = _highScore,
                LongestGameDuration = _longestTimeSurvived // Format as mm:ss
            };
            // Serialize to JSON and save to file
            string json = System.Text.Json.JsonSerializer.Serialize(gameRecord);
            System.IO.File.WriteAllText(SaveFilePath, json);
        }

        /**
         * Load game record from a json file
         */
        private void LoadGameRecord()
        {
            if (System.IO.File.Exists(SaveFilePath))
            {
                string json = System.IO.File.ReadAllText(SaveFilePath);
                var gameRecord = System.Text.Json.JsonSerializer.Deserialize<GameRecord>(json);
                _highScore = gameRecord.HighScore;
                _longestTimeSurvived = gameRecord.LongestGameDuration; // Directly assign TimeSpan property
                // Update UI with loaded values
                HighScoreText.Text = $"High Score: {_highScore}";
                LongestTimeText.Text = $"Longest Time: {_longestTimeSurvived.Minutes:00}:{_longestTimeSurvived.Seconds:00}";
            }
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            // Hide the start button and show the game canvas
            MainMenuPanel.Visibility = Visibility.Collapsed; // Hide the main menu panel
            GameCanvas.Visibility = Visibility.Visible; // Show the game canvas
            // Initialize the game
            InitGame(); // Start the game
        }

        private void ExitGame_Click(object sender, RoutedEventArgs e)
        {
            // Close the application when the exit button is clicked
            Application.Current.Shutdown(); // Close the application
        }

        private void Instruction_Click(object sender, RoutedEventArgs e)
        {
            // Show instructions in a message box or a new window
            MessageBox.Show("Instructions:\n" +
                            "Use the left and right arrow keys to move the player.\n" +
                            "Press the spacebar to shoot bullets.\n" +
                            "Avoid enemies and try to survive as long as possible.\n" +
                            "Good luck!", "Game Instructions", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /**
         * Exit button click event handler
         */
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Show confirmation dialog before exiting
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit the game?", "Exit Game", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // update game record before exiting
                UpdateGameRecord(); // Update high score and longest time survived
                SaveGameRecord(); // Save high score and longest time survived to file

                // stop the game loop timer if it's running
                if (_gameLoopTimer != null && _gameLoopTimer.IsEnabled)
                {
                    _gameLoopTimer.Stop(); // Stop the game loop timer
                }

                Application.Current.Shutdown(); // Close the application
            }
        }
    }
}