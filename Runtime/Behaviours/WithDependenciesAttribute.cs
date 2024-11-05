using System;

namespace Akela.Behaviours
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class WithDependenciesAttribute : Attribute
    {
        private readonly Type _dependenciesContainer;

        public WithDependenciesAttribute(Type dependenciesContainer)
        {
            _dependenciesContainer = dependenciesContainer;
        }

        public Type DependenciesContainer => _dependenciesContainer;
    }
}
