using System;

namespace Akela.Tools
{
    [AttributeUsage(AttributeTargets.Delegate, Inherited = false, AllowMultiple = false)]
    public sealed class InternalMethodAttribute : Attribute
    {
        public string MethodName { get; set; }
        public bool IsStatic { get; set; }
        public bool IsPrivate { get; set; }
    }
}