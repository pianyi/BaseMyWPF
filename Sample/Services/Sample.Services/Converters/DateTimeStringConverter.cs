using System;
using System.Globalization;
using System.Windows.Data;

namespace Sample.Services.Converters
{
    public class DateTimeStringConverter : IValueConverter
    {
        /// <summary>
        /// 出力する日付フォーマットを変換します
        /// </summary>
        /// <param name="value">表示元の値</param>
        /// <param name="targetType">データ型</param>
        /// <param name="parameter">画面からのパラメータ</param>
        /// <param name="culture">国情報(必ずen-USが来ます)</param>
        /// <returns>表示するデータ</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // valueがDateTime型の時
            if (value != null && value is DateTime time)
            {
                // parameterのみ指定されている時
                if (parameter != null)
                {
                    return time.ToString(parameter.ToString());
                }

                // 何も指定されていない時
                return value.ToString();
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
