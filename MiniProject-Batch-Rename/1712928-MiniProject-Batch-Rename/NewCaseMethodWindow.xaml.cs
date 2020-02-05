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
    /// Interaction logic for NewCaseMethodWindow.xaml
    /// </summary>
    public partial class NewCaseMethodWindow : Window
    {
        public static string newcaseFrom = "this";
        public static string newcaseTo;

        public NewCaseMethodWindow()
        {
            InitializeComponent();
        }

        private void NewCaseMethodOKButton_Click(object sender, RoutedEventArgs e)
        {
         
            if(UpperCaseAllLetters.IsChecked==true)
            {
                newcaseTo = "upper";
            }
            if (LowerCaseAllLetters.IsChecked == true)
            {
                newcaseTo = "lower";
            }

            if (UpperCaseFirstLetter.IsChecked == true)
            {
                newcaseTo = "firstChar";
            }
            this.DialogResult = true;
            this.Close();
        }

        private void NewCaseMethodCancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
