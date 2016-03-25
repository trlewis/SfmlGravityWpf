namespace SfmlGravityWpf.Code.Extensions
{
    using System;
    using System.Windows.Data;

    public static class ValueConverterExtensions
    {
        public static bool IsInverse(this IValueConverter valueConverter, object parameter)
        {
            var inverse = parameter as string;
            return inverse != null && inverse.Equals("inverse", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
