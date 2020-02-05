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
    /// Interaction logic for UniqueMethodWindow.xaml
    /// </summary>
    public partial class UniqueMethodWindow : Window
    {
        public static string UniqueFrom = "this";
        public static string UniqueTo;

        public UniqueMethodWindow()
        {
            InitializeComponent();
        }

        private void UniqueMethodOKButton_Click(object sender, RoutedEventArgs e)
        {
         
            if(UniqueNameCheck.IsChecked==true)
            {
                UniqueTo = "Unique";
                this.DialogResult = true;
                this.Close();
            }

            this.Close();
            
        }

        private void UniqueMethodCancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
