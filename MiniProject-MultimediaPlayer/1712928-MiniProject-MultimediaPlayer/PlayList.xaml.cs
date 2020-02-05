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
using System.Windows.Shapes;

namespace _1712928_MiniProject_MultimediaPlayer
{
    /// <summary>
    /// Interaction logic for PlayList.xaml
    /// </summary>
    public partial class PlayList : Window
    {
        public static bool returnPlaylist = false;
        BindingList<FileInfo> _ListPlaylist = new BindingList<FileInfo>();
        public PlayList()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists("Playlist"))
            {
                DirectoryInfo d = new DirectoryInfo("Playlist");//Assuming Test is your Folder
                FileInfo[] Files = d.GetFiles("*.txt");
                foreach(var file in Files)
                {
                    _ListPlaylist.Add(file);
                }
            }
            ListPlaylistListBox.ItemsSource = _ListPlaylist;

        }
        private void PlaylistSelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (ListPlaylistListBox.SelectedIndex >= 0)
            {
                var reader = new StreamReader("Playlist/" + ListPlaylistListBox.SelectedItem.ToString());
                int numberofsongs = int.Parse(reader.ReadLine());
                MainWindow._fullPaths.Clear();
                for(int i=0;i<numberofsongs;i++)
                {
                    var song = new FileInfo(reader.ReadLine());
                    MainWindow._fullPaths.Add(song);
                }
                returnPlaylist = true;
                MainWindow.endPoint = numberofsongs;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select 1 playlist!!!");
            }
        }

        private void PlaylistCancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SavePlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            var savePlaylist = new SavePlaylist();
            if(savePlaylist.ShowDialog()==true)
            {
                _ListPlaylist.Clear();
                DirectoryInfo d = new DirectoryInfo("Playlist");//Assuming Test is your Folder
                FileInfo[] Files = d.GetFiles("*.txt");
                foreach (var file in Files)
                {
                    _ListPlaylist.Add(file);
                }
                ListPlaylistListBox.ItemsSource = _ListPlaylist;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if(ListPlaylistListBox.SelectedItem != null)
            {
                File.Delete(_ListPlaylist[ListPlaylistListBox.SelectedIndex].FullName);
                _ListPlaylist.Remove(_ListPlaylist[ListPlaylistListBox.SelectedIndex]);
                MessageBox.Show("Playlist deleted!!!");
            }
        }

        private void ListPlaylistListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListPlaylistListBox.SelectedItem != null)
            {
                var reader = new StreamReader("Playlist/" + ListPlaylistListBox.SelectedItem.ToString());
                int numberofsongs = int.Parse(reader.ReadLine());
                MainWindow._fullPaths.Clear();
                for (int i = 0; i < numberofsongs; i++)
                {
                    var song = new FileInfo(reader.ReadLine());
                    MainWindow._fullPaths.Add(song);
                }
                returnPlaylist = true;
                MainWindow.endPoint = numberofsongs;
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
