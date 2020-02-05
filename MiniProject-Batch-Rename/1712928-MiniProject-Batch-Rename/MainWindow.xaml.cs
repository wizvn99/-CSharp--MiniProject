using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
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
using Path = System.IO.Path;
using System.Globalization;
using  Microsoft.VisualBasic.FileIO;

namespace _1712928_MiniProject_Batch_Rename
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MethodsListBox.ItemsSource = actions;
            
        }

        BindingList<Folder> _folders = new BindingList<Folder>();
        BindingList<File> _files = new BindingList<File>();

        class Folder
        {
            public string FolderName { get; set; }
            public string newFolderName { get; set; }
            public string FolderPath { get; set; }

        }

        class File
        {
            public string FileName { get; set; }
            public string newFileName { get; set; }
            public string FilePath { get; set; }

        }

        class MethodChecked
        {
            public bool ReplaceMethodChecked { get; set; }

            public bool NewCaseMethodChecked { get; set; }

            public bool FullNameNomalizeMethodChecked { get; set; }

            public bool MoveMethodChecked { get; set; }

            public bool UniqueNameMethodChecked { get; set; }
        }

        private void AddFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    FileNameListBox.Items.Add(Path.GetFileName(filename));
                    FilePathListBox.Items.Add(Path.GetDirectoryName(filename));
                    var file = new File()
                    {
                        FileName = Path.GetFileName(filename),
                        FilePath = Path.GetDirectoryName(filename),
                        newFileName = null
                    };
                    _files.Add(file);
                }
            }
        }

        private void AddFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new CommonOpenFileDialog();
            openFileDialog.IsFolderPicker = true;
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    FolderNameListBox.Items.Add(Path.GetFileName(filename));
                    FolderPathListBox.Items.Add(Path.GetDirectoryName(filename));
                    var folder = new Folder()
                    {
                        FolderName = Path.GetFileName(filename),
                        FolderPath = Path.GetDirectoryName(filename),
                        newFolderName = null
                    };
                    _folders.Add(folder);
                }
            }
        }

        private void AddFileFromRootButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new CommonOpenFileDialog();
            openFileDialog.IsFolderPicker = true;
            List<File> listfiles = new List<File>();
            if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                foreach (string f in Directory.GetFiles(openFileDialog.FileName))
                {
                    FileNameListBox.Items.Add(Path.GetFileName(f));
                    FilePathListBox.Items.Add(Path.GetDirectoryName(f));
                    var file = new File()
                    {
                        FileName = Path.GetFileName(f),
                        FilePath = Path.GetDirectoryName(f),
                        newFileName = null
                    };
                    _files.Add(file);
                }
            }
        }

        private void AddFolderFromRootButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new CommonOpenFileDialog();
            openFileDialog.IsFolderPicker = true;
            if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                foreach (string d in Directory.GetDirectories(openFileDialog.FileName))
                {
                    FolderNameListBox.Items.Add(Path.GetFileName(d));
                    FolderPathListBox.Items.Add(Path.GetDirectoryName(d));
                    var folder = new Folder()
                    {
                        FolderName = Path.GetFileName(d),
                        FolderPath = Path.GetDirectoryName(d)
                    };
                    _folders.Add(folder);
                }
            }
        }

        //Toolbar view
        private void StartBatchButton_Click(object sender, RoutedEventArgs e)
        {
            int checkError = 0;
            if (FileTab.IsSelected == true)
            {
                if (NewFileNameListBox.Items.IsEmpty == true)
                {
                    setPreview();
                    checkNameSake();
                }
                else
                {
                    NewFileNameListBox.Items.Clear();
                    FileError.Items.Clear();
                    setPreview();
                    checkNameSake();
                }

                if (FileError.Items.IndexOf("Trùng tên với một trong các file khác.") != -1)
                {
                    
                    var screen = new CatchErrorWindow();
                    if (screen.ShowDialog() == true)
                    {
                        if (CatchErrorWindow.Solution == "Add")
                        {
                            if (NewFileNameListBox.Items.IsEmpty == false)
                            {
                                int addnumber = 1;
                                foreach (var file in _files)
                                {
                                    int count = 0;
                                    foreach (var file2 in _files)
                                    {
                                        if (file.newFileName == file2.newFileName)
                                        {
                                            count++;
                                        }
                                    }
                                    if (count > 1)
                                    {
                                        int extendposition = file.newFileName.LastIndexOf(".");
                                        string extend = file.newFileName.Substring(extendposition);
                                        file.newFileName = file.newFileName.Remove(extendposition);
                                        file.newFileName = file.newFileName + $"_{addnumber}";
                                        file.newFileName = file.newFileName + extend;
                                        addnumber++;
                                    }
                                }
                            }
                        }

                        else if (CatchErrorWindow.Solution == "Skip")
                        {
                            if (NewFileNameListBox.Items.IsEmpty == false)
                            {
                                foreach (var file in _files)
                                {
                                    int count = 0;
                                    foreach (var file2 in _files)
                                    {
                                        if (file.newFileName == file2.newFileName)
                                        {
                                            count++;
                                        }
                                    }
                                    if (count > 1)
                                    {
                                        file.newFileName = file.FileName;
                                    }
                                }
                            }
                        }
                        else
                        {
                            checkError = 1;
                        }
                    }

                }
            }
            else 
            {
                if (NewFolderNameListBox.Items.IsEmpty == true)
                {
                    setPreview();
                    checkNameSake();
                }
                else
                {
                    NewFolderNameListBox.Items.Clear();
                    FolderError.Items.Clear();
                    setPreview();
                    checkNameSake();
                }

                if (FolderError.Items.IndexOf("Trùng tên với một trong các folder khác.") != -1)
                {
                    var screen = new CatchErrorWindow();
                    if (screen.ShowDialog() == true)
                    {
                        if (CatchErrorWindow.Solution == "Add")
                        {
                            if (NewFolderNameListBox.Items.IsEmpty == false)
                            {
                                int addnumber = 1;
                                foreach (var folder in _folders)
                                {
                                    int count = 0;
                                    foreach (var folder2 in _folders)
                                    {
                                        if (folder.newFolderName == folder2.newFolderName)
                                        {
                                            count++;
                                        }
                                    }
                                    if (count > 1)
                                    {
                                        folder.newFolderName = folder.newFolderName + $"_{addnumber}";
                                        addnumber++;
                                    }

                                }
                            }
                        }

                        else if (CatchErrorWindow.Solution == "Skip")
                        {
                            if (NewFolderNameListBox.Items.IsEmpty == false)
                            {
                                foreach (var folder in _folders)
                                {
                                    int count = 0;
                                    foreach (var folder2 in _folders)
                                    {
                                        if (folder.newFolderName == folder2.newFolderName)
                                        {
                                            count++;
                                        }
                                    }
                                    if (count > 1)
                                    {
                                        folder.newFolderName = folder.FolderName;
                                    }

                                }
                            }
                        }
                        else
                        {
                            checkError = 1;
                        }
                    }

                }
            }

            if(checkError!=1)
            {
                foreach(var file in _files)
                {
                    int checkcount = 0;
                    foreach (var file2 in _files)
                    {
                        if (file.newFileName == file2.FileName)
                        {
                            checkcount++;
                        }
                    }
                    if (checkcount < 1)
                        System.IO.File.Move(file.FilePath + "//" + file.FileName, file.FilePath + "//" + file.newFileName);
                    _files.Remove(file);
                }
                foreach (var folder in _folders)
                {
                    if (folder.FolderName != folder.newFolderName)
                    {
                        int checkcount = 0;
                        foreach(var folder2 in _folders)
                        {
                            if(folder.newFolderName ==folder2.FolderName)
                            {
                                checkcount++;
                            }
                        }
                        if(checkcount<1)
                        Microsoft.VisualBasic.FileIO.FileSystem.RenameDirectory(folder.FolderPath + "//" + folder.FolderName, folder.newFolderName);                      
                    }
                    _folders.Remove(folder);
                }
                actions.Clear();
                MethodsListBox.Items.Refresh();
                _files.Clear();
                _folders.Clear();
                FileNameListBox.Items.Clear();
                NewFileNameListBox.Items.Clear();
                FilePathListBox.Items.Clear();
                FileError.Items.Clear();
                FolderNameListBox.Items.Clear();
                NewFolderNameListBox.Items.Clear();
                FolderPathListBox.Items.Clear();
                FolderError.Items.Clear();
                MessageBox.Show("Đã đổi tên thành công!");
            }
        }

        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (FileTab.IsSelected == true)
            {
                if (NewFileNameListBox.Items.IsEmpty == true)
                {
                    setPreview();
                    checkNameSake();
                }
                else
                {
                    NewFileNameListBox.Items.Clear();
                    FileError.Items.Clear();
                    setPreview();
                    checkNameSake();
                }
            }
            else if(FolderTab.IsEnabled == true)
            {
                if (NewFolderNameListBox.Items.IsEmpty == true)
                {
                    setPreview();
                    checkNameSake();
                }
                else
                {
                    NewFolderNameListBox.Items.Clear();
                    FolderError.Items.Clear();
                    setPreview();
                    checkNameSake();
                }
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            actions.Clear();
            MethodsListBox.Items.Refresh();
            _files.Clear();
            _folders.Clear();
            FileNameListBox.Items.Clear();
            NewFileNameListBox.Items.Clear();
            FilePathListBox.Items.Clear();
            FileError.Items.Clear();
            FolderNameListBox.Items.Clear();
            NewFolderNameListBox.Items.Clear();
            FolderPathListBox.Items.Clear();
            FolderError.Items.Clear();
        }

        //Methods View

        private void ReplaceMethodWindowButton_Click(object sender, RoutedEventArgs e)
        {

            var screen = new ReplaceMethodWindow();
            if (screen.ShowDialog() == true)
            {
                var action = new ReplaceOperation()
                {
                    Args = new ReplaceArgs() { From = ReplaceMethodWindow.replaceFrom, To = ReplaceMethodWindow.replaceTo }
                };
                actions.Add(action.Clone());
            }
        }

        private void NewCaseMethodWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new NewCaseMethodWindow();
            if (screen.ShowDialog() == true)
            {
                var action = new NewCaseOperation()
                {
                    Args = new NewCaseArgs() { From = NewCaseMethodWindow.newcaseFrom, To = NewCaseMethodWindow.newcaseTo }
                };
                actions.Add(action.Clone());
            }
        }

        private void FullnameNormalizeMethodWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new FullNameMethodWindow();
            if (screen.ShowDialog() == true)
            {
                var action = new FullNameOperation()
                {
                    Args = new FullNameArgs() { From = FullNameMethodWindow.fullnameFrom, To = FullNameMethodWindow.fullnameTo }
                };
                actions.Add(action.Clone());
            }
        }

        private void MoveMethodWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new MoveMethodWindow();
            if (screen.ShowDialog() == true)
            {
                var action = new MoveOperation()
                {
                    Args = new MoveArgs() { From = MoveMethodWindow.moveNChars, To = MoveMethodWindow.fromto }
                };
                actions.Add(action.Clone());
            }
        }

        private void UniqueNameMethodWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new UniqueMethodWindow();
            if (screen.ShowDialog() == true)
            {
                var action = new UniqueNameOperation()
                {
                    Args = new UniqueNameArgs() { From = UniqueMethodWindow.UniqueFrom, To = UniqueMethodWindow.UniqueTo }
                };
                actions.Add(action.Clone());
            }
        }

        //Preset 
        private void SaveListMethods_Click(object sender, RoutedEventArgs e)
        {
            const string filename = "save.txt";
            string text ="";
            int NumberOfMethods=MethodsListBox.Items.Count;
            text = text + $"{NumberOfMethods}";
            foreach(var action in actions)
            {
                text = text + "\n";
                string type = action.Name.ToString();
                string fromto = action.fromtoDescription.ToString();
                text = text + type + " " + fromto;
            }
            var write = new StreamWriter(filename);
            write.Write(text);
            MessageBox.Show("File đã được lưu!");
            
        }

        private void LoadListMethods_Click(object sender, RoutedEventArgs e)
        {
            string text ="";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                text = System.IO.File.ReadAllText(openFileDialog.FileName);

                string[] textpart = text.Split('\n');
                int numberofMethods = int.Parse(textpart[0]);
                for (int i = 1; i <= numberofMethods; i++)
                {
                    string[] textpartoftextpart = textpart[i].Split(' ');
                    if (textpartoftextpart[0] == "Replace")
                    {
                        var action = new ReplaceOperation()
                        {
                            Args = new ReplaceArgs() { From = textpartoftextpart[1], To = textpartoftextpart[2] }
                        };
                        actions.Add(action.Clone());
                    }
                    if (textpartoftextpart[0] == "NewCase")
                    {
                        var action = new NewCaseOperation()
                        {
                            Args = new NewCaseArgs() { From = textpartoftextpart[1], To = textpartoftextpart[2] }
                        };
                        actions.Add(action.Clone());
                    }
                    if (textpartoftextpart[0] == "FullNameNormalize")
                    {
                        var action = new FullNameOperation()
                        {
                            Args = new FullNameArgs() { From = textpartoftextpart[1], To = textpartoftextpart[2] }
                        };
                        actions.Add(action.Clone());
                    }
                    if (textpartoftextpart[0] == "Move")
                    {
                        var action = new MoveOperation()
                        {
                            Args = new MoveArgs() { From = textpartoftextpart[1], To = textpartoftextpart[2] }
                        };
                        actions.Add(action.Clone());
                    }
                    if (textpartoftextpart[0] == "UniqueName")
                    {
                        var action = new UniqueNameOperation()
                        {
                            Args = new UniqueNameArgs() { From = textpartoftextpart[1], To = textpartoftextpart[2] }
                        };
                        actions.Add(action.Clone());
                    }
                }
            }
        }

        //Methods Setting
        BindingList<StringOperation> actions = new BindingList<StringOperation>();

        public class StringArgs
        {

        }

        public abstract class StringOperation : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public StringArgs Args { get; set; }
            public abstract string Operate(string origin);
            public abstract string Name { get; }
            public abstract string Description { get; }
            public abstract string fromtoDescription { get; }
            public abstract StringOperation Clone();
        }

        public void setPreview()
        {
            if (FileTab.IsSelected == true)
            {
                foreach (var file in _files)
                {
                    file.newFileName = file.FileName;
                    for (int i = 0; i < actions.Count; i++)
                    {
                        file.newFileName = actions[i].Operate(file.newFileName);
                    }
                    NewFileNameListBox.Items.Add(file.newFileName);
                }
            }
            else if (FolderTab.IsSelected==true)
            {
                foreach (var folder in _folders)
                {
                    folder.newFolderName = folder.FolderName;
                    for (int i = 0; i < actions.Count; i++)
                    {
                        folder.newFolderName = actions[i].Operate(folder.newFolderName);
                    }
                    NewFolderNameListBox.Items.Add(folder.newFolderName);
                }
            }
        }

        //Replace Method

        public class ReplaceArgs : StringArgs, INotifyPropertyChanged
        {
            public string From { get; set; }
            public string To { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        

        public class ReplaceOperation : StringOperation, INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public override string Operate(string origin)
            {
                var args = Args as ReplaceArgs;
                var from = args.From;
                var to = args.To;

                return origin.Replace(from, to);
            }

            public override StringOperation Clone()
            {
                var oldArgs = Args as ReplaceArgs;
                return new ReplaceOperation()
                {
                    Args = new ReplaceArgs()
                    {
                        From = oldArgs.From,
                        To = oldArgs.To
                    }
                };
            }

            public override string Name => "Replace";
            public override string Description
            {
                get
                {
                    var args = Args as ReplaceArgs;
                    return $"Replace from {args.From} to {args.To}";
                }
            }
            public override string fromtoDescription
            {
                get
                {
                    var args = Args as ReplaceArgs;
                    return $"{args.From} {args.To}";
                }
            }

        }

        //NewCase Method

       public class NewCaseArgs : StringArgs, INotifyPropertyChanged
        {
            public string From { get; set; }
            public string To { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
        }


        public class NewCaseOperation : StringOperation, INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public override string Operate(string origin)
            {
                var args = Args as NewCaseArgs;
                var from = args.From;
                var to = args.To;
                if (to == "upper")
                {
                    return origin.ToUpper();
                }
                if (to == "lower")
                {
                    return origin.ToLower();
                }
                if (to == "firstChar")
                {
                    if (string.IsNullOrEmpty(origin))
                        return string.Empty;
                    char[] letters = origin.ToCharArray();
                    letters[0] = char.ToUpper(letters[0]);
                    return new string(letters);
                }
                return origin;
            }

            public override StringOperation Clone()
            {
                var oldArgs = Args as NewCaseArgs;
                return new NewCaseOperation()
                {
                    Args = new NewCaseArgs()
                    {
                        From = oldArgs.From,
                        To = oldArgs.To
                    }
                };
            }

            public override string Name => "NewCase";
            public override string Description
            {
                get
                {
                    var args = Args as NewCaseArgs;
                    return $"New Case from {args.From} to {args.To}";
                }
            }

            public override string fromtoDescription
            {
                get
                {
                    var args = Args as NewCaseArgs;
                    return $"{args.From} {args.To}";
                }
            }
        }

        //Full Name Normalize Method

        public class FullNameArgs : StringArgs, INotifyPropertyChanged
        {
            public string From { get; set; }
            public string To { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
        }


        public class FullNameOperation : StringOperation, INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public override string Operate(string origin)
            {
                var args = Args as FullNameArgs;
                var from = args.From;
                var to = args.To;
                if (to == "fullname")
                {
                    string Result = "";
                    if (string.IsNullOrEmpty(origin))
                        return string.Empty;
                    origin = origin.Trim();
                    while (origin.IndexOf("  ") != -1)
                    {
                        origin = origin.Replace("  ", " ");
                    }
                    string[] SubName = origin.Split(' ');

                    for (int i = 0; i < SubName.Length; i++)
                    {
                        string FirstChar = SubName[i].Substring(0, 1);
                        string OtherChar = SubName[i].Substring(1);
                        SubName[i] = FirstChar.ToUpper() + OtherChar.ToLower();
                        Result += SubName[i] + " ";
                    }
                    if (Result.LastIndexOf(".") == (Result.LastIndexOf(" .")+1))
                    {
                        Result = Result.Remove(Result.LastIndexOf(" ."),1);
                    }

                    return Result;
                }
                return origin;
            }

            public override StringOperation Clone()
            {
                var oldArgs = Args as FullNameArgs;
                return new FullNameOperation()
                {
                    Args = new FullNameArgs()
                    {
                        From = oldArgs.From,
                        To = oldArgs.To
                    }
                };
            }

            public override string Name => "FullNameNormalize";
            public override string Description
            {
                get
                {
                    var args = Args as FullNameArgs;
                    return $"Full Name Normalize";
                }
            }

            public override string fromtoDescription
            {
                get
                {
                    var args = Args as FullNameArgs;
                    return $"{args.From} {args.To}";
                }
            }
        }


        //Move Method

        public class MoveArgs : StringArgs, INotifyPropertyChanged
        {
            public string From { get; set; }
            public string To { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
        }


        public class MoveOperation : StringOperation, INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public override string Operate(string origin)
            {
                var args = Args as MoveArgs;
                var from = args.From;
                var to = args.To;
                int nChars = int.Parse(from);
                int extendposition=origin.LastIndexOf(".");
                string extend = origin.Substring(extendposition);
                origin=origin.Remove(extendposition);
                if (nChars < origin.Length)
                {

                    if (to == "fromFronttoEnd")
                    {
                        string temp;
                        for (int i = 0; i < nChars; i++)
                        {
                            temp = origin[0].ToString();
                            origin = origin.Remove(0, 1);
                            origin += temp;
                        }
                    }

                    if (to == "fromEndtoFront")
                    {
                        string temp;
                        for (int i = 0; i < nChars; i++)
                        {
                            temp = origin[origin.Length - 1].ToString();
                            origin = origin.Remove(origin.Length - 1, 1);
                            origin = temp + origin;
                        }
                    }
                }
                origin = origin + extend;
                return origin;
            }

            public override StringOperation Clone()
            {
                var oldArgs = Args as MoveArgs;
                return new MoveOperation()
                {
                    Args = new MoveArgs()
                    {
                        From = oldArgs.From,
                        To = oldArgs.To
                    }
                };
            }

            public override string Name => "Move";
            public override string Description
            {
                get
                {
                    var args = Args as MoveArgs;
                    return $"Move {args.From} characters {args.To}";
                }
            }

            public override string fromtoDescription
            {
                get
                {
                    var args = Args as MoveArgs;
                    return $"{args.From} {args.To}";
                }
            }
        }

        //Unique Name Method

        public class UniqueNameArgs : StringArgs, INotifyPropertyChanged
        {
            public string From { get; set; }
            public string To { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
        }


        public class UniqueNameOperation : StringOperation, INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public override string Operate(string origin)
            {
                var args = Args as UniqueNameArgs;
                var from = args.From;
                var to = args.To;
                int extendposition = origin.LastIndexOf(".");
                string extend = origin.Substring(extendposition);
                if (to == "Unique")
                {
                    Guid guid = Guid.NewGuid();
                    if (guid == Guid.Empty)
                    {
                        guid = Guid.NewGuid();
                        if (guid == Guid.Empty)
                        {
                            guid = Guid.NewGuid();
                        }
                    }
                    return (DateTime.Now.ToString("yyyyMMdd", CultureInfo.GetCultureInfo("en-US")) + guid.ToString().Replace("-", string.Empty)) + extend;
                }
                return origin;
            }

            public override StringOperation Clone()
            {
                var oldArgs = Args as UniqueNameArgs;
                return new UniqueNameOperation()
                {
                    Args = new UniqueNameArgs()
                    {
                        From = oldArgs.From,
                        To = oldArgs.To
                    }
                };
            }

            public override string Name => "UniqueName";
            public override string Description
            {
                get
                {
                    var args = Args as UniqueNameArgs;
                    return $"Unique Name";
                }
            }

            public override string fromtoDescription
            {
                get
                {
                    var args = Args as UniqueNameArgs;
                    return $"{args.From} {args.To}";
                }
            }
        }

        

        //Check Error
        public void checkNameSake()
        {
            if(NewFileNameListBox.Items.IsEmpty==false)
            {
                foreach(var file in _files)
                {
                    int count = 0;
                    foreach(var file2 in _files)
                    {
                        if(file.newFileName == file2.newFileName)
                        {
                            count++;
                        }
                    }
                    if(count > 1)
                    {
                        FileError.Items.Add("Trùng tên với một trong các file khác.");
                    }
                    else
                    {
                        FileError.Items.Add("Hợp lệ.");
                    }
                }
            }
            if (NewFolderNameListBox.Items.IsEmpty == false)
            {
                foreach (var folderCheck in _folders)
                {
                    folderCheck.newFolderName=folderCheck.newFolderName.Trim();
                }
                foreach (var folder in _folders)
                {
                    int count = 0;
                    foreach (var folder2 in _folders)
                    {
                        if (folder.newFolderName == folder2.newFolderName)
                        {
                            count++;
                        }
                    }
                    if (count > 1)
                    {
                        FolderError.Items.Add("Trùng tên với một trong các folder khác.");
                    }
                    else
                    {
                        FolderError.Items.Add("Hợp lệ.");
                    }
                }
            }
        }

    }
}
