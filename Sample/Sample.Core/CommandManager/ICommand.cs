namespace Sample.Core.CommandManager
{
    /// <summary>
    /// ICommandインターフェイス
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 履歴に追加可能かどうか
        /// </summary>
        /// <param name="previous">前回登録したデータ</param>
        /// <returns>true:追加可能、false:追加不可能</returns>
        bool CanAdd(ICommand previous);

        /// <summary>
        /// 元に戻す
        /// </summary>
        void Undo();

        /// <summary>
        /// やり直し
        /// </summary>
        void Redo();
    }
}
