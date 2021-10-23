using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FolderExplorer_
{
    public partial class MainWindow : Window
    {
        List<FolderControl> dirs = new List<FolderControl>();
        string currentDir;

        public MainWindow()
        {
            InitializeComponent();
            //MessageBox.Show(ByteSimplifier(DirSize(new DirectoryInfo(@"F:\Documents\CodeProjects"))).Item1.ToString());
            BackButton.Click += (s, e) => PrevDir();
        }

        private void UpdateList()
        {
            for (int i = 0; i < dirs.Count; i++)
            {
                FolderControl _folderControl = dirs[i];
                if (_folderControl.dirItem.Size < 0)
                {
                    _folderControl.FolderName.Foreground = System.Windows.Media.Brushes.Red;
                    _folderControl.FolderName.Text = _folderControl.dirItem.Name;
                    _folderControl.Size.Text = $"N/A";
                    _folderControl.Perc.Value = 0;
                }
                else if (_folderControl.dirItem.Size == long.MaxValue)
                {
                    _folderControl.FolderName.Text = _folderControl.dirItem.Name;
                    _folderControl.Size.Text = $"Getting Size...";
                    _folderControl.Perc.Value = 0;
                }
                else
                {
                    var _formattedSize = ByteSimplifier(_folderControl.dirItem.Size);
                    _folderControl.FolderName.Text = _folderControl.dirItem.Name;
                    _folderControl.Size.Text = $"{_formattedSize.Item1}{_formattedSize.Item2}";
                    _folderControl.Perc.Value = GetPerc(_folderControl.dirItem.Size);
                }
            }

            Folders.Children.Clear();
            foreach (var _folderControl in dirs)
            {
                Folders.Children.Add(_folderControl);
            }
        }

        public async void GetDirs(string _path)
        {
            _path = _path.Replace('/', '\\');
            string[] _dirPaths;
            try
            {
                _dirPaths = Directory.GetDirectories(_path);
            }
            catch
            {
                MessageBox.Show("Invalid Path");
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Status.Text = "Getting Folder Sizes...";
            List<Task> tasks = new List<Task>();

            //Folders
            try
            {
                currentDir = _path;
                Input.Text = _path;
                dirs.Clear();
                foreach (string _dirPath in _dirPaths)
                {
                    FolderControl _folderControl = new FolderControl();
                    _folderControl.mainWindow = this;
                    _folderControl.dirItem.Name = _dirPath;
                    dirs.Add(_folderControl);

                    tasks.Add(Task.Run(GetDirSize(_dirPath, _folderControl)));
                }
                //return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //return false;
            }


            //Files
            DirectoryInfo d = new DirectoryInfo(_path);
            foreach (var _file in d.GetFiles())
            {
                FolderControl _folderControl = new FolderControl();
                _folderControl.mainWindow = this;
                _folderControl.dirItem.Name = _file.FullName;
                _folderControl.dirItem.canLaunch = true;
                dirs.Add(_folderControl);

                tasks.Add(Task.Run(GetFileSize(_file.FullName, _folderControl)));
            }

            UpdateList();
            await Task.WhenAll(tasks);
            stopwatch.Stop();
            Status.Text = $"All Folders Done in {Math.Round(stopwatch.Elapsed.TotalSeconds, 3)}s";
        }

        public static long DirSize(DirectoryInfo d)
        {
            try
            {
                long size = 0;
                // Add file sizes.
                FileInfo[] fis = d.GetFiles();
                foreach (FileInfo fi in fis)
                {
                    size += fi.Length;
                }
                // Add subdirectory sizes.
                DirectoryInfo[] dis = d.GetDirectories();
                foreach (DirectoryInfo di in dis)
                {
                    size += DirSize(di);
                }
                return size;
                }
            catch
            {
                return long.MinValue;
            }
        }

        private void Sort(bool _isDecending)
        {
            if (_isDecending)
            {
                dirs.Sort((pair1, pair2) => pair1.dirItem.Size.CompareTo(pair2.dirItem.Size));
            }
            else
            {
                dirs.Sort((pair1, pair2) => pair2.dirItem.Size.CompareTo(pair1.dirItem.Size));
            }
            UpdateList();
        }

        private void PrevDir()
        {
            if (string.IsNullOrEmpty(currentDir)) return;
            int i = currentDir.IndexOf(@"\");
            var tempDir = currentDir.Substring(i - 1, currentDir.Length - 1);

            if (tempDir.Length > 2)
            {
                int index = currentDir.LastIndexOf(@"\");
                if (index > 0)
                {
                    string prevDir = currentDir.Substring(0, index);
                    prevDir = prevDir.Length <= 2 ? currentDir.Substring(0, index + 1) : prevDir;
                    GetDirs(prevDir);
                }
            }
        }

        private float GetPerc(float _value)
        {
            long _totalSize = 0;
            for (int i = 0; i < dirs.Count; i++)
            {
                if (dirs[i].dirItem.Size > -1 && dirs[i].dirItem.Size != long.MaxValue)
                {
                    _totalSize += dirs[i].dirItem.Size;
                }
            }
            var perc = _value / (float)_totalSize;
            perc = perc < 0.01 ? 0 : perc;
            return perc * 1000;
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Folders.Children.Clear();
                GetDirs(Input.Text);
            }
        }

        private void SortOption_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (SortOption.SelectedIndex)
            {
                case 0:
                    GetDirs(currentDir);
                    break;
                case 1:
                    Sort(false);
                    break;
                case 2:
                    Sort(true);
                    break;
            }
        }

        Tuple<float, string> ByteSimplifier(long _bytes)
        {
            int unit = 0;
            float _newValue = _bytes;
            while (_newValue > 1024 && unit < 3)
            {
                _newValue /= 1024;
                unit++;
            }

            switch (unit)
            {
                case 0:
                    return new Tuple<float, string>((float)Math.Round(_newValue, 2), "B");
                case 1:
                    return new Tuple<float, string>((float)Math.Round(_newValue, 2), "KB");
                case 2:
                    return new Tuple<float, string>((float)Math.Round(_newValue, 2), "MB");
                case 3:
                    return new Tuple<float, string>((float)Math.Round(_newValue, 2), "GB");
            }
            return null;
        }

        public Action GetDirSize(string _dirPath, FolderControl _folderControl)
        {
            return new Action(() => {
                long _size = DirSize(new DirectoryInfo(_dirPath));

                _folderControl.dirItem.Size = _size;

                Dispatcher.Invoke(() =>
                {
                    if (SortOption.SelectedIndex == 1)
                    {
                        Sort(false);
                    }
                    else if (SortOption.SelectedIndex == 2)
                    {
                        Sort(true);
                    }
                    UpdateList();
                });
            });
        }

        public Action GetFileSize(string _filePath, FolderControl _folderControl)
        {
            return new Action(() => {
                FileInfo _file = new FileInfo(_filePath);
                _folderControl.dirItem.Size = _file.Length;

                var icon = System.Drawing.Icon.ExtractAssociatedIcon(_file.FullName);
                Bitmap bitmap = icon.ToBitmap();
                IntPtr hBitmap = bitmap.GetHbitmap();

                Dispatcher.Invoke(() =>
                {
                    _folderControl.imgIcon.Source = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                });


                Dispatcher.Invoke(() =>
                {
                    if (SortOption.SelectedIndex == 1)
                    {
                        Sort(false);
                    }
                    else if (SortOption.SelectedIndex == 2)
                    {
                        Sort(true);
                    }
                    UpdateList();
                });
            });
        }
    }
}