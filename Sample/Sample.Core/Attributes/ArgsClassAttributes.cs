using Sample.Core.Enums;
using System;

namespace Sample.Core.Attributes
{
    /// <summary>
    /// 引数専用クラス保持
    /// </summary>
    public static class ArgsClassAttributes
    {
        /// <summary>
        /// イベントクラス属性
        /// </summary>
        [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
        public sealed class ArgsClassAttribute : Attribute
        {
            /// <summary>
            /// イベントクラス保持
            /// </summary>
            public Type ArgsClass { get; private set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="clazz">設定するイベントクラス名称</param>
            public ArgsClassAttribute(Type clazz)
            {
                this.ArgsClass = clazz;
            }
        }

        /// <summary>
        /// イベントクラス属性の取得
        /// </summary>
        /// <param name="value">対象のEnum</param>
        /// <returns>Enumに設定されているイベントクラス</returns>
        public static Type GetArgsClass<T>(this Enum value)
        {
            return value.GetAttribute<ArgsClassAttribute>()?.ArgsClass ?? typeof(T);
        }
    }
}
