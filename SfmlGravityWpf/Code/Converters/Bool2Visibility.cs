namespace SfmlGravityWpf.Code.Converters
{
    using Code.Extensions;
    using Code.Helpers;
    using System;
    using System.Windows;
    using System.Windows.Data;

    public class Bool2Visibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var isInverse = this.IsInverse(parameter);
            if (value == null)
                return isInverse ? Visibility.Visible : Visibility.Collapsed;

            var b = (bool)value;
            return VisibilityHelper.FromValue(b, isInverse);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
