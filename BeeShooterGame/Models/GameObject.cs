using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BeeShooterGame.Models
{
    public abstract class GameObject
    {
        // Image sprite
        public Image Sprite { get; set; }
        // X position
        public double X { get; set; }
        // Y position
        public double Y { get; set; }
        // Speed of movement
        public double Speed { get; set; }

        /**
         * updatePosition method to be implemented by derived classes
         */
        public void UpdatePosition()
        {
            Canvas.SetLeft(Sprite, X);
            Canvas.SetTop(Sprite, Y);
        }

        // GetRound method to get the bounding rectangle of the game object
        public virtual System.Windows.Rect GetBounds()
        {
            return new System.Windows.Rect(X, Y, Sprite.Width, Sprite.Height);
        }

        // IntersectsWith method to check intersection with another game object
        public bool IntersectsWith(GameObject other)
        {
            return GetBounds().IntersectsWith(other.GetBounds());

        }
    }
}
