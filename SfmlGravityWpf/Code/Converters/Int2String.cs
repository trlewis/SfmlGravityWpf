namespace SfmlGravityWpf.Code.Converters
{
    using System;
    using System.Windows.Data;

    public class Int2String : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var i = value as int?;
            if (i == null)
                return string.Empty;

            return i.ToString();
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var str = value as string;
            if (string.IsNullOrWhiteSpace(str))
                return null;

            int i;
            if (int.TryParse(str, out i))
                return i;

            return null;
        }
    }
}
