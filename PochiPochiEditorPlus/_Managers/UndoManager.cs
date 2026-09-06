using System;
using System.Collections.Generic;

namespace PochiPochiEditorPlus._Managers
{
    public sealed class UndoManager
    {
        private List<ICommand> _history = new List<ICommand>();
        private int _currentIndex = 0;

        // Undo, Redoの発生判定
        public event EventHandler StateChanged = null;

        // 公開用
        public List<ICommand> History => _history;
        public int CurrentIndex => _currentIndex;
        public bool CanUndo => _currentIndex > 0;
        public bool CanRedo => _currentIndex < _history.Count;

        public void PushCommand(ICommand command)
        {
            ExecuteAndNotify(() =>
            {
                // Undo済みの位置から新しい操作を行った場合
                // そこから先のRedo履歴は破棄
                if (_currentIndex < _history.Count)
                {
                    _history.RemoveRange(
                        _currentIndex,
                        _history.Count - _currentIndex);
                }

                _history.Add(command);
                _currentIndex++;
            });
        }

        public void Undo()
        {
            if (!CanUndo) return;

            ExecuteAndNotify(() =>
            {
                _currentIndex--;
                _history[_currentIndex].Undo();
            });
        }

        public void Redo()
        {
            if (!CanRedo) return;

            ExecuteAndNotify(() =>
            {
                _history[_currentIndex].Redo();
                _currentIndex++;
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
            while (_currentIndex > targetIndex)
            {
                Undo();
            }

            while (_currentIndex < targetIndex)
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
                _history.Clear();
                _currentIndex = 0;
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
