using Microsoft.Xaml.Behaviors;
using Sample.Core.CustomControls.Adorners;
using System.Windows;
using System.Windows.Documents;

namespace Sample.Core.CustomControls.Behaviors
{
    /// <summary>
    /// リサイズ用グリッド付き
    /// </summary>
    public class GripperBehavior : Behavior<FrameworkElement>
    {
        private Adorner _adorner;

        /// <summary>
        /// データアタッチ
        /// </summary>
        protected override void OnAttached()
        {
            _adorner = new Gripper(AssociatedObject);

            base.OnAttached();
            if (AssociatedObject.IsLoaded)
            {
                AdornerLayer.GetAdornerLayer(AssociatedObject)?.Add(_adorner);
            }
            else
            {
                AssociatedObject.Loaded += (s, e) =>
                {
                    AdornerLayer.GetAdornerLayer(AssociatedObject)?.Add(_adorner);
                };
            }
        }

        /// <summary>
        /// データ破棄
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AdornerLayer.GetAdornerLayer(AssociatedObject)?.Remove(_adorner);

        }
    }
}
