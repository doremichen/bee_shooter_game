using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeShooterGame.Models
{
    public class Enemy : GameObject
    {
        //Constructor with startX and startY parameters
        public Enemy(double startX, double startY)
        {
            // Initialize enemy properties
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/enemy.png")),
                Width = 40, // Set appropriate width for the enemy
                Height = 40 // Set appropriate height for the enemy
            };
            X = startX; // Set initial X position
            Y = startY; // Set initial Y position
            Speed = 2; // Set enemy speed
        }

        // MoveDown method to move the enemy downwards
        public void MoveDown()
        {
            Y += Speed; // Move the enemy down by its speed
            UpdatePosition(); // Update the position of the enemy on the canvas
        }
    }
}
