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

namespace WpfFileExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow ()
        {
            InitializeComponent ();
        }

        private void Window_Loaded (object sender, RoutedEventArgs e)
        {
            foreach (string drive in Directory.GetLogicalDrives ())
            {
                TreeViewItem item = new TreeViewItem ()
                {
                    Header = drive,
                    Tag=drive
                };

                item.Items.Add (null);

                item.Expanded += Folder_Expanded;

                FolderView.Items.Add (item);
            }
        }

        private void Folder_Expanded (object sender, RoutedEventArgs e)
        {
            #region Initialization
            TreeViewItem item = (TreeViewItem) sender;

            if (item.Items.Count != 1 || item.Items[0] != null)
            {
                return;
            }

            item.Items.Clear ();

            string fullPath = item.Tag.ToString ();
            #endregion

            #region Get Directories
            List<string> directories = new List<string> ();

            try
            {
                directories.AddRange (Directory.GetDirectories (fullPath));
            }
            catch { }

            directories.ForEach (directoryPath =>
            {
                TreeViewItem subItem = new TreeViewItem ()
                {
                    Header = GetFileFolderName (directoryPath),
                    Tag = directoryPath
                };

                subItem.Items.Add (null);

                subItem.Expanded += Folder_Expanded;

                item.Items.Add (subItem);
            });
            #endregion

            #region Get Files
            List<string> files = new List<string> ();

            try
            {
                files.AddRange (Directory.GetFiles(fullPath));
            }
            catch { }

            files.ForEach (filePath =>
            {
                TreeViewItem subItem = new TreeViewItem ()
                {
                    Header = GetFileFolderName (filePath),
                    Tag = filePath
                };

                item.Items.Add (subItem);
            });
            #endregion
        }

        public static string GetFileFolderName (string path)
        {
            if (string.IsNullOrEmpty (path))
            {
                return string.Empty;
            }

            string normalizedPath = path.Replace ('/', '\\');

            int lastIndex = normalizedPath.LastIndexOf ('\\');

            if (lastIndex <= 0)
            {
                return path;
            }

            return path.Substring (lastIndex + 1);
        }
    }   
}
