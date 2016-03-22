namespace SfmlGravityWpf.Code.Helpers
{
    using System;
    using System.Linq.Expressions;

    public static class PropertyHelper
    {
        public static string GetPropertyName(Expression<Func<object>> propertyExpression)
        {
            var unaryExpression = propertyExpression.Body as UnaryExpression;
            var memberExpression = unaryExpression == null
                ? (MemberExpression) propertyExpression.Body
                : (MemberExpression) unaryExpression.Operand;

            var propertyName = memberExpression.Member.Name;
            return propertyName;
        }
    }
}
