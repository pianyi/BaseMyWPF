using System;

namespace Sample.Core.Exceptions
{
    /// <summary>
    /// 入力値エラー例外
    /// </summary>
    public class InputErrorException : Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InputErrorException()
            : base()
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        public InputErrorException(string message)
            : base(message)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="innerException">例外</param>
        public InputErrorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
