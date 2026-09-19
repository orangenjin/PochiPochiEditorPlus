using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PochiPochiEditorPlus._Utilities._FormGroupManager
{
    public class FormGroupData : DynamicAccessor<FieldBinding>
    {
        public void Register<TValue>(object targetInstance, Expression<Func<TValue>> fieldExpression)
        {
            if (fieldExpression.Body is MemberExpression member && member.Member is FieldInfo fieldInfo)
            {
                var binding = new FieldBinding(targetInstance, fieldInfo);
                _values.Add(fieldInfo.Name, binding);
            }
        }
    }

    /// <summary>
    /// 格納するフィールドを整理するクラスを定義する。
    /// </summary>
    public class FieldBinding
    {
        public object Instance { get; set; }
        public FieldInfo Field { get; set; }

        public FieldBinding(object targetInstance, FieldInfo field)
        {
            Instance = targetInstance;
            Field = field;
        }

        public object GetValue() => Field.GetValue(Instance);
        public void SetValue(object value) => Field.SetValue(Instance, value);
    }
}
