using NLog;
using Prism.Events;
using Sample.MainWindow.Events;
using Sample.MainWindow.Events.Startup;
using System;
using System.ComponentModel;
using System.Windows;
using Unity;

namespace Sample.MainWindow.Views
{
    /// <summary>
    /// メイン画面実装クラス
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// イベント制御
        /// </summary>
        [Dependency]
        internal IEventAggregator EventAggregator { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            ContentRendered += MainWindowContentRendered;
            Closing += MainWindowClosing;
            Closed += MainWindowClosed;
        }

        /// <summary>
        /// スタートアップ処理を実行します
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindowContentRendered(object sender, EventArgs e)
        {
            _logger.Debug("初期化イベント開始");

            // 初期化処理開始イベント発行(イベントなので処理が止まる)
            EventAggregator.GetEvent<DoStartup>().Publish();

            // メインウィンドウを表示
            Show();
            Activate();
            Focus();

            ContentRendered -= MainWindowContentRendered;
        }

        /// <summary>
        /// ウィンドウが閉じる前の動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindowClosing(object sender, CancelEventArgs e)
        {
            EventAggregator.GetEvent<ClosingWindowEvent<CancelEventArgs>>().Publish(e);
        }

        /// <summary>
        /// ウィンドウが閉じた時の動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindowClosed(object sender, EventArgs e)
        {
            _logger.Debug("メイン画面を閉じる時のイベントを発行");

            EventAggregator.GetEvent<ClosedWindowEvent>().Publish();

            Closing -= MainWindowClosing;
            Closed -= MainWindowClosed;
        }
    }
}
