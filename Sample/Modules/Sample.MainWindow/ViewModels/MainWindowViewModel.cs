using NLog;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Sample.Core.Mvvm;
using Sample.MainWindow.Events;
using Sample.MainWindow.Events.Startup;
using Sample.Services.MainWindows;
using System;
using System.ComponentModel;
using System.Windows;
using Unity;

namespace Sample.MainWindow.ViewModels
{
    /// <summary>
    /// メインウィンドウのビューモデル
    /// </summary>
    class MainWindowViewModel : DisposableBindableViewModelBase, IDisposable
    {
        /// <summary>
        /// ログクラス
        /// </summary>
        readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// メインウィンドウ操作クラス
        /// </summary>
        [Dependency]
        internal MainWindowControlService MainCtrlService { private get; set; }

        /// <summary>
        /// イベント登録クラス
        /// </summary>
        [Dependency]
        internal IEventAggregator EventAggregator { get; set; }

        /// <summary>
        /// DI用マネージャ
        /// </summary>
        [Dependency]
        internal IRegionManager RegionManager = null;

        /// <summary>
        /// ダイアログ制御クラス
        /// </summary>
        [Dependency]
        internal IDialogService DialogService { get; set; }

        /// <summary>
        /// タイトル
        /// </summary>
        public ReactivePropertySlim<string> Title { get; set; }

        /// <summary>
        /// メインウィンドウ操作可否
        /// </summary>
        public ReactivePropertySlim<bool> IsEnableMainWindow { set; get; }

        /// <summary>
        /// メインウィンドウの幅
        /// </summary>
        public ReactivePropertySlim<double> MainWindowWidth { get; set; }

        /// <summary>
        /// メインウィンドウの最小幅
        /// </summary>
        public ReactivePropertySlim<double> MainWindowMinWidth { get; set; }

        /// <summary>
        /// メインウィンドウの高さ
        /// </summary>
        public ReactivePropertySlim<double> MainWindowHeight { get; set; }

        /// <summary>
        /// メインウィンドウの最小高
        /// </summary>
        public ReactivePropertySlim<double> MainWindowMinHeight { get; set; }

        /// <summary>
        /// メインウィンドウのステータス
        /// </summary>
        public ReactivePropertySlim<WindowState> MainWindowState { get; set; }

        /// <summary>
        /// Escキー押下コマンド
        /// </summary>
        public ReactiveCommand KeyDownEscCommand { get; private set; }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="eventAggregator">イベント制御クラス</param>
        /// <param name="dialogService">ダイアログサービス</param>
        /// <param name="mainWindowService">メインウィンドウのコントロールサービス</param>
        public MainWindowViewModel(IEventAggregator eventAggregator,
                                   IDialogService dialogService,
                                   MainWindowControlService mainWindowService)
        {
            // DI系クラスを内部保持
            EventAggregator = eventAggregator;
            DialogService = dialogService;
            MainCtrlService = mainWindowService;

            // 初期化
            Title = new ReactivePropertySlim<string>($"{Application.Current.Resources["COM-00-000"]}      {Application.Current.Resources["COM-00-S999-00"]}").AddTo(Disposables);

            // ウィンドウの状態
            MainWindowWidth = MainCtrlService.Width.ToReactivePropertySlimAsSynchronized(e => e.Value);
            MainWindowMinWidth = MainCtrlService.MinWidth.ToReactivePropertySlimAsSynchronized(e => e.Value);
            MainWindowHeight = MainCtrlService.Height.ToReactivePropertySlimAsSynchronized(e => e.Value);
            MainWindowMinHeight = MainCtrlService.MinHeight.ToReactivePropertySlimAsSynchronized(e => e.Value);
            MainWindowState = MainCtrlService.State.ToReactivePropertySlimAsSynchronized(e => e.Value);

            // 画面のEnable処理
            IsEnableMainWindow = MainCtrlService.IsEnable.ToReactivePropertySlimAsSynchronized(e => e.Value);

            // Escキー押下イベントをセット
            KeyDownEscCommand = new ReactiveCommand().WithSubscribe(CanCloseMessage)
                                                     .AddTo(Disposables);

            // ウィンドウが閉じる前の動作
            eventAggregator.GetEvent<ClosingWindowEvent<CancelEventArgs>>()
                           .Subscribe(MainWindowClosing)
                           .AddTo(Disposables);

            // ウィンドウが閉じた後の動作
            eventAggregator.GetEvent<ClosedWindowEvent>()
                           .Subscribe(MainWindowClosed)
                           .AddTo(Disposables);

            // 初期化処理イベントを受け取る
            eventAggregator.GetEvent<DoStartup>()
                           .Subscribe(DoStartup)
                           .AddTo(Disposables);
        }

        /// <summary>
        /// 初期化処理を行います
        /// </summary>
        private void DoStartup()
        {

        }

        /// <summary>
        /// 画面を閉じて良いかどうかのメッセージを表示します
        /// </summary>
        private void CanCloseMessage()
        {
            Application.Current.MainWindow.Close();
        }

        /// <summary>
        /// 画面を閉じる前に動作します
        /// </summary>
        /// <param name="e">メインウィンドウを閉じるかどうかのイベント</param>
        private void MainWindowClosing(CancelEventArgs e)
        {
        }

        /// <summary>
        /// ウィンドウが閉じる時の処理を行います
        /// </summary>
        private void MainWindowClosed()
        {
            Destroy();
        }

        #region IDisposable Support
        /// <summary>
        /// オブジェクトの破棄処理
        /// </summary>
        /// <param name="disposing">処理フラグ</param>
        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Disposables.Dispose();

                    foreach (var region in RegionManager.Regions)
                    {
                        region.RemoveAll();
                    }
                }

                disposedValue = true;
            }
        }
        #endregion
    }
}
