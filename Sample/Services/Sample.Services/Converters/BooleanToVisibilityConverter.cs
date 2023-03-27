using System.Windows;

namespace Sample.Services.Converters
{
    /// <summary>
    /// BooleanをVisibility に変換します
    /// </summary>
    public sealed class BooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BooleanToVisibilityConverter() :
            base(Visibility.Visible, Visibility.Collapsed)
        {
        }
    }
}
