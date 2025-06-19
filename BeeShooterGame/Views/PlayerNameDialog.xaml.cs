using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BeeShooterGame.Views
{
    /// <summary>
    /// Interaction logic for PlayerNameDialog.xaml
    /// </summary>
    public partial class PlayerNameDialog : Window
    {
        // player name
        public string PlayerName { get; private set; }


        public PlayerNameDialog()
        {
            InitializeComponent();
            // auto focus on the TextBox for player name input
            NameTextBox.Focus();
            // select default text in the TextBox
            NameTextBox.SelectAll();

        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Submit();
        }

        private bool Submit()
        {
            // get the player name from the TextBox
            PlayerName = NameTextBox.Text.Trim();
            // check if the player name is not empty
            if (string.IsNullOrWhiteSpace(PlayerName))
            {
                // show a message box if the player name is empty
                MessageBox.Show("Please enter a valid player name.", "Invalid Name", MessageBoxButton.OK, MessageBoxImage.Warning);
                // focus back on the TextBox for player name input
                NameTextBox.Focus();
                // select all text in the TextBox
                NameTextBox.SelectAll();
                return false;
            }

            // close the dialog with OK result
            DialogResult = true;
            // close the dialog window
            Close();
            return true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // player name is null
            PlayerName = null;
            // close the dialog with Cancel result
            DialogResult = false;
            // close the dialog window
            Close();
        }

        private void NameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // press Enter key to submit the player name
            if (e.Key == Key.Enter)
            {
                e.Handled = true; // prevent the beep sound on Enter key press
                Submit(); // call the Submit method to process the player name
            }
        }
    }
}
