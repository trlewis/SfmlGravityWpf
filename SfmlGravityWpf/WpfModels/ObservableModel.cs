﻿namespace SfmlGravityWpf.WpfModels
{
    using Code.Helpers;
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

    public abstract class ObservableModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged == null)
                return;

            var args = new PropertyChangedEventArgs(propertyName);
            this.PropertyChanged(this, args);
        }

        protected void NotifyPropertyChanged(Expression<Func<object>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException("propertyExpression");

            var propertyName = PropertyHelper.GetPropertyName(propertyExpression);
            this.NotifyPropertyChanged(propertyName);
        }
    }
}
