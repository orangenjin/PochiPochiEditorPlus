using System.Collections.Generic;
using System.Dynamic;

namespace PochiPochiEditorPlus._Utilities
{
    /// <summary>
    /// ドット記法によるアクセスを可能にして、キーの二重定義を防ぐ。
    /// </summary>
    public abstract class DynamicAccessor<T> : DynamicObject
    {
        protected Dictionary<string, T> _values = new Dictionary<string, T>();

        /// <summary>
        /// 値を登録する。
        /// 辞書のAddの仕様を利用して、上書きを拒否する。
        /// </summary>
        public virtual void Register(string key, T value)
        {
            _values.Add(key, value);
        }

        /// <summary>
        /// 登録された値を初期化する。
        /// </summary>
        public virtual void ClearDict()
        {
            _values.Clear();
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

        /// <summary>
        /// インデクサによるアクセスを可能にする。
        /// </summary>
        public override bool TryGetIndex(
            GetIndexBinder binder, 
            object[] indexes, 
            out object result)
        {
            if (indexes[0] is string key && _values.TryGetValue(key, out T value))
            {
                result = value;
                return true;
            }

            result = null;
            return false;
        }
    }
}
