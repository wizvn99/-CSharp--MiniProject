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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Sql;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.ComponentModel;

namespace _1712928_ADO.Net
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Entity class - Contains data only
        /// 
        /// </summary>
        class Customer
        {
            public string Fullname { get; set; }
            public string Tel { get; set; }
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            // Ket qua cua viec doc du lieu
            var list = new BindingList<Customer>();

            // Buoc 1: Mo ket noi
            var connectionString = "Server= localhost;Database= mycompany ;User Id=root ;password=12345";
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                MessageBox.Show("Sever is connected!");
                // Buoc 2 - Chuan bi cau truy van
                // Lay danh sach cac customer
                var sql = "SELECT * FROM mycompany.customer;";

                // Buoc 3 - Thuc thi cau truy van
                var command = new MySqlCommand(sql, connection);
                MySqlDataReader reader = command.ExecuteReader();


                // Buoc 4- Xu li ket qua tra ve
                while(reader.Read())
                {
                    var customer = new Customer();
                    customer.Fullname = reader.GetString("Fullname"); ;
                    customer.Tel = reader.GetString("Tel"); ;
                    list.Add(customer);
                }

                // Buoc 5: Dong ket noi
                connection.Close();
            }
            catch 
            {
            }

            customersListBox.ItemsSource = list;


        }
    }
}
