using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Sample.Core.CustomControls
{
    /// <summary>
    /// リピートボタンにイメージを配置した
    /// </summary>
    public class ImageRepeatButton : RepeatButton
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        static ImageRepeatButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageRepeatButton), new FrameworkPropertyMetadata(typeof(ImageRepeatButton)));
        }

        /// <summary>
        /// 通常画像
        /// </summary>
        public ImageSource RepeatEnableImage
        {
            get { return (ImageSource)GetValue(RepeatEnableImageProperty); }
            set { SetValue(RepeatEnableImageProperty, value); }
        }
        public static readonly DependencyProperty RepeatEnableImageProperty =
            DependencyProperty.Register(nameof(RepeatEnableImage), typeof(ImageSource), typeof(ImageRepeatButton), new PropertyMetadata(null));

        /// <summary>
        /// クリック時の画像
        /// </summary>
        public ImageSource RepeatClickImage
        {
            get { return (ImageSource)GetValue(RepeatClickImageProperty); }
            set { SetValue(RepeatClickImageProperty, value); }
        }
        public static readonly DependencyProperty RepeatClickImageProperty =
            DependencyProperty.Register(nameof(RepeatClickImage), typeof(ImageSource), typeof(ImageRepeatButton), new PropertyMetadata(null));

        /// <summary>
        /// ホバー時の画像
        /// </summary>
        public ImageSource RepeatHoverImage
        {
            get { return (ImageSource)GetValue(RepeatHoverImageProperty); }
            set { SetValue(RepeatHoverImageProperty, value); }
        }
        public static readonly DependencyProperty RepeatHoverImageProperty =
            DependencyProperty.Register(nameof(RepeatHoverImage), typeof(ImageSource), typeof(ImageRepeatButton), new PropertyMetadata(null));

        /// <summary>
        /// 無効時の画像
        /// </summary>
        public ImageSource RepeatDisableImage
        {
            get { return (ImageSource)GetValue(RepeatDisableImageProperty); }
            set { SetValue(RepeatDisableImageProperty, value); }
        }
        public static readonly DependencyProperty RepeatDisableImageProperty =
            DependencyProperty.Register(nameof(RepeatDisableImage), typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));
    }
}
