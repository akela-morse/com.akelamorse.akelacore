using System;

namespace Akela.Behaviours
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class TickOptionsAttribute : Attribute
    {
        private readonly TickBehaviour.TickUpdateType[] _allowedUpdateTypes;

        public TickOptionsAttribute(params TickBehaviour.TickUpdateType[] positionalString)
        {
            _allowedUpdateTypes = positionalString;
        }

        public TickBehaviour.TickUpdateType[] AllowedUpdateTypes => _allowedUpdateTypes;
    }
}
