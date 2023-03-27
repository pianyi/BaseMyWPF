using Sample.Core.Enums;
using System;
using System.Windows;

namespace Sample.Core.Attributes
{
    /// <summary>
    /// メッセージコードを保持し、メッセージを取得できるようにします
    /// </summary>
    public static class MessageCodeAttributes
    {
        /// <summary>
        /// メッセージコード属性
        /// </summary>
        [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
        public sealed class MessageCodeAttribute : Attribute
        {
            /// <summary>
            /// メッセージコード保持
            /// </summary>
            public string MessageCode { get; private set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="Code">設定するメッセージコード</param>
            public MessageCodeAttribute(string Code)
            {
                this.MessageCode = Code;
            }

            /// <summary>
            /// 該当メッセージを取得します
            /// </summary>
            public string Message
            {
                get
                {
                    return Application.Current.Resources[MessageCode]?.ToString() ?? string.Empty;
                }
            }
        }

        /// <summary>
        /// メッセージコード属性の取得
        /// </summary>
        /// <param name="value">対象のEnum</param>
        /// <returns>Enumに設定されているメッセージコード</returns>
        public static string GetMessageCode(this Enum value)
        {
            return value.GetAttribute<MessageCodeAttribute>()?.MessageCode ?? string.Empty;
        }

        /// <summary>
        /// メッセージを取得します
        /// </summary>
        /// <param name="value">対象のEnum</param>
        /// <returns>リソースに設定されている値</returns>
        public static string GetMessage(this Enum value)
        {
            return value.GetAttribute<MessageCodeAttribute>()?.Message ?? string.Empty;
        }
    }
}
