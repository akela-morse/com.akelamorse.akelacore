using System;

namespace Akela.Tools
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class InternalWrapperAttribute : Attribute
    {
        private readonly Type _neighbouringType;
        private readonly string _typeName;

        public InternalWrapperAttribute(Type neighbouringType, string typeName)
        {
            _neighbouringType = neighbouringType;
            _typeName = typeName;
        }

        public Type Assembly => _neighbouringType;
        public string TypeName => _typeName;
    }
}