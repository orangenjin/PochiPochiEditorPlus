using System;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;
using PochiPochiEditorPlus._Utilities;

namespace PochiPochiEditorPlus._Managers._FormGroupManager
{
    public sealed class FormGroupData : DynamicAccessor<FieldBinding>
    {
        public EventHandler<Form> RefreshRequested { get; set; }

        public void Register<TValue>(object targetInstance, Expression<Func<TValue>> fieldExpression)
        {
            if (fieldExpression.Body is MemberExpression member && member.Member is FieldInfo fieldInfo)
            {
                var binding = new FieldBinding(targetInstance, fieldInfo);
                _values.Add(fieldInfo.Name, binding);
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_values.TryGetValue(binder.Name, out FieldBinding binding)) 
            {
                result = binding.GetValue();
                return true;
            }

            return base.TryGetMember(binder, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_values.TryGetValue(binder.Name, out FieldBinding binding))
            {
                binding.SetValue(value);
                return true;
            }

            return base.TrySetMember(binder, value);
        }

        /// <summary>
        /// あるフォームから他のフォームへの再描画を行う。
        /// </summary>
        public void RequestRefresh(Form senderForm)
        {
            RefreshRequested?.Invoke(this, senderForm);
        }
    }

    /// <summary>
    /// 格納するフィールドを整理するクラスを定義する。
    /// </summary>
    public sealed class FieldBinding
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
