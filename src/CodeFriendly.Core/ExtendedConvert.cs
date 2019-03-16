using System;
using System.ComponentModel;

namespace CodeFriendly.Core
{
    public static class ExtendedConvert
    {
        public static object ChangeType(object value, Type targetType)
        {
            if ((targetType == typeof(bool) || targetType == typeof(bool?)) &&
                value is string s &&
                decimal.TryParse(s, out var numericValue))
            {
                return numericValue != 0;
            }
            var valueType = value.GetType();
            var c1 = TypeDescriptor.GetConverter(valueType);
            if (c1.CanConvertTo(targetType)) // this returns false for string->bool
            {
                return c1.ConvertTo(value, targetType);
            }
            var c2 = TypeDescriptor.GetConverter(targetType);
            return c2.CanConvertFrom(valueType) ? c2.ConvertFrom(value) : Convert.ChangeType(value, targetType);
        }

        public static T ChangeType<T>(object value)
        {
            return value !=null ? (T)ChangeType(value, typeof(T)) : default(T);
        }
    }
}