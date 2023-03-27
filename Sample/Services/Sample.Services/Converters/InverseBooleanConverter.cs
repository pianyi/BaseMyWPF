using System;
using System.Globalization;
using System.Windows.Data;

namespace Sample.Services.Converters
{
    /// <summary>
    /// bool値を逆にして取得するコンバーター
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">表示元の値</param>
        /// <param name="targetType">データ型</param>
        /// <param name="parameter">画面からのパラメータ</param>
        /// <param name="culture">国情報(必ずen-USが来ます)</param>
        /// <returns>表示するデータ</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
            {
                throw new InvalidOperationException("The target must be a boolean");
            }

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
