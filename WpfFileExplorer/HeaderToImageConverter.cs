using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WpfFileExplorer
{
    [ValueConversion (typeof (string), typeof (BitmapImage))]
    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter ();

        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = value.ToString ();

            if (path == null)
            {
                return null;
            }

            string name = MainWindow.GetFileFolderName (path);

            string image = "file.png";

            if (string.IsNullOrEmpty (name))
            {
                image = "drive.png";
            }
            else if (new FileInfo (path).Attributes.HasFlag (FileAttributes.Directory))
            {
                image = "folder.png";
            }

            return new BitmapImage (new Uri ($"pack://application:,,,/Images/{ image }"));
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException ();
        }
    }
}
