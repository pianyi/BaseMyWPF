using NLog;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Sample.Core.Mvvm;
using Sample.Services.NormalPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Linq;
using System.Windows;
using Unity;

namespace Sample.NormalPage.ViewModels
{
    /// <summary>
    /// 画面表示内容のビューモデル
    /// </summary>
    class NormalPageMainViewModel : RegionViewModelBase
    {
        /// <summary>
        /// ログクラス
        /// </summary>
        readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 専用サービスクラス
        /// </summary>
        [Dependency]
        internal NormalPageControlService NormalPageControlService;

        /// <summary>
        /// Escキー押下コマンド
        /// </summary>
        public ReactiveCommand KeyDownEscCommand { get; private set; }

        /// <summary>
        /// テキストボックス
        /// </summary>
        [Required]
        public ReactiveProperty<string> TextBox { get; set; }

        /// <summary>
        /// テキストボックスでEnterキーを押下した時の処理
        /// </summary>
        public ReactiveCommand KeyDownEnter { get; private set; }

        /// <summary>
        /// ボタン押下処理
        /// </summary>
        public ReactiveCommand ExecuteButtonClick { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="regionManager"></param>
        /// <param name="eventAggregator">イベント登録用</param>
        public NormalPageMainViewModel(IRegionManager regionManager,
                                       NormalPageControlService normalPageControlService)
            : base(regionManager)
        {
            // DI
            NormalPageControlService = normalPageControlService;

            // テキストボックスの内容連携
            TextBox = NormalPageControlService.TextBox
                                              .ToReactivePropertyAsSynchronized(v => v.Value)
                                              .SetValidateNotifyError(e => GetFileNameValidationError(e))
                                              .AddTo(Disposables);

            // Escキー押下イベントをセット
            KeyDownEscCommand = new ReactiveCommand().WithSubscribe(CanCloseMessage)
                                                     .AddTo(Disposables);

            // Enterキー押下時のイベント
            KeyDownEnter = TextBox.ObserveHasErrors
                                  .Select(v => !v)
                                  .ToReactiveCommand()
                                  .WithSubscribe(ClickButton)
                                  .AddTo(Disposables);

            // ボタン押下処理
            ExecuteButtonClick = TextBox.ObserveHasErrors
                                        .Select(v => !v)
                                        .ToReactiveCommand()
                                        .WithSubscribe(ClickButton)
                                        .AddTo(Disposables);
        }

        /// <summary>
        /// 入力チェック
        /// </summary>
        private string GetFileNameValidationError(string value)
        {
            string result = null;
            int maxLength = 10;

            if (maxLength < value.Length)
            {
                // 文字数チェック
                result = $"{maxLength}文字以下で入力してください。";
            }

            return result;
        }

        /// <summary>
        /// 画面を閉じて良いかどうかのメッセージを表示します
        /// </summary>
        private void CanCloseMessage()
        {
            Application.Current.MainWindow.Close();
        }

        /// <summary>
        /// ボタン押下処理
        /// </summary>
        private void ClickButton()
        {
            try
            {
                NormalPageControlService.DoExecute();
            }
            catch (Exception ex)
            {
                // Subscribe 内では絶対に例外を throw してはいけない。
                // イベントがクリアされてしまい、以降のSubscribeは実行されない。
                _logger.Error(ex, "エラーが発生");
            }
        }
    }
}
