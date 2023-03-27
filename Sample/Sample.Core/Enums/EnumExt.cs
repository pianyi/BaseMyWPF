using System;
using System.Linq;

namespace Sample.Core.Enums
{
    /// <summary>
    /// Enum型の拡張機能を定義します
    /// </summary>
    public static class EnumExt
    {
        /// <summary>
        /// 特定の属性を取得する
        /// </summary>
        /// <typeparam name="TAttribute">属性型</typeparam>
        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            // リフレクションを用いて列挙体の型から情報を取得
            var fieldInfo = value.GetType().GetField(value.ToString());

            // 指定した属性のリスト
            var attributes = fieldInfo.GetCustomAttributes(typeof(TAttribute), false)
                                      .Cast<TAttribute>();

            // 属性がなかった場合、空を返す
            if ((attributes?.Count() ?? 0) <= 0)
            {
                return null;
            }

            // 同じ属性が複数含まれていても、最初のみ返す
            return attributes.First();
        }

        /// <summary>
        /// 文字列型からEnum型に変換します
        /// </summary>
        /// <typeparam name="TEnum">変換する型</typeparam>
        /// <param name="src">変換元文字列</param>
        /// <param name="type">変換後の値を入れる変数</param>
        /// <returns>true：成功、false:失敗</returns>
        public static bool TryParse<TEnum>(string src, out TEnum type) where TEnum : struct
        {
            return Enum.TryParse(src, out type) && Enum.IsDefined(typeof(TEnum), type);
        }

        /// <summary>
        /// int型からEnum型に変換します
        /// </summary>
        /// <typeparam name="TEnum">変換する型</typeparam>
        /// <param name="src">変換元数値</param>
        /// <param name="type">変換後の値を入れる変数</param>
        /// <returns>true：成功、false:失敗</returns>
        public static bool TryParse<TEnum>(int src, out TEnum type) where TEnum : struct
        {
            return Enum.TryParse(src.ToString(), out type) && Enum.IsDefined(typeof(TEnum), type);
        }
    }
}
