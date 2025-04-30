using UnityEngine;

namespace Akela.Tools
{
    [InternalWrapper(typeof(Object), "UnityEngine.Object")]
    public static partial class InternalTools
    {
        [InternalMethod(IsStatic = true, IsPrivate = true)]
        private delegate bool CurrentThreadIsMainThreadDelegate();
    }
}