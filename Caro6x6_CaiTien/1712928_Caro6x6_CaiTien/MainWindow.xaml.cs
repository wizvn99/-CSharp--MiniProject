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

namespace _1712928_Caro6x6_CaiTien
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
        const int Rows = 6;
        const int Cols = 6;
        const int startX = 30;
        const int startY = 30;
        const int width = 50;
        const int height = 50;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            _a = new int[Rows, Cols];

            //ve gach ngang
            for (int i = 0; i <= Rows; i++)
            {
                var line1 = new Line();
                line1.StrokeThickness = 1;
                line1.Stroke = new SolidColorBrush(Colors.Red);
                uiCanvas.Children.Add(line1);

                line1.X1 = startX + i * width;
                line1.Y1 = startY;

                line1.X2 = startX + i * width;
                line1.Y2 = startY + 6 * height;
            }

            //Ve ganh doc
            for (int i = 0; i <= Cols; i++)
            {
                var line2 = new Line();
                line2.StrokeThickness = 1;
                line2.Stroke = new SolidColorBrush(Colors.Red);
                uiCanvas.Children.Add(line2);

                line2.X1 = startX;
                line2.Y1 = startY + i * height;

                line2.X2 = startX + 6 * width;
                line2.Y2 = startY + i * height;
            }

        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);


        }

        bool isXTurn = true;
        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(this);
            int i = ((int)position.Y - startY) / height;
            int j = ((int)position.X - startX) / width;
            if (position.Y <= startY || position.X <= startX || i >= Rows || j >= Cols)
            {
            }
            else
            {
                if (_a[i, j] == 0)
                {
                    if (isXTurn)
                    {
                        var imgX = new Image();
                        imgX.Width = 50;
                        imgX.Height = 50;
                        imgX.Source = new BitmapImage(
                            new Uri("x.png", UriKind.Relative));
                        uiCanvas.Children.Add(imgX);

                        Canvas.SetLeft(imgX, startX + j * width);
                        Canvas.SetTop(imgX, startY + i * height);
                        _a[i, j] = 1;
                    }
                    else
                    {
                        var imgO = new Image();
                        imgO.Width = 50;
                        imgO.Height = 50;
                        imgO.Source = new BitmapImage(
                            new Uri("o.png", UriKind.Relative));
                        uiCanvas.Children.Add(imgO);

                        Canvas.SetLeft(imgO, startX + j * width);
                        Canvas.SetTop(imgO, startY + i * height);
                        _a[i, j] = 2;
                    }
                    isXTurn = !isXTurn;
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
                        for (int a = 0; a < 6; a++)
                        {
                            for (int b = 0; b < 6; b++)
                            {
                                _a[a, b] = 0;
                                var imgW = new Image();
                                imgW.Width = 50;
                                imgW.Height = 50;
                                imgW.Source = new BitmapImage(
                                    new Uri("white.png", UriKind.Relative));
                                uiCanvas.Children.Add(imgW);                                
                                Canvas.SetLeft(imgW, startX + a * width);
                                Canvas.SetTop(imgW, startY + b * height);
                                this.Window_Loaded(sender, e);
                            }
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
                if (_a[i, j] == a[startI, j])
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
                if (_a[i, j] == a[startI, j]) // Tang bien dem
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
            while (-1 != startI + di && -1 != startJ + dj)
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
            while (startI + di != 6 && startJ + dj != 6)
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
                            var imgX = new Image();
                            imgX.Width = 50;
                            imgX.Height = 50;
                            imgX.Source = new BitmapImage(
                                new Uri("x.png", UriKind.Relative));
                            uiCanvas.Children.Add(imgX);

                            Canvas.SetLeft(imgX, startX + j * width);
                            Canvas.SetTop(imgX, startY + i * height);
                        }

                        if (_a[i, j] == 2)
                        {
                            var imgO = new Image();
                            imgO.Width = 50;
                            imgO.Height = 50;
                            imgO.Source = new BitmapImage(
                                new Uri("o.png", UriKind.Relative));
                            uiCanvas.Children.Add(imgO);

                            Canvas.SetLeft(imgO, startX + j * width);
                            Canvas.SetTop(imgO, startY + i * height);
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
                    var imgW = new Image();
                    imgW.Width = 50;
                    imgW.Height = 50;
                    imgW.Source = new BitmapImage(
                        new Uri("white.png", UriKind.Relative));
                    uiCanvas.Children.Add(imgW);
                    Canvas.SetLeft(imgW, startX + a * width);
                    Canvas.SetTop(imgW, startY + b * height);
                    this.Window_Loaded(sender, e);
                }
            }
        }
    }
}