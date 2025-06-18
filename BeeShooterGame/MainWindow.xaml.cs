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



        public MainWindow()
        {
            InitializeComponent();
            
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
                        continue;
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

                // Check for collisions between bullets and enemies
                for (int i = _bullets.Count - 1; i >= 0; i--)
                {
                    Models.Bullet bullet = _bullets[i];
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



            };
        }

        private void GamOver()
        {
            _gameOver = true; // Set game over flag
            // Game over is visible to the user
            GameOverText.Visibility = Visibility.Visible;
            RestartButton.Visibility = Visibility.Visible; // Show restart button
            _gameLoopTimer.Stop(); // Stop the game loop timer
            MessageBox.Show("Game Over!"); // Show game over message
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
            LivesText.Text = $"Lives: {_playerLives}"; // Update lives display
            ScoreText.Text = $"Score: {_score}"; // Update score display
            TimeText.Text = $"Time: 00:00"; // Reset elapsed time display
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
    }
}