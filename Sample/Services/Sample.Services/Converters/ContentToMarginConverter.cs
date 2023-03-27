using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Sample.Services.Converters
{
    /// <summary>
    /// 台形タブ用の描画範囲取得用のコンバーター
    /// </summary>
    public class ContentToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //元の描画の高さの分の厚みを確保する
            return new Thickness(0, 0, -((ContentPresenter)value).ActualHeight, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
