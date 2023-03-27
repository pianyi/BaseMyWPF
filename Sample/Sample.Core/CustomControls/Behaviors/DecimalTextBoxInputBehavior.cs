using Microsoft.Xaml.Behaviors;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sample.Core.CustomControls.Behaviors
{
    /// <summary>
    /// テキストボックスを拡張し、小数点が有効なテキストボックスにします
    /// </summary>
    public class DecimalTextBoxInputBehavior : Behavior<TextBox>
    {
        /// <summary>
        /// 入力可能記号など(小数点ドット＋マイナス記号のみOK)
        /// </summary>
        private const NumberStyles _validNumberStyles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DecimalTextBoxInputBehavior()
        {
            InputMode = TextBoxInputMode.None;
            DecimalCount = 0;
            JustPositivDecimalInput = false;
        }

        /// <summary>
        /// フォーマットスタイル
        /// </summary>
        public TextBoxInputMode InputMode { get; set; }

        /// <summary>
        /// 小数点以下の桁数
        /// </summary>
        public int DecimalCount { get; set; }

        /// <summary>
        /// これ何に使うんだろう？
        /// </summary>
        public static readonly DependencyProperty JustPositivDecimalInputProperty =
         DependencyProperty.Register("JustPositivDecimalInput", typeof(bool),
         typeof(DecimalTextBoxInputBehavior), new FrameworkPropertyMetadata(false));

        /// <summary>
        /// これ何に使うんだろう？
        /// </summary>
        public bool JustPositivDecimalInput
        {
            get { return (bool)GetValue(JustPositivDecimalInputProperty); }
            set { SetValue(JustPositivDecimalInputProperty, value); }
        }

        /// <summary>
        /// コントロールアタッチ
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewTextInput += AssociatedObjectPreviewTextInput;
            AssociatedObject.PreviewKeyDown += AssociatedObjectPreviewKeyDown;
            AssociatedObject.LostFocus += AssociatedObjectLostFocus;

            DataObject.AddPastingHandler(AssociatedObject, Pasting);

        }

        /// <summary>
        /// コントロールデタッチ
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewTextInput -= AssociatedObjectPreviewTextInput;
            AssociatedObject.PreviewKeyDown -= AssociatedObjectPreviewKeyDown;
            AssociatedObject.LostFocus -= AssociatedObjectLostFocus;

            DataObject.RemovePastingHandler(AssociatedObject, Pasting);
        }

        /// <summary>
        /// これ何に使うんだろう？
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var pastedText = (string)e.DataObject.GetData(typeof(string));

                if (!IsValidInput(GetText(pastedText)))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        /// <summary>
        /// キーボード入力チェック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AssociatedObjectPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (!IsValidInput(GetText(" ")))
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// データ入力前チェック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AssociatedObjectPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsValidInput(GetText(e.Text)))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// フォーカスアウトした後に動作します
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AssociatedObjectLostFocus(object sender, RoutedEventArgs e)
        {
            if (e.Source is TextBox src && e.OriginalSource is TextBox original)
            {
                // 小数点の有効桁数を表示します
                src.Text = GetDecimalString(original.Text.ToString());
            }
        }

        /// <summary>
        /// 入力データのチェック
        /// </summary>
        /// <param name="input">追加文字</param>
        /// <returns>追加後の文字列</returns>
        string GetText(string input)
        {
            var txt = AssociatedObject;

            int selectionStart = txt.SelectionStart;
            if (txt.Text.Length < selectionStart)
            {
                selectionStart = txt.Text.Length;
            }

            int selectionLength = txt.SelectionLength;
            if (txt.Text.Length < selectionStart + selectionLength)
            {
                selectionLength = txt.Text.Length - selectionStart;
            }

            var realtext = txt.Text.Remove(selectionStart, selectionLength);

            int caretIndex = txt.CaretIndex;
            if (realtext.Length < caretIndex)
            {
                caretIndex = realtext.Length;
            }

            var newtext = realtext.Insert(caretIndex, input);

            return newtext;
        }

        /// <summary>
        /// 追加後の文字列チェック
        /// </summary>
        /// <param name="input">追加後の文字列</param>
        /// <returns>true:OK、false:NG</returns>
        bool IsValidInput(string input)
        {
            switch (InputMode)
            {
                case TextBoxInputMode.None:
                    return true;

                case TextBoxInputMode.DigitInput:
                    return CheckIsDigit(input);

                case TextBoxInputMode.DecimalInput:
                    decimal d;
                    //wen mehr als ein Komma
                    if (input.ToCharArray().Where(x => x == ',').Count() > 1)
                    {
                        return false;
                    }

                    if (input.Contains("-"))
                    {
                        if (JustPositivDecimalInput)
                        {
                            return false;
                        }

                        if (input.IndexOf("-", StringComparison.Ordinal) > 0)
                        {
                            return false;
                        }

                        if (input.ToCharArray().Count(x => x == '-') > 1)
                        {
                            return false;
                        }

                        //minus einmal am anfang zulässig
                        if (input.Length == 1)
                        {
                            return true;
                        }
                    }

                    return decimal.TryParse(input, _validNumberStyles, CultureInfo.CurrentCulture, out d);

                default:
                    throw new ArgumentException("Unknown TextBoxInputMode");

            }
        }

        /// <summary>
        /// 10進数チェック
        /// </summary>
        /// <param name="wert"></param>
        /// <returns></returns>
        bool CheckIsDigit(string wert)
        {
            return wert.ToCharArray().All(char.IsDigit);
        }

        /// <summary>
        /// 数値文字列を対応する小数点以下の0を追加して返します
        /// </summary>
        /// <param name="value">変換元データ</param>
        /// <param name="decimalCount">小数点以下の個数</param>
        /// <returns>作成した文字列</returns>
        string GetDecimalString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            string result = value;
            string prefix = string.Empty;

            if (0 < DecimalCount)
            {
                if (result.StartsWith("+") || result.StartsWith("-"))
                {
                    // 記号は取得しておく
                    prefix = result.Substring(0, 1);
                    result = result.Substring(1);
                }

                int indexPoint = result.IndexOf(".");
                if (indexPoint < 0)
                {
                    // ドットが無ければ追加する
                    result += ".";
                }

                // 前後の0を消して有効数字のみにする(整数部のみの場合に10→1に変換されるので、.が追加されたこのタイミングで行う)
                result = result.Trim('0');
                indexPoint = result.IndexOf(".");
                if (indexPoint == 0)
                {
                    // 先頭が数字じゃなければ0を追加
                    result = "0" + result;
                }

                for (int i = 0; i < DecimalCount; i++)
                {
                    if (Regex.IsMatch(result, $@"^\d+\.\d{{{DecimalCount}}}"))
                    {
                        // 一致すれば終わり
                        break;
                    }
                    else
                    {
                        // 一致しない場合は0を追加
                        result += "0";
                    }
                }

                // もし0なら記号を外す
                if (double.TryParse(result, out double tmpResult) && tmpResult == 0)
                {
                    prefix = string.Empty;
                }
            }

            return prefix + result;
        }
    }

    /// <summary>
    /// 入力モード
    /// </summary>
    public enum TextBoxInputMode
    {
        /// <summary>
        /// 未指定
        /// </summary>
        None,
        /// <summary>
        /// 小数点許可
        /// </summary>
        DecimalInput,
        /// <summary>
        /// 正数のみ
        /// </summary>
        DigitInput
    }
}
