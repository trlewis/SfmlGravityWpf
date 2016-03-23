namespace SfmlGravityWpf.Code.Helpers
{
    using System.Windows;

    public static class VisibilityHelper
    {
        public static Visibility FromValue(bool value, bool isInverse = false)
        {
            if (isInverse)
                return value ? Visibility.Collapsed : Visibility.Visible;
            return value ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
