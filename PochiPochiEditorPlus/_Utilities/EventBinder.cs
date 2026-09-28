using System;
using System.Collections.Generic;

namespace PochiPochiEditorPlus._Utilities
{
    public sealed class EventBinder
    {
        // イベントを解除するため
        private List<Action> _detachActions = new List<Action>();

        /// <summary>
        /// 通常のイベントの追加と解除。
        /// </summary>
        public void BindCtrl(
            Action<EventHandler> adder,
            Action<EventHandler> remover,
            EventHandler handler = null)
        {
            // nullの場合、解除用のイベントを登録する
            if (handler == null)
            {
                handler = (_, __) => Dispose();
            }

            adder(handler);
            _detachActions.Add(() => remover(handler));
        }

        /// <summary>
        /// 自作のイベントの追加と解除。
        /// </summary>
        public void BindCustom(Action attachAction, Action detachAction)
        {
            attachAction();
            _detachActions.Add(detachAction);
        }

        /// <summary>
        /// 破棄のタイミングを指定する用。
        /// </summary>
        private void Dispose()
        {
            foreach (var detach in _detachActions)
            {
                detach();
            }

            _detachActions.Clear();
        }
    }
}
