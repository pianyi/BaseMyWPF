namespace Sample.Core.CustomControls.Interfaces
{
    /// <summary>
    /// 画面に表示するデータかどうかを判断します
    /// </summary>
    public interface ICanDisplayItem
    {
        /// <summary>
        /// 表示可能かどうかを判断します
        /// </summary>
        bool CanDisplay { get; }
    }
}
