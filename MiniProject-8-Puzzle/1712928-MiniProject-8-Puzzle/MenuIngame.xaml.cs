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

namespace _1712928_MiniProject_8_Puzzle
{
    /// <summary>
    /// Interaction logic for MenuIngame.xaml
    /// </summary>
    public partial class MenuIngame : Window
    {
        public static string menuReturn;
        public MenuIngame()
        {
            InitializeComponent();
        }

        
        private void ImageModeButton_Click(object sender, RoutedEventArgs e)
        {
            menuReturn = "Image";
            this.DialogResult = true;
            this.Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            menuReturn = "Exit";
            this.DialogResult = true;
            this.Close();
        }

        private void LoadGame_Click(object sender, RoutedEventArgs e)
        {
            menuReturn = "Load";
            this.DialogResult = true;
            this.Close();
        }
    }
}
