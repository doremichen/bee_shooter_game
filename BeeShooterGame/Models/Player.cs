using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BeeShooterGame.Models
{
    public class Player : GameObject
    {
        public Player() 
        {
            // Initialize player properties
            Sprite = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Resources/player.png")),
                Width = 80, // Set appropriate width
                Height = 80 // Set appropriate height
            };
            Speed = 5; // Set player speed
        }

        public void MoveLeft()
        {
            X -= Speed;
            UpdatePosition();
        }

        public void MoveRight()
        {
            X += Speed;
            UpdatePosition();
        }

    }
}
