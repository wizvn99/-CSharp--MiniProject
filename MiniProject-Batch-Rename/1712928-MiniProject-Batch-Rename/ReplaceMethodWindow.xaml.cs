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
    /// Interaction logic for ReplaceMethodWindow.xaml
    /// </summary>
    public partial class ReplaceMethodWindow : Window
    {
        public static string replaceFrom;
        public static string replaceTo;
        public ReplaceMethodWindow()
        {
            InitializeComponent();
            
        }

        private void ReplaceMethodOKButton_Click(object sender, RoutedEventArgs e)
        {
            replaceFrom = ReplaceMethodFromString.Text;
            replaceTo = ReplaceMethodToString.Text;

            if (replaceTo == null)
            {
                replaceTo = "";
            }

            this.DialogResult = true;
            this.Close();
        }

        private void ReplaceMethodCancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
