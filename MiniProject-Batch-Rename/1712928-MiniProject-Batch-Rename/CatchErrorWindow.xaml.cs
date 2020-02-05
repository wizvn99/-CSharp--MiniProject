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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CatchErrorWindow : Window
    {
        public static string Solution;
        public CatchErrorWindow()
        {
            InitializeComponent();
        }

        private void ErrorCaseMethodOKButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddNumber.IsChecked == true)
            {
                Solution = "Add";
                this.DialogResult = true;
                this.Close();
            }
            else if(Skip.IsChecked == true)
            {
                Solution = "Skip";
                this.DialogResult = true;
                this.Close();
            }

            this.Close();
        }
    }
}
