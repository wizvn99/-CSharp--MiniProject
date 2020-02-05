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
    /// Interaction logic for FullNameMethodWindow.xaml
    /// </summary>
    public partial class FullNameMethodWindow : Window
    {
        public static string fullnameFrom = "this";
        public static string fullnameTo;

        public FullNameMethodWindow()
        {
            InitializeComponent();
        }

        private void FullNameMethodOKButton_Click(object sender, RoutedEventArgs e)
        {
         
            if(FullNameNormalizeCheck.IsChecked==true)
            {
                fullnameTo = "fullname";
                this.DialogResult = true;
                this.Close();
            }

            this.Close();
            
        }

        private void FullNameMethodCancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
