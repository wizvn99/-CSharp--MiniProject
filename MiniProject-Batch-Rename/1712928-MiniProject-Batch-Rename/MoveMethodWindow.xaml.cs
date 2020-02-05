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

namespace _1712928_MiniProject_Batch_Rename
{
    /// <summary>
    /// Interaction logic for MoveMethodWindow.xaml
    /// </summary>
    public partial class MoveMethodWindow : Window
    {
        public static string moveNChars;
        public static string fromto;
        public MoveMethodWindow()
        {
            InitializeComponent();
            
        }

        private void MoveMethodOKButton_Click(object sender, RoutedEventArgs e)
        {
            int value,check=0;

            if (MoveMethodNCharacters.Text == String.Empty)
            {
                MessageBox.Show("Bạn chưa điền N.");
                check = 1;
                
            }

            else if (int.TryParse(MoveMethodNCharacters.Text, out value)==false)
            {
                MessageBox.Show("N là số ký tự muốn chuyển, N phải là số.");
                check = 1;
                
            }
            
            moveNChars = MoveMethodNCharacters.Text;

            if (FromEndToFrontCheck.IsChecked==true)
            {
                fromto = "fromEndtoFront";
            }

            if (FromFrontToEndCheck.IsChecked == true)
            {
                fromto = "fromFronttoEnd";
            }
            if (check == 0)
            {
                this.DialogResult = true;
                this.Close();
            }
        }

        private void MoveMethodCancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
