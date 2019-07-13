using Surging.Core.CPlatform.Configurations;
using Surging.Core.CPlatform.Messages;
using System;

namespace Surging.Core.CPlatform.Exceptions
{
    public static class ExceptionExtension
    {
        public  static  string GetExceptionMessage(this Exception exception) 
        {
            if (exception == null)
                return string.Empty;

            var message = exception.Message;
            if (AppConfig.ServerOptions.Environment == RuntimeEnvironment.Development 
                && ((exception.GetGetExceptionStatusCode() == StatusCode.CPlatformError)
                && (exception.GetGetExceptionStatusCode() == StatusCode.DataAccessError)
                && (exception.GetGetExceptionStatusCode() == StatusCode.RequestError)
                && (exception.GetGetExceptionStatusCode() == StatusCode.UnKnownError)
                || AppConfig.ServerOptions.ForceDisplayStackTrace))
            {
                message += Environment.NewLine + " 堆栈信息:" + exception.StackTrace;
                if (exception.InnerException != null)
                {
                    message += "|InnerException:" + GetExceptionMessage(exception.InnerException);
                }
            }
            else
            {
                if (exception.InnerException != null)
                {
                    if (exception.InnerException is BusinessException 
                        || exception.InnerException is ValidateException 
                        || exception.InnerException is AuthException
                        || exception.InnerException is UserFriendlyException)
                    {
                        message = exception.InnerException.Message;
                    }
                    else
                    {
                        message += ";" + GetExceptionMessage(exception.InnerException);
                    }
                    
                }
            }

            return message;
        }

        public static StatusCode GetGetExceptionStatusCode(this Exception exception)
        {
            var statusCode = StatusCode.UnKnownError;
            if (exception is CPlatformException)
            {
                statusCode = ((CPlatformException)exception).ExceptionCode;
                return statusCode;
            }
            if (exception.InnerException != null)
            {
                if (exception.InnerException is CPlatformException)
                {
                    statusCode = ((CPlatformException)exception.InnerException).ExceptionCode;
                    return statusCode;
                }
            }
            return statusCode;

        }
    }
}
