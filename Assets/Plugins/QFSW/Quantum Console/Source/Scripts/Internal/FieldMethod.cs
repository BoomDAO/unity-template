using System;
using System.Globalization;
using System.Reflection;

namespace QFSW.QC.Internal
{
    internal abstract class FieldMethod : MethodInfo
    {
        protected readonly FieldInfo _fieldInfo;
        protected Delegate _internalDelegate;
        protected ParameterInfo[] _parameters;

        protected FieldMethod(FieldInfo fieldInfo) { _fieldInfo = fieldInfo; }

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            object[] realParameters = new object[parameters.Length + 1];
            realParameters[0] = IsStatic ? _fieldInfo : obj;

            Array.Copy(parameters, 0, realParameters, 1, parameters.Length);
            return _internalDelegate.DynamicInvoke(realParameters);
        }

        public override ParameterInfo[] GetParameters()
        {
            return _parameters;
        }

        public override string Name => _fieldInfo.Name;
        public override Type DeclaringType => _fieldInfo.DeclaringType;
        public override Type ReflectedType => _fieldInfo.ReflectedType;
        public override object[] GetCustomAttributes(bool inherit) { return _fieldInfo.GetCustomAttributes(inherit); }
        public override object[] GetCustomAttributes(Type attributeType, bool inherit) { return _fieldInfo.GetCustomAttributes(attributeType, inherit); }

        public override MethodAttributes Attributes => _internalDelegate.Method.Attributes;
        public override RuntimeMethodHandle MethodHandle => _internalDelegate.Method.MethodHandle;
        public override ICustomAttributeProvider ReturnTypeCustomAttributes => _internalDelegate.Method.ReturnTypeCustomAttributes;
        public override MethodInfo GetBaseDefinition() { return _internalDelegate.Method.GetBaseDefinition(); }
        public override MethodImplAttributes GetMethodImplementationFlags() { return _internalDelegate.Method.GetMethodImplementationFlags(); }
        public override bool IsDefined(Type attributeType, bool inherit) { return _internalDelegate.Method.IsDefined(attributeType, inherit); }
    }
}
