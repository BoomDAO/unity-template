using System;
using System.Reflection;

namespace QFSW.QC.Internal
{
    internal class FieldAutoMethod : FieldMethod
    {
        public enum AccessType
        {
            Read = 0,
            Write = 1
        }

        private readonly AccessType _accessType;

        public FieldAutoMethod(FieldInfo fieldInfo, AccessType accessType) : base(fieldInfo)
        {
            _accessType = accessType;
            if (_accessType == AccessType.Read)
            {
                _internalDelegate = _fieldInfo.IsStatic
                    ? (Func<FieldInfo, object>)_StaticReader
                    : (Func<object, object>)_fieldInfo.GetValue;

                _parameters = Array.Empty<ParameterInfo>();
            }
            else
            {
                _internalDelegate = _fieldInfo.IsStatic
                    ? (Action<FieldInfo, object>)_StaticWriter
                    : (Action<object, object>)_fieldInfo.SetValue;

                _parameters = new ParameterInfo[] { new CustomParameter(_internalDelegate.Method.GetParameters()[1], _fieldInfo.FieldType, "value") };
            }
        }

        private static object _StaticReader(FieldInfo field) { return field.GetValue(null); }
        private static void _StaticWriter(FieldInfo field, object value) { field.SetValue(null, value); }
    }
}