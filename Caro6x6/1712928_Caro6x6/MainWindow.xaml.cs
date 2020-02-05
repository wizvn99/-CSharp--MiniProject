using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace _1712928_Caro6x6
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
        int[,] _a;
        Button[,] _buttons;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            const int Rows = 6;
            const int Cols = 6;
            const int ButtonWidth = 70;
            const int ButtonHeight = 70;
            const int Padding = 10;
            const int TopOffset = 50;
            const int LeftOffset = 50;
            //j la cols, i là rows
            // Model - Tao ra ma tran nut bam 3x3
            _a = new int[Rows, Cols];
            _buttons = new Button[Rows, Cols];

            // UI
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    var button = new Button();
                    button.Width = ButtonWidth;
                    button.Height = ButtonHeight;
                    button.Tag = new Tuple<int, int>(i, j);
                    button.Click += Button_Click;

                    // Dua vao model quan li UI
                    _buttons[i, j] = button; // Luu tham chieu toi button

                    // Dua vao UI
                    uiCanvas.Children.Add(button);
                    Canvas.SetLeft(button, LeftOffset + j * (ButtonWidth + Padding));
                    Canvas.SetTop(button, TopOffset + i * (ButtonHeight + Padding));
                }
            }
        }

        bool isXTurn = true;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var (i, j) = button.Tag as Tuple<int, int>;
            
            if (_a[i, j] == 0)
            {
                if (isXTurn)
                {
                    button.Content = "X"; 
                    _a[i, j] = 1; 

                }
                else
                {
                    button.Content = "O"; // UI
                    _a[i, j] = 2;
                }
                isXTurn = !isXTurn; // Model / State

                var (gameOver, xWin) = checkWin(_a, i, j);

                if (gameOver)
                {
                    if (xWin)
                    {
                        MessageBox.Show("X won!!!");
                    }
                    else
                    {
                        MessageBox.Show("O won!!!");
                    }
                    for(int a = 0; a < 6; a++)
                    {
                        for (int b = 0; b < 6; b++)
                        {
                            _a[a, b] = 0;
                            _buttons[a, b].Content = "";
                        }
                    }
                }

            }


        }

        private (bool, bool) checkWin(int[,] a, int i, int j)
        {
            const int WinCondition = 5;           

            var xWin = false;
            var gameOver = false;

            // Check win ngang
            // Loang ben trai
            int dj = -1; // Di qua trai
            int startJ = j;
            var count = 1;
            while (-1 != startJ + dj) // Con duoc ben trai
            {
                startJ += dj; // Di qua trai
                if (_a[i, j] == a[i, startJ]) // Tang bien dem
                {
                    count++;
                }
                else break;
            }

            startJ = j;
            dj = 1;
            while (startJ + dj != 6)
            {
                startJ += dj; // Di qua phai
                if (_a[i, j] == a[i, startJ]) // Tang bien dem
                {
                    count++;
                }
                else break;
            }

            if (count >= WinCondition)
            {
                gameOver = true;
                xWin = _a[i, j] == 1;
                return (gameOver, xWin);
            }

            // Check win doc
            int di = -1; // Di len tren
            int startI = i;
            count = 1;
            while (-1 != startI + di) 
            {
                startI += di; 
                if (_a[i, j] == a[startI,j])
                {
                    count++;
                }
                else break;
            }

            startI = i;
            di = 1;
            while (startI + di != 6)
            {
                startI += di; // Di qua phai
                if (_a[i, j] == a[startI,j]) // Tang bien dem
                {
                    count++;
                }
                else break;
            }

            if (count >= WinCondition)
            {
                gameOver = true;
                xWin = _a[i, j] == 1;
                return (gameOver, xWin);
            }

            // Check win cheo      
            //cheo tu trai qua phai
            di = -1;
            dj = -1; 
            startI = i;
            startJ = j;
            count = 1;
            while (-1 != startI + di && -1 != startJ +dj)
            {
                startI += di;
                startJ += dj;
                if (_a[i, j] == a[startI, startJ])
                {
                    count++;
                }
                else break;
            }

            di = 1;
            dj = 1;
            startI = i;
            startJ = j;
            while (startI + di != 6 && startJ + dj !=6)
            {
                startI += di;
                startJ += dj;
                if (_a[i, j] == a[startI, startJ]) // Tang bien dem
                {
                    count++;
                }
                else break;
            }

            if (count >= WinCondition)
            {
                gameOver = true;
                xWin = _a[i, j] == 1;
            return (gameOver, xWin);
            }

            //cheo tu phai qua trai
            di = -1;
            dj = 1;
            startI = i;
            startJ = j;
            count = 1;
            while (-1 != startI + di && 6 != startJ + dj)
            {
                startI += di;
                startJ += dj;
                if (_a[i, j] == a[startI, startJ])
                {
                    count++;
                }
                else break;
            }

            di = 1;
            dj = -1;
            startI = i;
            startJ = j;
            while (startI + di != 6 && startJ + dj != -1)
            {
                startI += di;
                startJ += dj;
                if (_a[i, j] == a[startI, startJ]) // Tang bien dem
                {
                    count++;
                }
                else break;
            }

            if (count >= WinCondition)
            {
                gameOver = true;
                xWin = _a[i, j] == 1;
            }
            return (gameOver, xWin);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            const string filename = "save.txt";

            var writer = new StreamWriter(filename);
            // Dong dau tien la luot di hien tai
            writer.WriteLine(isXTurn ? "X" : "O");

            // Theo sau la ma tran bieu dien game
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    writer.Write($"{_a[i, j]}");
                    if (j != 5)
                    {
                        writer.Write(" ");
                    }
                }
                writer.WriteLine("");
            }

            writer.Close();

            MessageBox.Show("Game is saved");
        }

        private void LoadMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            if (screen.ShowDialog() == true)
            {
                var filename = screen.FileName;

                var reader = new StreamReader(filename);
                var firstLine = reader.ReadLine();
                isXTurn = firstLine == "X";

                for (int i = 0; i < 6; i++)
                {
                    var tokens = reader.ReadLine().Split(
                        new string[] { " " }, StringSplitOptions.None);
                    // Model

                    for (int j = 0; j < 6; j++)
                    {
                        _a[i, j] = int.Parse(tokens[j]);

                        if (_a[i, j] == 1)
                        {
                            _buttons[i, j].Content = "X";
                        }

                        if (_a[i, j] == 2)
                        {
                            _buttons[i, j].Content = "O";
                        }
                    }
                }
                MessageBox.Show("Game is loaded");
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            for (int a = 0; a < 6; a++)
            {
                for (int b = 0; b < 6; b++)
                {
                    _a[a, b] = 0;
                    _buttons[a, b].Content = "";
                }
            }
        }
    }
}

