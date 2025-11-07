using System;
using UnityEngine;

namespace Akela.Behaviours
{
    /// <summary>
    /// Indicates that the component this field references is present on the same GameObject.
    /// The dependency will be automatically injected and serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class FromThisAttribute : PropertyAttribute
    {
        public FromThisAttribute() : base(true) { }
    }

    /// <summary>
    /// Indicates that the component this field references is present on one of this GameObject's parents.
    /// The dependency will be automatically injected and serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class FromParentsAttribute : PropertyAttribute
    {
        public FromParentsAttribute() : base(true) { }
    }

    /// <summary>
    /// Indicates that the component this field references is present on one of this GameObject's children.
    /// The dependency will be automatically injected and serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class FromChildrenAttribute : PropertyAttribute
    {
        public FromChildrenAttribute() : base(true) { }
    }
}