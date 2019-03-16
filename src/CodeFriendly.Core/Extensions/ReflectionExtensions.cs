using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
namespace CodeFriendly.Core
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<FieldInfo> GetConstants(this Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Static |
                                  BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
        }
        
        public static bool IsSubclassOfRawGeneric(this Type toCheck, Type generic) {
            while (toCheck != null && toCheck != typeof(object)) {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur) {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
        public static PropertyInfo GetNestedProperty(this Type type, string propertyPath, bool ignoreCase = false)
        {
            var nameParts = propertyPath.Split('.');
            var first = nameParts.First();
            var prop = ignoreCase ? type.GetPropertyIgnoreCase(first): type.GetProperty(first);
            return prop != null
                ? (nameParts.Length > 1
                    ? prop.PropertyType.GetNestedProperty(string.Join(".", nameParts.Skip(1)))
                    : prop)
                : null;
        }

        public static PropertyInfo GetPropertyIgnoreCase(this Type type, string property)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(p => string.Equals(p.Name, property, StringComparison.OrdinalIgnoreCase));
        }
        
        public static object GetPropertyValue(this object obj, string propertyPath, bool ignoreCase = false)
        {
            var prop = obj.GetType().GetNestedProperty(propertyPath, ignoreCase);
            var nameParts = propertyPath.Split('.');
            var root = nameParts.Length > 1 ? obj.GetPropertyValue(string.Join(".", nameParts.SkipLast(1)), ignoreCase) : obj;
            return prop?.GetValue(root);
        }

        public static T GetPropertyValue<T>(this object obj, string propertyPath, T defaultValue = default(T), bool ignoreCase = false)
        {
            var value = obj.GetPropertyValue(propertyPath, ignoreCase);
            return value != null ? ExtendedConvert.ChangeType<T>(value) : defaultValue;
        }

        public static void SetPropertyValue(this object obj, string propertyPath, object value, bool ignoreCase = false)
        {
            var prop = obj.GetType().GetNestedProperty(propertyPath, ignoreCase);
            var nameParts = propertyPath.Split('.');
            var root = nameParts.Length > 1 ? obj.GetPropertyValue(string.Join(".", nameParts.SkipLast(1)), ignoreCase) : obj;
            prop?.SetValue(root, value);
        }

        public static TPropertyType GetValueOfPropertyWithAttribute<TAttr, TPropertyType>(this object obj) where TAttr : Attribute
        {
            var prop = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(p => p.GetCustomAttribute<TAttr>() != null);
            return prop !=null ? ExtendedConvert.ChangeType<TPropertyType>(prop.GetValue(obj)) : default(TPropertyType);
        }
    }
}