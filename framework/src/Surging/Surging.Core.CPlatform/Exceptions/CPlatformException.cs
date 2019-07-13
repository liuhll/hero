using Surging.Core.CPlatform.Messages;
using System;

namespace Surging.Core.CPlatform.Exceptions
{
    /// <summary>
    /// 基础异常类。
    /// </summary>
    public class CPlatformException : Exception
    {
        private readonly StatusCode _exceptionCode;
        /// <summary>
        /// 初始化构造函数
        /// </summary>
        /// <param name="message">异常消息。</param>
        /// <param name="innerException">内部异常。</param>
        //public CPlatformException(string message, Exception innerException = null) : base(message, innerException)
        //{
        //    _exceptionCode = StatusCode.CPlatformError;
        //}

        public CPlatformException(string message, Exception innerException = null, StatusCode status = StatusCode.CPlatformError) : base(message, innerException)
        {
            _exceptionCode = status;
        }

        public CPlatformException(string message, StatusCode status = StatusCode.CPlatformError) : base(message)
        {
            _exceptionCode = status;
        }

        public StatusCode ExceptionCode { get { return _exceptionCode; } }
    }
}