using System.Windows;
using System.Windows.Media;

namespace BeeShooterGame.Toast
{
    /// <summary>
    /// Interaction logic for ToastWindow.xaml
    /// </summary>
    public partial class ToastWindow : Window
    {
        public ToastWindow(string message, ToastType type, ToastPosition position)
        {
            InitializeComponent();
            DataContext = new ToastViewModel(message, type);
            Loaded += (_, _) => SetLocation(position);
        }

        private void SetLocation(ToastPosition pos)
        {
            Point pt = pos.GetPosition(Width, Height);
            Left = pt.X;
            Top = pt.Y;
        }
    }

    public class ToastViewModel
    {
        public string Message { get; }
        public Brush Background { get; }

        public ToastViewModel(string message, ToastType type)
        {
            Message = message;
            Background = type switch
            {
                ToastType.Success => Brushes.Green,
                ToastType.Warning => Brushes.Orange,
                ToastType.Error => Brushes.Red,
                _ => Brushes.SteelBlue
            };
        }
    }
}
