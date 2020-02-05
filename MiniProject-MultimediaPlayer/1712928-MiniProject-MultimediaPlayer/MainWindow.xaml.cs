using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;
using System.Windows.Forms;

namespace _1712928_MiniProject_MultimediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MediaPlayer _player = new MediaPlayer();
        DispatcherTimer _timer;
        int _lastIndex = -1;
        public static int endPoint=-1;
        private IKeyboardMouseEvents _hook;
        bool _isPlaying = false;
        bool _isRandom = false;
        string _playingMode = "noLoop";
        public static BindingList<FileInfo> _fullPaths = new BindingList<FileInfo>();

        public MainWindow()
        {
            InitializeComponent();
            _player.MediaEnded += _player_MediaEnded;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += timer_Tick;

            // Dang ky su kien hook
            _hook = Hook.GlobalEvents();
            _hook.KeyUp += KeyUp_hook;
        }


        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            if (playlistListBox.SelectedIndex >= 0 )
            {
                _lastIndex = playlistListBox.SelectedIndex;
                PlaySelectedIndex(_lastIndex);
            }
            else
            {
                System.Windows.MessageBox.Show("No file selected!");
            }
        }

        private void PlaySelectedIndex(int i)
        {

            string filename = _fullPaths[i].FullName;
            _player.Open(new Uri(filename, UriKind.Absolute));

            if (_player.NaturalDuration.HasTimeSpan)
            {
                var duration = _player.NaturalDuration.TimeSpan;
                var testDuration = new TimeSpan(duration.Hours, duration.Minutes, duration.Seconds);
                _player.Position = testDuration;
            }
            _player.Play();
            _isPlaying = true;
            _timer.Start();
        }

        private void _player_MediaEnded(object sender, EventArgs e)
        {
           
            if (_playingMode == "noLoop")
            {
                if (_isRandom == false)
                {
                    _lastIndex++;
                    if (_lastIndex < endPoint)
                    {
                        PlaySelectedIndex(_lastIndex);
                    }
                    else
                    {
                        PlaySelectedIndex(0);
                        _isPlaying = false;
                        _player.Stop();

                    }
                }
                else if (_isRandom==true &&_lastIndex!=endPoint-1)
                {
                    Random random = new Random();
                    int RandomNumber = random.Next(0, endPoint);
                    _lastIndex = RandomNumber;
                    PlaySelectedIndex(_lastIndex);
                    
                }
                else
                {
                    PlaySelectedIndex(0);
                    _isPlaying = false;
                    _player.Stop();
                }
            }
            else if(_playingMode == "Loop")
            {
                if (_isRandom == false)
                {
                    _lastIndex++;
                    if (_lastIndex < endPoint)
                    {
                        PlaySelectedIndex(_lastIndex);
                    }
                    else
                    {
                        _lastIndex = 0;
                        PlaySelectedIndex(_lastIndex);
                        TimeSpan ts = TimeSpan.FromSeconds(0);
                        _player.Position = ts;
                    }
                }
                else
                {
                    Random random = new Random();
                    int RandomNumber = random.Next(0, endPoint-1);
                    _lastIndex = RandomNumber;
                    PlaySelectedIndex(_lastIndex);
                }
            }
            else if (_playingMode == "Loop1")
            { 
                TimeSpan ts = TimeSpan.FromSeconds(0);
                _player.Position = ts;
            }
            playlistListBox.SelectedIndex = _lastIndex;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (_player.Source != null)
            {
                
                if (_isPlaying == true)
                {
                    string filepath = _fullPaths[_lastIndex].FullName;
                    TagLib.File tagFile = TagLib.File.Create(filepath);
                    string title = tagFile.Tag.Title;
                    string singer = tagFile.Tag.FirstPerformer;
                    if (_player.NaturalDuration.HasTimeSpan)
                    {
                        var currentPos = _player.Position.ToString(@"mm\:ss");
                        var duration = _player.NaturalDuration.TimeSpan.ToString(@"mm\:ss");

                        MusicTitleLable.Content = String.Format($"{title} - {singer}");
                        TimePlayingLable.Content = String.Format($"{currentPos} / {duration}");
                        TimePlayingSliderBar.Minimum = 0;
                        TimePlayingSliderBar.Maximum = _player.NaturalDuration.TimeSpan.TotalSeconds;
                        TimePlayingSliderBar.Value = _player.Position.TotalSeconds;
                       
                    }
                }
            }

        }



        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isPlaying && _player.Source!=null)
            {
                _player.Pause();
                pauseButton.Content = FindResource("Continue");
            }
            else
            {
                _player.Play();
                pauseButton.Content = FindResource("Pause");
            }
            _isPlaying = !_isPlaying;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new Microsoft.Win32.OpenFileDialog();
            screen.Multiselect = true;
            if (screen.ShowDialog() == true)
            {
                foreach (var fileMusic in screen.FileNames)
                {
                    var info = new FileInfo(fileMusic);
                    _fullPaths.Add(info);
                }
            }
            endPoint = _fullPaths.Count;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            playlistListBox.ItemsSource = _fullPaths;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void KeyUp_hook(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Control && e.Shift && (e.KeyCode == Keys.E))
            {
                ForwardButton_Click(null, null);
            }
            if (e.Control && e.Shift && (e.KeyCode == Keys.Q))
            {
                BackwardButton_Click(null, null);
            }
            if (e.Control && e.Shift && (e.KeyCode == Keys.W))
            {
                pauseButton_Click(null, null);
            }
        }


        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _hook.KeyUp -= KeyUp_hook;
            _hook.Dispose();

        }

        private void TimePlayingSliderBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TimeSpan ts = TimeSpan.FromSeconds(e.NewValue);
            _player.Position = ts;
        }

        private void PlaylistModeButton_Click(object sender, RoutedEventArgs e)
        {
            if(_playingMode == "noLoop")
            {
                _playingMode = "Loop";
                PlaylistModeButton.Content = FindResource("Loop-active");
            }
            else if (_playingMode == "Loop")
            {
                _playingMode = "Loop1";
                PlaylistModeButton.Content = FindResource("Loop-1-song");
            }
            else if (_playingMode == "Loop1")
            {
                _playingMode = "noLoop";
                PlaylistModeButton.Content = FindResource("Loop-no-active");
            }
        }

        private void RandomModeButton_Click(object sender, RoutedEventArgs e)
        {
            if(_isRandom == false)
            {
                RandomModeButton.Content = FindResource("Random-active");
            }
            else
            {
                RandomModeButton.Content = FindResource("Random-no-active");
            }
            _isRandom = !_isRandom;
        }

        private void playlistButton_Click(object sender, RoutedEventArgs e)
        {
            var playlistScreen = new PlayList();
            if(playlistScreen.ShowDialog() == true)
            {
                if(PlayList.returnPlaylist == true)
                {
                    if(_player.Source != null)
                    _player.Stop();
                    _lastIndex = -1;
                    playlistListBox.SelectedIndex = 0;
                }
            }
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (_player.Source != null)
            {
                if (_lastIndex == endPoint - 1)
                {
                    _lastIndex = -1;
                    _player_MediaEnded(null, null);
                }
                else _player_MediaEnded(null, null);
            }
        }

        private void BackwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (_player.Source != null)
            {
                if (_lastIndex != 0)
                {
                    _lastIndex = _lastIndex - 2;
                    _player_MediaEnded(sender, e);
                }
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if(_isPlaying==true && _player.Source!=null)
            {
                _player.Stop();
                _isPlaying = false;
            }
        }

        private void PlaylistListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (playlistListBox.SelectedItem != null)
            {
                _lastIndex = playlistListBox.SelectedIndex;
                PlaySelectedIndex(_lastIndex);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (playlistListBox.SelectedItem != null)
            {
                _fullPaths.Remove(_fullPaths[playlistListBox.SelectedIndex]);
                endPoint--;
            }
        }

        private void KeyButton_Click(object sender, RoutedEventArgs e)
        {
            var key = new KeyHook();
            if(key.ShowDialog()==true)
            {

            }
        }
    }
}
