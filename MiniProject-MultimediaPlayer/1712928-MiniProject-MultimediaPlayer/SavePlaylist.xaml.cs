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
using System.Windows.Shapes;

namespace _1712928_MiniProject_MultimediaPlayer
{
    /// <summary>
    /// Interaction logic for SavePlaylist.xaml
    /// </summary>
    public partial class SavePlaylist : Window
    {
        public SavePlaylist()
        {
            InitializeComponent();
        }

        private void SavePlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            if(namePlaylistToSave.Text != "")
            {
                string path = "Playlist/" + namePlaylistToSave.Text + ".txt";
                if(!Directory.Exists("Playlist"))
                {
                    Directory.CreateDirectory("Playlist");
                }
                if (File.Exists(path))
                {
                    MessageBox.Show("Name of playlist existed!");
                }
                else
                {
                    if(MainWindow.endPoint == -1)
                    {
                        MessageBox.Show("Playlist is empty!");
                    }
                    else
                    {
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            sw.WriteLine(MainWindow.endPoint);
                            foreach (var musicPath in MainWindow._fullPaths)
                            {
                                sw.WriteLine(musicPath.FullName);
                            }
                        }
                        MessageBox.Show("Playlist saved!!!");
                        this.DialogResult = true;
                        this.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("You forgot name the playlist!!!");
            }
        }
    }
}
