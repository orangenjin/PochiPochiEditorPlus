using System.Collections.Generic;
using System.Dynamic;

namespace PochiPochiEditorPlus._Utilities
{
    /// <summary>
    /// ドット記法によるアクセスを可能にする。
    /// </summary>
    public sealed class DynamicAccessor<T> : DynamicObject
    {
        private Dictionary<string, T> _values = new Dictionary<string, T>();

        /// <summary>
        /// 値を登録する。
        /// 辞書のAddの仕様を利用して、上書きを拒否する。
        /// </summary>
        public void Register(string key, T value)
        {
            _values.Add(key, value);
        }

        /// <summary>
        /// ドット記法で値へアクセスする。
        /// </summary>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_values.TryGetValue(binder.Name, out T value))
            {
                result = value;
                return true;
            }

            result = null;
            return false;
        }
    }
}
