using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Sample.Core.CustomControls.Adorners
{
    /// <summary>
    /// コントロールにリサイズ用グリッドを追加します。
    /// </summary>
    public class Gripper : Adorner
    {
        /// <summary>
        /// リサイズ用グリッドクラス
        /// </summary>
        /// <remarks>
        /// ドラッグイベントが用意されていて便利なのでThumbコントロールを利用します。
        /// </remarks>
        private readonly Thumb _resizeGrip;
        /// <summary>
        /// 
        /// </summary>
        private readonly VisualCollection _visualChildren;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adornedElement">対象エレメント</param>
        /// <param name="controlTemplate">リサイズグリッドのテンプレート</param>
        public Gripper(UIElement adornedElement, ControlTemplate controlTemplate = null) : base(adornedElement)
        {
            _resizeGrip = new Thumb()
            {
                Cursor = Cursors.SizeNWSE
            };
            _resizeGrip.SetValue(WidthProperty, 18d);
            _resizeGrip.SetValue(HeightProperty, 18d);
            _resizeGrip.DragDelta += OnGripDelta;
            _resizeGrip.Template = controlTemplate ?? MakeDefaultGripTemplate();
            _visualChildren = new VisualCollection(this)
            {
                _resizeGrip
            };
        }

        /// <summary>
        /// デフォルトのマーカーを作成する
        /// </summary>
        /// <returns>デフォルトマーカー(青〇？)</returns>
        private ControlTemplate MakeDefaultGripTemplate()
        {
            // 指定なしの場合の見た目を作成
            FrameworkElementFactory visualTree = new(typeof(Border));
            visualTree.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
            visualTree.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Center);
            visualTree.SetValue(WidthProperty, 12d);
            visualTree.SetValue(HeightProperty, 12d);
            visualTree.SetValue(Border.BackgroundProperty, new SolidColorBrush(Colors.RoyalBlue));
            visualTree.SetValue(Border.CornerRadiusProperty, new CornerRadius(6));
            return new ControlTemplate(typeof(Thumb))
            {
                VisualTree = visualTree
            };
        }

        /// <summary>
        /// Thumb のドラッグイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGripDelta(object sender, DragDeltaEventArgs e)
        {
            if (AdornedElement is FrameworkElement frameworkElement)
            {
                var w = frameworkElement.Width;
                var h = frameworkElement.Height;
                if (w.Equals(double.NaN))
                {
                    w = frameworkElement.DesiredSize.Width;
                }
                if (h.Equals(double.NaN))
                {
                    h = frameworkElement.DesiredSize.Height;
                }
                w += e.HorizontalChange;
                h += e.VerticalChange;
                // clamp
                w = Math.Max(_resizeGrip.Width, w);
                h = Math.Max(_resizeGrip.Height, h);
                w = Math.Max(frameworkElement.MinWidth, w);
                h = Math.Max(frameworkElement.MinHeight, h);
                w = Math.Min(frameworkElement.MaxWidth, w);
                h = Math.Min(frameworkElement.MaxHeight, h);
                // ※ = で入れるとBindingが外れるので注意
                frameworkElement.SetValue(WidthProperty, w);
                frameworkElement.SetValue(HeightProperty, h);
            }
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (AdornedElement is FrameworkElement frameworkElement)
            {
                var w = _resizeGrip.Width;
                var h = _resizeGrip.Height;
                var x = frameworkElement.ActualWidth - w;
                var y = frameworkElement.ActualHeight - h;
                _resizeGrip.Arrange(new Rect(x, y, w, h));
            }
            return finalSize;
        }

        /// <inheritdoc/>
        protected override int VisualChildrenCount => _visualChildren.Count;

        /// <inheritdoc/>
        protected override Visual GetVisualChild(int index)
        {
            return _visualChildren[index];
        }
    }
}
