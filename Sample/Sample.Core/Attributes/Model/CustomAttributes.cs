using Sample.Core.Enums;
using System;

namespace Sample.Core.Attributes.Model
{
    /// <summary>
    /// 実行可能かどうかを保持します
    /// </summary>
    public static class CustomAttributes
    {
        /// <summary>
        /// 実行可能かの判断属性
        /// </summary>
        [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
        public sealed class UseAttribute : Attribute
        {
            /// <summary>
            /// 実行可能フラグ
            /// </summary>
            public bool Use { get; private set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="canUse">実行可能フラグ</param>
            public UseAttribute(bool canUse)
            {
                Use = canUse;
            }
        }

        /// <summary>
        /// 実行できるかどうかを判断します
        /// </summary>
        /// <param name="value">対象のEnum</param>
        /// <returns>Enumに設定されているメッセージコード</returns>
        public static bool CanUse(this Enum value)
        {
            return value.GetAttribute<UseAttribute>()?.Use ?? false;
        }
    }
}
