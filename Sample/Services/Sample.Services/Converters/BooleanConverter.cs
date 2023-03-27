using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Sample.Services.Converters
{
    /// <summary>
    /// Booleanにデータを紐づけるコンバーター
    /// </summary>
    /// <typeparam name="T">変換する型</typeparam>
    public class BooleanConverter<T> : IValueConverter
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="trueValue"></param>
        /// <param name="falseValue"></param>
        public BooleanConverter(T trueValue, T falseValue)
        {
            True = trueValue;
            False = falseValue;
        }

        /// <summary>
        /// true時の設定値
        /// </summary>
        public T True { get; set; }
        /// <summary>
        /// false時の設定値
        /// </summary>
        public T False { get; set; }

        /// <summary>
        /// bool To オブジェクト コンバータ
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolean && boolean ? True : False;
        }

        /// <summary>
        /// オブジェクト To bool コンバータ
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is T t && EqualityComparer<T>.Default.Equals(t, True);
        }
    }
}
