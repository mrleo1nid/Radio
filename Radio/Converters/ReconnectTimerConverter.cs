using System;
using System.Globalization;
using System.Windows.Data;

namespace Radio.Converters
{
    class ReconnectTimerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            int time = (int)value;
            return $"Отсуствует соединение с сервером! Переподключение через {time} секунд.";
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
