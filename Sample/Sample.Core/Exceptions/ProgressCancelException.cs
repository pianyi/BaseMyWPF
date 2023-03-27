using System;

namespace Sample.Core.Exceptions
{
    /// <summary>
    /// 処理キャンセル例外
    /// </summary>
    public class ProgressCancelException : Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProgressCancelException()
            : base()
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        public ProgressCancelException(string message)
            : base(message)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="innerException">例外</param>
        public ProgressCancelException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
