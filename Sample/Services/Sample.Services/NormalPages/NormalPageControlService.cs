using NLog;
using Prism.Events;
using Reactive.Bindings;
using Unity;

namespace Sample.Services.NormalPages
{
    /// <summary>
    /// メイン画面のレイアウト制御
    /// </summary>
    public class NormalPageControlService
    {
        /// <summary>
        /// ログクラス
        /// </summary>
        readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// イベント登録クラス
        /// </summary>
        [Dependency]
        internal IEventAggregator EventAggregator { private get; set; }

        /// <summary>
        /// テキストボックス
        /// </summary>
        public ReactivePropertySlim<string> TextBox { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NormalPageControlService()
        {
            // サービスはシングルトン作成にしているので、破棄は不要
            TextBox = new ReactivePropertySlim<string>("");
        }

        /// <summary>
        /// 処理実行
        /// </summary>
        public void DoExecute()
        {
            _logger.Info($"ボタンが押下されました。{TextBox.Value}");
        }
    }
}
