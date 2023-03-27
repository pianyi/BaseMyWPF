using System.Windows;

namespace Sample.Core.CustomControls
{
    /// <summary>
    /// コンテキストメニューなどが初期表示されない問題に対応するためのプロキシ
    /// </summary>
    public class BindingProxy : Freezable
    {
        /// <summary>
        /// インスタンス
        /// </summary>
        /// <returns>インスタンス</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        /// <summary>
        /// Freezableするデータ
        /// </summary>
        public object Data
        {
            get
            {
                return GetValue(DataProperty);
            }
            set
            {
                SetValue(DataProperty, value);
            }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));
    }
}
