namespace SfmlGravityWpf.Code.Extensions
{
    using System.Windows;
    using System.Windows.Data;

    public static class UserControlExtensions
    {
        public static void BindDataContext(this FrameworkElement userControl, object source)
        {
            userControl.DataContext = null;
            userControl.SetBinding(FrameworkElement.DataContextProperty, new Binding { Source = source });
        }
    }
}
