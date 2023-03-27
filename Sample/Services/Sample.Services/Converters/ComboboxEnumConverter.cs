using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using static Sample.Core.Attributes.MessageCodeAttributes;

namespace Sample.Services.Converters
{
    public class ComboboxEnumConverter : IValueConverter
    {
        /// <summary>
        /// Enum型を文字列型に変換します
        /// </summary>
        /// <param name="value">表示元の値</param>
        /// <param name="targetType">データ型</param>
        /// <param name="parameter">画面からのパラメータ</param>
        /// <param name="culture">国情報(必ずen-USが来ます)</param>
        /// <returns>表示するデータ</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return DependencyProperty.UnsetValue;
            }

            return GetDescription((Enum)value);
        }

        /// <summary>
        /// 文字列型をEnum型に変換します
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.ToObject(targetType, value);
        }

        /// <summary>
        /// Enum型から対応したメッセージを取得します
        /// </summary>
        /// <param name="en">変換対象のenum型</param>
        /// <returns>変換後の文字列</returns>
        public string GetDescription(Enum en)
        {
            MemberInfo[] memInfo = en.GetType().GetMember(en.ToString());
            if (memInfo != null && 0 < memInfo.Length)
            {
                // メッセージコードが設定されているかを判定
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(MessageCodeAttribute), false);
                if (attrs != null && 0 < attrs.Length)
                {
                    // メッセージコードに対応するメッセージを取得
                    return ((MessageCodeAttribute)attrs[0]).Message;
                }
            }

            // 存在しない場合はEnumのコードを表示する
            return en.ToString();
        }
    }
}
