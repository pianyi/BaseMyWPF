using Prism.Events;

namespace Sample.Core.Events
{
    /// <summary>
    /// ダイアログを閉じるイベント
    /// </summary>
    public class CloseDialogEvent : PubSubEvent<string>
    {
    }
}
