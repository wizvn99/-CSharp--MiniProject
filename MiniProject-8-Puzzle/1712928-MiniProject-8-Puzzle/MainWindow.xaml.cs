using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace _1712928_MiniProject_8_Puzzle
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
        
        const int startX = 80;
        const int startY = 60;
        const int width = 50;
        const int height = 50;
        int imgWidth;
        int imgHeight;
        bool _isGamePLaying = false;
        List<Image> listCropImg = new List<Image>();
        DispatcherTimer dt;
        int timeCountdown = 180;
        string filePath;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Rules.Text = "Cách chơi:\nBạn có thể sử dụng thao tác kéo thả để di chuyển các mảnh ghép đến vị trí mới" +
                " hoặc sử dụng 4 phím mũi tên để di chuyển các mảnh ghép.\n" +
                "Lưu ý: Chỉ có thể di chuyển các mảnh ghép sát bên ô trống đến ô trống.\n" +
                "Thắng: Bạn sẽ chiến thắng nếu đưa các mảnh ghép về đúng vị trí ban đầu của nó trong thời gian quy định.";
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += dtTicker;
            var menu = new MenuIngame();
            if (menu.ShowDialog() == true)
            {
                if(MenuIngame.menuReturn=="Exit")
                {
                    this.Close();
                }
                else if(MenuIngame.menuReturn=="Image")
                {
                    imageMode(sender, e);
                }
                else if(MenuIngame.menuReturn=="Load")
                {
                    Load(sender, e);
                }
            }
            else
            {
                this.Close();
            }
        }

        private void dtTicker(object sender, EventArgs e)
        {
            if (_isGamePLaying == true)
            {
                CountdownTime.Text = timeCountdown.ToString();
                timeCountdown--;
                if (timeCountdown < 0)
                {
                    dt.Stop();
                    _isGamePLaying = false;
                    MessageBox.Show("Hãy cố gắng hơn!");
                    timeCountdown = 180;
                }
            }
        }

        public void imageMode(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            screen.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            screen.InitialDirectory = @"C:\";
            screen.Title = "Please select an image file to encrypt.";
            if (screen.ShowDialog() == true)
            {
                filePath = screen.FileName;
                var source = new BitmapImage(
                    new Uri(screen.FileName, UriKind.Absolute));
                previewImage.Width = 300;
                previewImage.Height = 240;
                previewImage.Source = source;
                
                
                // Bat dau cat thanh 9 manh
                int count = 0;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (!((i == 2) && (j == 2)))
                        {
                            TransformedBitmap mybitmapIMG;
                            if (source.Height > source.Width)
                            {
                                double scaleTransfH = 300 / source.Height;
                                mybitmapIMG = new TransformedBitmap(source, new ScaleTransform(scaleTransfH, scaleTransfH));
                                imgWidth = (int)(mybitmapIMG.PixelWidth / 3);
                                imgHeight = (int)(mybitmapIMG.PixelHeight / 3);
                            }
                            else
                            {
                                double scaleTransfW = 300 / source.Width;
                                mybitmapIMG = new TransformedBitmap(source, new ScaleTransform(scaleTransfW, scaleTransfW));
                                imgWidth = (int)(mybitmapIMG.PixelWidth / 3);
                                imgHeight = (int)(mybitmapIMG.PixelHeight / 3);
                            }
                            drawLines(startX, startY, imgWidth, imgHeight);
                            var rect = new Int32Rect(j * imgWidth , i * imgHeight , imgWidth, imgHeight);
                            var cropBitmap = new CroppedBitmap(mybitmapIMG,
                                        rect);
                            var cropImage = new Image();
                            cropImage.Stretch = Stretch.Fill;
                            cropImage.Width = imgWidth;
                            cropImage.Height = imgHeight;
                            cropImage.Source = cropBitmap;
                                canvas.Children.Add(cropImage);
                            Canvas.SetLeft(cropImage, startX + j * (imgWidth + 2));
                            Canvas.SetTop(cropImage, startY + i * (imgHeight + 2));
                            cropImage.MouseLeftButtonDown += CropImage_MouseLeftButtonDown;
                            cropImage.PreviewMouseLeftButtonUp += CropImage_MouseLeftButtonUp;
                            cropImage.Tag = new Tuple<int, int>(i, j);
                            cropImage.Name = $"s{count}";
                                listCropImg.Add(cropImage);
                                count++;
                        }
                        
                    }
                }
                Image nullImage = new Image();
                listCropImg.Add(nullImage);
                
            }
            else
            {
                this.Window_Loaded(sender,e);
            }

        }

        public void Load(object sender, RoutedEventArgs e)
        {
            const string filename = "save.txt";
            try
            { 
            var reader = new StreamReader(filename);
            
                string firstLine = reader.ReadLine();
                string secondLine = reader.ReadLine();
                timeCountdown = int.Parse(firstLine);
                CountdownTime.Text = firstLine;
                var source = new BitmapImage(
                        new Uri(secondLine, UriKind.RelativeOrAbsolute));
                previewImage.Width = 300;
                previewImage.Height = 300;
                previewImage.Source = source;

                // Bat dau cat thanh 9 manh
                int count = 0;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (!((i == 2) && (j == 2)))
                        {
                            TransformedBitmap mybitmapIMG;
                            if (source.Height > source.Width)
                            {
                                double scaleTransfH = 300 / source.Height;
                                mybitmapIMG = new TransformedBitmap(source, new ScaleTransform(scaleTransfH, scaleTransfH));
                                imgWidth = (int)(mybitmapIMG.PixelWidth / 3);
                                imgHeight = (int)(mybitmapIMG.PixelHeight / 3);
                            }
                            else
                            {
                                double scaleTransfW = 300 / source.Width;
                                mybitmapIMG = new TransformedBitmap(source, new ScaleTransform(scaleTransfW, scaleTransfW));
                                imgWidth = (int)(mybitmapIMG.PixelWidth / 3);
                                imgHeight = (int)(mybitmapIMG.PixelHeight / 3);
                            }
                            drawLines(startX, startY, imgWidth, imgHeight);
                            var rect = new Int32Rect(j * imgWidth, i * imgHeight, imgWidth, imgHeight);
                            var cropBitmap = new CroppedBitmap(mybitmapIMG,
                                        rect);
                            var cropImage = new Image();
                            cropImage.Stretch = Stretch.Fill;
                            cropImage.Width = imgWidth;
                            cropImage.Height = imgHeight;
                            cropImage.Source = cropBitmap;
                            canvas.Children.Add(cropImage);
                            Canvas.SetLeft(cropImage, startX + j * (imgWidth + 2));
                            Canvas.SetTop(cropImage, startY + i * (imgHeight + 2));
                            cropImage.MouseLeftButtonDown += CropImage_MouseLeftButtonDown;
                            cropImage.PreviewMouseLeftButtonUp += CropImage_MouseLeftButtonUp;
                            cropImage.Tag = new Tuple<int, int>(i, j);
                            cropImage.Name = $"s{count}";
                            listCropImg.Add(cropImage);
                            count++;
                        }

                    }
                }
                Image nullImage = new Image();
                listCropImg.Add(nullImage);
                int i2 = 0;
                int j2 = 0;
                int irndIMG = 0;
                int jrndIMG = 0;
                int rndIMG;
                for (int a = 0; a < 9; a++)
                {
                    string nameIMG = reader.ReadLine();

                    if (nameIMG != "")
                    {
                        rndIMG = int.Parse(nameIMG.Substring(nameIMG.Length - 1, 1));
                    }
                    else
                    {
                        rndIMG = 8;
                    }
                    if (j2 == 3)
                    {
                        j2 = 0;
                        i2++;
                    }

                    switch (rndIMG)
                    {
                        case 0:
                            irndIMG = 0;
                            jrndIMG = 0;
                            break;
                        case 1:
                            irndIMG = 0;
                            jrndIMG = 1;
                            break;
                        case 2:
                            irndIMG = 0;
                            jrndIMG = 2;
                            break;
                        case 3:
                            irndIMG = 1;
                            jrndIMG = 0;
                            break;
                        case 4:
                            irndIMG = 1;
                            jrndIMG = 1;
                            break;
                        case 5:
                            irndIMG = 1;
                            jrndIMG = 2;
                            break;
                        case 6:
                            irndIMG = 2;
                            jrndIMG = 0;
                            break;
                        case 7:
                            irndIMG = 2;
                            jrndIMG = 1;
                            break;
                        case 8:
                            irndIMG = 2;
                            jrndIMG = 2;
                            iFree = i2;
                            jFree = j2;
                            break;
                    }
                    Canvas.SetLeft(listCropImg[rndIMG], startX + j2 * imgWidth + 2 * j2);
                    Canvas.SetTop(listCropImg[rndIMG], startY + i2 * imgHeight + 2 * i2);
                    Canvas.SetLeft(listCropImg[a], startX + jrndIMG * imgWidth + 2 * jrndIMG);
                    Canvas.SetTop(listCropImg[a], startY + irndIMG * imgHeight + 2 * irndIMG);
                    listCropImg[a].Tag = new Tuple<int, int>(irndIMG, jrndIMG);
                    listCropImg[rndIMG].Tag = new Tuple<int, int>(i2, j2);
                    Image imgtemp = new Image();
                    imgtemp = listCropImg[a];
                    listCropImg[a] = listCropImg[rndIMG];
                    listCropImg[rndIMG] = imgtemp;
                    j2++;
                }
                MessageBox.Show("Game đã được load!!!");
                dt.Start();
                _isGamePLaying = true;
            }
            catch
            {
                MessageBox.Show("Không thể load game gần nhất!!!\nBạn chưa lưu game hoặc file ảnh đã bị thay đổi!");
                this.Window_Loaded(sender, e);
            }
        }


        
        public void shuffle()
        {
            Random rnd = new Random();
            int rndIMG;
            var excludedNumbers = new List<int>();
            int i = 0;
            int j = 0;
            int irndIMG = 0;
            int jrndIMG = 0;
            for (int a = 0; a < 8; a++)
            {
                do
                {
                    rndIMG = rnd.Next(7);
                } while (excludedNumbers.Contains(rndIMG));

                if (j == 3)
                {
                    j = 0;
                    i++;
                }

                switch(rndIMG)
                {
                    case 0:
                        irndIMG = 0;
                        jrndIMG = 0;
                        break;
                    case 1:
                        irndIMG = 0;
                        jrndIMG = 1;
                        break;
                    case 2:
                        irndIMG = 0;
                        jrndIMG = 2;
                        break;
                    case 3:
                        irndIMG = 1;
                        jrndIMG = 0;
                        break;
                    case 4:
                        irndIMG = 1;
                        jrndIMG = 1;
                        break;
                    case 5:
                        irndIMG = 1;
                        jrndIMG = 2;
                        break;
                    case 6:
                        irndIMG = 2;
                        jrndIMG = 0;
                        break;
                    case 7:
                        irndIMG = 2;
                        jrndIMG = 1;
                        break;
                    case 8:
                        irndIMG = 2;
                        jrndIMG = 2;
                        break;
                }
                Canvas.SetLeft(listCropImg[rndIMG], startX + j * imgWidth + 2 * j);
                Canvas.SetTop(listCropImg[rndIMG], startY + i * imgHeight + 2 * i);
                Canvas.SetLeft(listCropImg[a], startX + jrndIMG * imgWidth + 2 * jrndIMG);
                Canvas.SetTop(listCropImg[a], startY + irndIMG * imgHeight + 2 * irndIMG);
                listCropImg[a].Tag = new Tuple<int, int>(irndIMG, jrndIMG);
                listCropImg[rndIMG].Tag = new Tuple<int, int>(i, j);
                Image imgtemp = new Image();
                imgtemp = listCropImg[a];
                listCropImg[a] = listCropImg[rndIMG];
                listCropImg[rndIMG] = imgtemp;
                j++;
            }    
        }



        public void drawLines( int startX, int startY, int width, int height)
        {
            //ve gach ngang
            for (int i = 1; i <= 2; i++)
            {
                var line1 = new Line();
                line1.StrokeThickness = 1;
                canvas.Children.Add(line1);

                line1.X1 = startX + i * width;
                line1.Y1 = startY;

                line1.X2 = startX + i * width;
                line1.Y2 = startY + 3 * height;
            }

            //Ve ganh doc
            for (int i = 1; i <= 2; i++)
            {
                var line2 = new Line();
                line2.StrokeThickness = 1;
                canvas.Children.Add(line2);

                line2.X1 = startX;
                line2.Y1 = startY + i * height;

                line2.X2 = startX + 3 * width;
                line2.Y2 = startY + i * height;
            }
        }

        bool _isDragging = false;
        Image _selectedBitmap = null;
        Point _lastPosition;
        int iFree = 2;
        int jFree = 2;
        private void CropImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            var position = e.GetPosition(this);

            int x = (int)(position.X - startX) / (imgWidth + 2) * (imgWidth + 2) + startX;
            int y = (int)(position.Y - startY) / (imgHeight + 2) * (imgHeight + 2) + startY;

            int iNew = (y - startY) / imgHeight;
            int jNew = (x - startX) / imgWidth;

            var (i, j) = _selectedBitmap.Tag as Tuple<int, int>;

            if (((i == iNew + 1 && j == jNew) || (i == iNew - 1 && j == jNew) || (j == jNew + 1 && i == iNew) || (j == jNew - 1 && i == iNew))
                && iNew < 3 && jNew < 3 && iNew==iFree && jNew==jFree)
            {
                try
                    {
                    iFree = i;
                    jFree = j;
                    Canvas.SetLeft(_selectedBitmap, x);
                    Canvas.SetTop(_selectedBitmap, y);
                    _selectedBitmap.Tag = new Tuple<int, int>(iNew, jNew);
                    int selected = selectBitmapfromIJ(i, j);
                    int free = selectBitmapfromIJ(iNew, jNew);
                    listCropImg[selected].Tag = new Tuple<int, int>(iNew, jNew);
                    listCropImg[free].Tag = new Tuple<int, int>(i, j);
                    Image imgtemp = new Image();
                    imgtemp = listCropImg[selected];
                    listCropImg[selected] = listCropImg[free];
                    listCropImg[free] = imgtemp;
                        
                    
                    /*MessageBox.Show($"{_selectedBitmap.Name} from {i},{j} to {iNew},{jNew}");*/
                    var (gameOver, win) = CheckWin();

                    if (gameOver)
                    {
                        _isGamePLaying = false;
                        if (win == true)
                        {
                            MessageBox.Show("Bạn thật giỏi!");
                        }
                        else
                        {
                            MessageBox.Show("Hãy cố gắng hơn!");
                        }
                    }
                }
                    catch
                    { }
            }
            else
            {
                try
                {
                    Canvas.SetLeft(_selectedBitmap, startX + j*imgWidth + 2*j);
                    Canvas.SetTop(_selectedBitmap, startY + i*imgHeight + 2*i);
                }
                catch
                { }
            }
            
        }


        private void CropImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_isGamePLaying == true)
            {
                _isDragging = true;
                foreach(var cropIMG in listCropImg)
                {
                    if(cropIMG==(sender as Image))
                    {
                        _selectedBitmap = cropIMG;
                    }
                }
                _lastPosition = e.GetPosition(this);
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);
           
            int i = ((int)position.Y - startY ) / height;
            int j = ((int)position.X - startX) / width;


            if (_isDragging)
            {
                var dx = position.X - _lastPosition.X;
                var dy = position.Y - _lastPosition.Y;
                
                var lastLeft = Canvas.GetLeft(_selectedBitmap);
                var lastTop = Canvas.GetTop(_selectedBitmap);
                Canvas.SetLeft(_selectedBitmap, lastLeft + dx);
                Canvas.SetTop(_selectedBitmap, lastTop + dy);

                _lastPosition = position;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (_isGamePLaying == true)
            {
                if (e.Key == Key.Down)
                {
                    int i = iFree - 1;
                    int j = jFree;
                    if (i >= 0 && j >= 0 && i <= 2 & j <= 2)
                    {
                        int selected = selectBitmapfromIJ(i, j);
                        int free = selectBitmapfromIJ(iFree, jFree);
                        if (selected != 9)
                        {
                     /*       MessageBox.Show($"{listCropImg[selected].Name} from {i},{j} to {iFree},{jFree}");*/
                            Canvas.SetLeft(listCropImg[selected], startX + jFree * imgWidth + 2 * jFree);
                            Canvas.SetTop(listCropImg[selected], startY + iFree * imgHeight + 2 * iFree);
                            listCropImg[selected].Tag = new Tuple<int, int>(iFree, jFree);
                            listCropImg[free].Tag = new Tuple<int, int>(i, j);
                            Image imgtemp = new Image();
                            imgtemp = listCropImg[selected];
                            listCropImg[selected] = listCropImg[free];
                            listCropImg[free] = imgtemp;
                            iFree = iFree - 1;
                        }
                    }
                }
                if (e.Key == Key.Up)
                {
                    int i = iFree + 1;
                    int j = jFree;
                    if (i >= 0 && j >= 0 && i <= 2 & j <= 2)
                    {
                        int selected = selectBitmapfromIJ(i, j);
                        int free = selectBitmapfromIJ(iFree, jFree);
                        if (selected != 9)
                        {
/*                            MessageBox.Show($"{listCropImg[selected].Name} from {i},{j} to {iFree},{jFree}");*/
                            Canvas.SetLeft(listCropImg[selected], startX + jFree * imgWidth + 2 * jFree);
                            Canvas.SetTop(listCropImg[selected], startY + iFree * imgHeight + 2 * iFree);
                            listCropImg[selected].Tag = new Tuple<int, int>(iFree, jFree);
                            listCropImg[free].Tag = new Tuple<int, int>(i, j);
                            Image imgtemp = new Image();
                            imgtemp = listCropImg[selected];
                            listCropImg[selected] = listCropImg[free];
                            listCropImg[free] = imgtemp;
                            iFree = i;
                            jFree = j;
                        }
                    }
                }
                if (e.Key == Key.Left)
                {
                    int i = iFree;
                    int j = jFree + 1;
                    if (i >= 0 && j >= 0 && i <= 2 & j <= 2)
                    {
                        int free = selectBitmapfromIJ(iFree, jFree);
                        int selected = selectBitmapfromIJ(i, j);
                        if (selected != 9)
                        {
/*                            MessageBox.Show($"{listCropImg[selected].Name} from {i},{j} to {iFree},{jFree}");*/
                            Canvas.SetLeft(listCropImg[selected], startX + jFree * imgWidth + 2 * jFree);
                            Canvas.SetTop(listCropImg[selected], startY + iFree * imgHeight + 2 * iFree);
                            listCropImg[selected].Tag = new Tuple<int, int>(iFree, jFree);
                            listCropImg[free].Tag = new Tuple<int, int>(i, j);
                            Image imgtemp = new Image();
                            imgtemp = listCropImg[selected];
                            listCropImg[selected] = listCropImg[free];
                            listCropImg[free] = imgtemp;
                            iFree = i;
                            jFree = j;
                        }
                    }
                }
                if (e.Key == Key.Right)
                {
                    int i = iFree;
                    int j = jFree - 1;
                    if (i >= 0 && j >= 0 && i <= 2 & j <= 2)
                    {
                        int free = selectBitmapfromIJ(iFree, jFree);
                        int selected = selectBitmapfromIJ(i, j);
                        if (selected != 8)
                        {
/*                            MessageBox.Show($"{listCropImg[selected].Name} from {i},{j} to {iFree},{jFree}");*/
                            Canvas.SetLeft(listCropImg[selected], startX + jFree * imgWidth + 2 * jFree);
                            Canvas.SetTop(listCropImg[selected], startY + iFree * imgHeight + 2 * iFree);
                            listCropImg[selected].Tag = new Tuple<int, int>(iFree, jFree);
                            listCropImg[free].Tag = new Tuple<int, int>(i, j);
                            Image imgtemp = new Image();
                            imgtemp = listCropImg[selected];
                            listCropImg[selected] = listCropImg[free];
                            listCropImg[free] = imgtemp;
                            iFree = i;
                            jFree = j;
                        }
                    }
                }
                var (gameOver, win) = CheckWin();

                if (gameOver)
                {
                    _isGamePLaying = false;
                    if(win == true)
                    {
                        MessageBox.Show("Bạn thật giỏi!");
                    }
                    else
                    {
                        MessageBox.Show("Hãy cố gắng hơn!");
                    }
                }
            }
        }
        public int selectBitmapfromIJ(int i, int j)
        {
            if(i == 0 && j == 0)
            {
                return 0;
            }
            if (i == 0 && j == 1)
            {
                return 1;
            }
            if (i == 0 && j == 2)
            {
                return 2;
            }
            if (i == 1 && j == 0)
            {
                return 3;
            }
            if (i == 1 && j == 1)
            {
                return 4;
            }
            if (i == 1 && j == 2)
            {
                return 5;
            }
            if (i == 2 && j == 0)
            {
                return 6;
            }
            if (i == 2 && j == 1)
            {
                return 7;
            }
            if (i == 2 && j == 2)
            {
                return 8;
            }
            return 9;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isGamePLaying == false)
            {
                MessageBox.Show("We will shuffle!!!");
                shuffle();
                _isGamePLaying = true;
                timeCountdown = 180;
                dt.Start();
            }
        }

        private (bool,bool) CheckWin()
        {
            var gameOver = false;
            var win = false;
            bool check = true;
            for (int i=0;i<=2;i++)
            {
                for (int j=0;j<=2;j++)
                {
                    int selected = selectBitmapfromIJ(i, j);
                    if (selected != 8)
                    {
                        if (listCropImg[selected].Name != ("s" + selected.ToString()))
                        {
                            check = false;
                        }
                    }
                }
            }
            if(check != false)
            {
                gameOver = true;
                win = true;
            }
            
            return (gameOver, win);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _isGamePLaying = false;
            const string filename = "save.txt";
            string text = "";
            text = text + $"{timeCountdown}\n";
            text = text + $"{filePath}";
            int i=0,j=0;
            foreach (var cropIMG in listCropImg)
            {
                if(j==3)
                {
                    i++;
                    j = 0;
                }
                text = text + "\n";
                string name = cropIMG.Name.ToString();
                text = text + name;
                j++;
            }
            var write = new StreamWriter(filename);
            write.WriteLine(text);
            write.Close();
            MessageBox.Show("Game đã được lưu!");
            _isGamePLaying = true;
        }
    }
}
