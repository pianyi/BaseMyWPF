using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sample.Core.CustomControls
{
    /// <summary>
    /// ボタンにイメージを配置した
    /// </summary>
    public class ImageButton : Button
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        /// <summary>
        /// 通常画像
        /// </summary>
        public ImageSource EnableImage
        {
            get { return (ImageSource)GetValue(EnableImageProperty); }
            set { SetValue(EnableImageProperty, value); }
        }
        public static readonly DependencyProperty EnableImageProperty =
            DependencyProperty.Register(nameof(EnableImage), typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));

        /// <summary>
        /// クリック時の画像
        /// </summary>
        public ImageSource ClickImage
        {
            get { return (ImageSource)GetValue(ClickImageProperty); }
            set { SetValue(ClickImageProperty, value); }
        }
        public static readonly DependencyProperty ClickImageProperty =
            DependencyProperty.Register(nameof(ClickImage), typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));

        /// <summary>
        /// ホバー時の画像
        /// </summary>
        public ImageSource HoverImage
        {
            get { return (ImageSource)GetValue(HoverImageProperty); }
            set { SetValue(HoverImageProperty, value); }
        }
        public static readonly DependencyProperty HoverImageProperty =
            DependencyProperty.Register(nameof(HoverImage), typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));

        /// <summary>
        /// 無効時の画像
        /// </summary>
        public ImageSource DisableImage
        {
            get { return (ImageSource)GetValue(DisableImageProperty); }
            set { SetValue(DisableImageProperty, value); }
        }
        public static readonly DependencyProperty DisableImageProperty =
            DependencyProperty.Register(nameof(DisableImage), typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));
    }
}
