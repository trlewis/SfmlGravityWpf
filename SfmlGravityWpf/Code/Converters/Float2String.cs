namespace SfmlGravityWpf.Code.Converters
{
    using System;
    using System.Windows.Data;

    public class Float2String : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var fl = value as float?;
            if (fl == null)
                return string.Empty;

            return fl.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var str = value as string;
            if (string.IsNullOrWhiteSpace(str))
                return null;

            float fl;
            if (float.TryParse(str, out fl))
                return fl;

            return null;
        }
    }
}
