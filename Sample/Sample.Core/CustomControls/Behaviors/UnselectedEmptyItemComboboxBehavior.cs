using Microsoft.Xaml.Behaviors;
using Sample.Core.CustomControls.Interfaces;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sample.Core.CustomControls.Behaviors
{
    public class UnselectedEmptyItemComboboxBehavior : Behavior<ComboBox>
    {
        /// <summary>
        /// 空白の選択がOKかどうか
        /// </summary>
        public bool IsEmptySelected { get; set; } = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UnselectedEmptyItemComboboxBehavior()
        {
        }

        /// <summary>
        /// コントロールアタッチ
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
            AssociatedObject.PreviewMouseWheel += AssociatedObject_PreviewMouseWheel;
        }

        /// <summary>
        /// コントロールデタッチ
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
            AssociatedObject.PreviewMouseWheel -= AssociatedObject_PreviewMouseWheel;
        }

        /// <summary>
        /// マウスホイール移動時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssociatedObject_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ComboBox combo)
            {
                int index = combo.SelectedIndex;
                if (e.Delta < 0)
                {
                    index++;
                }
                else
                {
                    index--;
                }

                e.Handled = Handled(combo, index, null);
            }
        }

        /// <summary>
        /// キー押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is ComboBox combo)
            {
                int index = combo.SelectedIndex;
                bool? isReverse = null;
                if (e.Key == Key.Up || e.Key == Key.Left)
                {
                    index--;
                }
                else if (e.Key == Key.Down || e.Key == Key.Right)
                {
                    index++;
                }
                else if (e.Key == Key.Home)
                {
                    index = 0;
                    isReverse = false;
                }
                else if (e.Key == Key.End)
                {
                    index = combo.Items.Count;
                    isReverse = true;
                }

                e.Handled = Handled(combo, index, isReverse);
            }
        }

        /// <summary>
        /// 処理済みかどうかを判断します。
        /// </summary>
        /// <param name="combo">元のコンボボックス</param>
        /// <param name="index">処理対象のデータIndex</param>
        /// <param name="isReverse">検索時の正順・逆順</param>
        /// <returns>true:処理済み、false:未処理</returns>
        protected bool Handled(ComboBox combo, int index, bool? isReverse)
        {
            bool result = false;

            if (index < 0 || combo.Items.Count <= index ||
                (combo.Items[index] is ICanDisplayItem item && !item.CanDisplay))
            {
                // 対象データが表示できないデータの場合、表示可能データに変換します。
                if (isReverse != null)
                {
                    combo.SelectedIndex = SearchIndex(combo, isReverse.GetValueOrDefault());
                }
                result = true;
            }

            return result;
        }

        /// <summary>
        /// コンボボックス内から表示できるデータIndexを検索します
        /// </summary>
        /// <param name="combo">コンボボックス</param>
        /// <param name="isReverse">true:正順、false:逆順</param>
        /// <returns>表示できるデータのIndex(存在しない場合は0番目)</returns>
        protected int SearchIndex(ComboBox combo, bool isReverse)
        {
            int result = 0;

            for (int i = 0; i < combo.Items.Count; i++)
            {
                int index = i;
                if (isReverse)
                {
                    index = combo.Items.Count - i - 1;
                }

                if (combo.Items[index] is ICanDisplayItem tmp && tmp.CanDisplay)
                {
                    result = index;
                    break;
                }
            }

            return result;
        }
    }
}
