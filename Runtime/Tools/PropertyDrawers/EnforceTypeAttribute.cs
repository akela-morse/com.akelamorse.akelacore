using System;
using UnityEngine;

namespace Akela.Tools
{
    public sealed class EnforceTypeAttribute : PropertyAttribute
    {
        public readonly Type type;
        public bool allowSceneObjects = true;

        public EnforceTypeAttribute(Type type)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type));
        }

    }
}