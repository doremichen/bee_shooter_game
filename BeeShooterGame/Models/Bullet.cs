using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BeeShooterGame.Models
{
    public class Bullet : GameObject
    {
        // Construcytor with startX andt startY parameters
        public Bullet(double startX, double startY)
        {
            // Initialize bullet properties
            Sprite = new Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/bullet.png")),
                Width = 8, // Set appropriate width for the bullet
                Height = 20 // Set appropriate height for the bullet
            };
            X = startX; // Set initial X position
            Y = startY; // Set initial Y position
            Speed = 10; // Set bullet speed
        }

        // MoveUp method to move the bullet upwards
        public void MoveUp()
        {
            Y -= Speed; // Move the bullet up by its speed
            UpdatePosition(); // Update the position of the bullet on the canvas

        }
    }
}
