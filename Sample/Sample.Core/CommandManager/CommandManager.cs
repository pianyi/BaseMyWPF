using NLog;
using System.Collections.Generic;

namespace Sample.Core.CommandManager
{
    /// <summary>
    /// 実行、元に戻す、やり直すの各動作を管理するクラス
    /// </summary>
    public class CommandManager
    {
        /// <summary>
        /// ログクラス
        /// </summary>
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 元に戻すための履歴
        /// </summary>
        private readonly Stack<ICommand> _undo = new();

        /// <summary>
        /// やり直すための履歴
        /// </summary>
        private readonly Stack<ICommand> _redo = new();

        /// <summary>
        /// 操作を実行して内容を履歴に追加する
        /// </summary>
        /// <param name="command">コマンド</param>
        public bool Add(ICommand command)
        {
            bool result = false;

            // 最初の1回目はデータが無いので、例外を出さないためTryを使う
            _undo.TryPeek(out ICommand peek);

            if (command.CanAdd(peek))
            {
                _undo.Push(command);
                _redo.Clear();

                result = true;

                _logger.Debug("Undo履歴を追加しました");
            }

            return result;
        }

        /// <summary>
        /// 操作を元に戻す
        /// </summary>
        public bool Undo()
        {
            bool result = false;

            if (CanUndo())
            {
                ICommand command = _undo.Pop();
                command.Undo();
                _redo.Push(command);

                result = true;

                _logger.Debug("Undoを実行しました");
            }

            return result;
        }

        /// <summary>
        /// 操作をやり直す
        /// </summary>
        public bool Redo()
        {
            bool result = false;

            if (CanRedo())
            {
                ICommand command = _redo.Pop();
                command.Redo();
                _undo.Push(command);

                result = true;

                _logger.Debug("Redoを実行しました");
            }

            return result;
        }

        /// <summary>
        /// 元に戻せるかどうかを確認する
        /// </summary>
        /// <returns>true:Undo可能、false:Undo不可能</returns>
        public bool CanUndo()
        {
            return 0 < _undo.Count;
        }

        /// <summary>
        /// やり直せるかどうかを確認する
        /// </summary>
        /// <returns>true:Redo可能、false:Redo不可能</returns>
        public bool CanRedo()
        {
            return 0 < _redo.Count;
        }

        /// <summary>
        /// 全マーカーを削除します
        /// </summary>
        public void Clear()
        {
            _undo.Clear();
            _redo.Clear();
        }
    }
}
