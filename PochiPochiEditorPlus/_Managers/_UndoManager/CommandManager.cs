using System;
using System.Collections.Generic;

namespace PochiPochiEditorPlus._Managers
{
    public sealed class CommandManager
    {
        public EventHandler StateChanged { get; set; } // UndoとRedoの発生判定
        public List<ICommand> History { get; }
        public int CurrentIndex { get; private set; }

        public CommandManager()
        {
            History = new List<ICommand>();
        }

        // 状態確認用
        public bool CanUndo => CurrentIndex > 0;
        public bool CanRedo => CurrentIndex < History.Count;

        public void PushCommand(ICommand command)
        {
            ExecuteAndNotify(() =>
            {
                // Undo済みの位置から新しい操作を行った場合
                // そこから先のRedo履歴は破棄
                if (CurrentIndex < History.Count)
                {
                    History.RemoveRange(
                        CurrentIndex,
                        History.Count - CurrentIndex);
                }

                History.Add(command);
                CurrentIndex++;
            });
        }

        public void Undo()
        {
            if (!CanUndo) return;

            ExecuteAndNotify(() =>
            {
                CurrentIndex--;
                History[CurrentIndex].Undo();
            });
        }

        public void Redo()
        {
            if (!CanRedo) return;

            ExecuteAndNotify(() =>
            {
                History[CurrentIndex].Redo();
                CurrentIndex++;
            });
        }

        /// <summary>
        /// 共通処理で、発生したことを知らせる。
        /// </summary>
        private void ExecuteAndNotify(Action action)
        {
            action();
            StateChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 指定した履歴位置まで移動する。
        /// </summary>
        public void MoveTo(int targetIndex)
        {
            while (CurrentIndex > targetIndex)
            {
                Undo();
            }

            while (CurrentIndex < targetIndex)
            {
                Redo();
            }
        }

        /// <summary>
        /// 初期状態に戻す。
        /// </summary>
        public void Clear()
        {
            ExecuteAndNotify(() =>
            {
                History.Clear();
                CurrentIndex = 0;
            });
        }
    }

    /// <summary>
    /// Undo, Redoの操作を規定する。
    /// </summary>
    public interface ICommand
    {
        string Desc { get; }

        void Undo();
        void Redo();
    }
}
