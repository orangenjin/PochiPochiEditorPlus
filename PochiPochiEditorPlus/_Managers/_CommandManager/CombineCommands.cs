using System.Collections.Generic;

namespace PochiPochiEditorPlus._Managers._CommandManager
{
    public sealed class CombineCommands : ICommand
    {
        private List<ICommand> _commands = null;

        public string Desc { get; }

        /// <summary>
        /// 複数のコマンドを単一コマンドとして扱うため。
        /// </summary>
        public CombineCommands(string desc)
        {
            _commands = new List<ICommand>();
            Desc = desc;
        }

        /// <summary>
        /// 後入れでコマンド(nullを判定)を追加する。
        /// </summary>
        public void Add(ICommand command)
        {
            if (command != null)
            {
                _commands.Add(command);
            }
        }

        /// <summary>
        /// 要素数を確認できる。
        /// </summary>
        public bool HasCommands => _commands.Count > 0;

        public void Undo()
        {
            // 一応逆順処理
            for (int i = _commands.Count - 1; i >= 0; i--)
            {
                _commands[i].Undo();
            }
        }

        public void Redo()
        {
            foreach (var command in _commands)
            {
                command.Redo();
            }
        }
    }
}
