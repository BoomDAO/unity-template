using System;
using System.Collections.Generic;
using System.Reflection;

namespace QFSW.QC.Internal
{
    internal class DummyParameter : ParameterInfo
    {
        private readonly ParameterInfo _internalDummy;

        public DummyParameter(Type type, string name, int position)
        {
            ParameterType = type;
            Name = name;
            Position = position;

            MethodInfo dummyMethod = typeof(DummyParameter).GetMethod(nameof(DummyMethod), BindingFlags.NonPublic | BindingFlags.Static);
            _internalDummy = dummyMethod.GetParameters()[0];
        }

        private static void DummyMethod(int dummyParameter) { }

        public override Type ParameterType { get; }
        public override string Name { get; }
        public override int Position { get; }

        public override ParameterAttributes Attributes => _internalDummy.Attributes;
        public override object DefaultValue => null;
        public override bool Equals(object obj) { return ReferenceEquals(this, obj); }
        public override IEnumerable<CustomAttributeData> CustomAttributes => _internalDummy.CustomAttributes;
        public override object[] GetCustomAttributes(bool inherit) => _internalDummy.GetCustomAttributes(inherit);
        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => _internalDummy.GetCustomAttributes(attributeType, inherit);
        public override IList<CustomAttributeData> GetCustomAttributesData() => _internalDummy.GetCustomAttributesData();
        public override int GetHashCode() { return 0; }
        public override Type[] GetOptionalCustomModifiers() { return _internalDummy.GetOptionalCustomModifiers(); }
        public override Type[] GetRequiredCustomModifiers() { return _internalDummy.GetRequiredCustomModifiers(); }
        public override bool HasDefaultValue => false;
        public override bool IsDefined(Type attributeType, bool inherit) { return false; }
        public override MemberInfo Member => _internalDummy.Member;
        public override int MetadataToken => _internalDummy.MetadataToken;
        public override object RawDefaultValue => null;
        public override string ToString() { return Name; }
    }
}