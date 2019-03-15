using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;

namespace Friendly.Core
{
    public static class StringExtensions
    {
        public static bool IsEmail(this string toCheck)
        {
            const string regexEmailPattern = "^.+@.+\\..+$";
            return Regex.IsMatch(toCheck, regexEmailPattern);
        }

        public static string FormatWith(this string str, params object[] format)
        {
            return string.Format(str, format);
        }

        public static string WrapWithChar(this string str, char wrap = ' ')
        {
            return string.Format("{0}{1}{0}", wrap, str);
        }

        public static string EnsureLeadingChar(this string str, char character)
        {
            return !string.IsNullOrEmpty(str) && !str.StartsWith(character) ? $"{character}{str}" : str;
        }
        
        public static string EnsureTrailingChar(this string str, char character)
        {
            return !string.IsNullOrEmpty(str) && !str.EndsWith(character) ? $"{str}{character}" : str;
        }
        
        public static string EnsureLeadingSlash(this string str)
        {
            return str.EnsureLeadingChar('/');
        }
        
        public static string EnsureTrailingSlash(this string str)
        {
            return str.EnsureTrailingChar('/');
        }

        public static string ToTitleCase(this string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
        }


        public static SecureString ToSecureString(this string val)
        {
            var secure = new SecureString();
            foreach (var c in val)
            {
                secure.AppendChar(c);
            }

            return secure;
        }

        public static string SecureStringToString(this SecureString value)
        {
            var valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
        
        public static bool IsBase64(this string base64String)
        {
            
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0
                || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
                return false;

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch(Exception)
            {
                // Handle the exception
            }
            return false;
        }

        public static bool IsNumeric(this string val)
        {
            var isNumeric = int.TryParse(val, out int n);
            return isNumeric;
        }
    }
}