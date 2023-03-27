using Prism.Events;

namespace Sample.MainWindow.Events
{
    /// <summary>
    /// メイン画面を閉じて良いかどうかの判断イベント
    /// </summary>
    /// <typeparam name="CancelEventArgs"></typeparam>
    class ClosingWindowEvent<CancelEventArgs> : PubSubEvent<CancelEventArgs>
    {
    }
}
