using System;

namespace Friendly.Core
{
    public static class ExceptionExtensions
    {
        public static string DumpMessage(this Exception ex)
        {
            
            var innerEx = ex.InnerException != null && ex.InnerException.Message != ex.Message
                ? ex.InnerException
                : ex.InnerException?.InnerException;
            var inner = innerEx != null;
            var message =  inner ? string.Concat(innerEx.DumpMessage(), " ", ex.Message) : ex.Message;
            return message;
        }
    }
}