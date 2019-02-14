using Radio.Helpers;
using Radio.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Radio.Converters
{
    class GifPathConverter : IValueConverter
    {
        LocalDownoloadHelper downoloadHelper = new LocalDownoloadHelper();
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            Gif gif = (Gif)value;
            gif.HaveLocalPath = downoloadHelper.CheckLocalPath(gif.LocalPath);
            if (gif.HaveLocalPath)
            {
                return gif.LocalPath;
            }
            else
            {
                return gif.Url;
            }
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
