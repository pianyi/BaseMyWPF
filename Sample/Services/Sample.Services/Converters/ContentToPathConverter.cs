using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Sample.Services.Converters
{
    /// <summary>
    /// 台形タブ用Path描画コンバーター
    /// </summary>
    public class ContentToPathConverter : IValueConverter
    {
        /// <summary>
        /// 台形タブ用の点を始点から3点取得するコンバーター
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //始点以外の3点
            var ps = new PathSegmentCollection(3);
            ContentPresenter cp = (ContentPresenter)value;
            //描画する範囲
            double h = cp.ActualHeight * 1.5;
            double w = cp.ActualWidth * 1.4;
            //台形の斜面を本来の幅の２０％分で作成
            ps.Add(new LineSegment(new Point(w * 0.20, h), true));
            ps.Add(new LineSegment(new Point(w * 1.20, h), true));
            ps.Add(new LineSegment(new Point(w * 1.40, 0), true));
            return ps;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
