using Akela.ExtendedPhysics;
using Akela.Optimisations;
using System;
using System.Reflection;
using UnityEditor;

namespace AkelaEditor.Tools
{
	[InitializeOnLoad]
	internal static class DisableUnwantedGizmosForInternalComponents
	{
		private static readonly Type[] _disableIconsForTypes =
		{
			typeof(Raycaster),
			typeof(TorusCollider),
			typeof(CullingSystem),
			typeof(ComponentCull)
		};
		
		private const int MONO_BEHAVIOR_CLASS_ID = 114; // https://docs.unity3d.com/Manual/ClassIDReference.html

		private static readonly MethodInfo setIconEnabled = typeof(Editor).Assembly
			.GetType("UnityEditor.AnnotationUtility")
			.GetMethod("SetIconEnabled", BindingFlags.Static | BindingFlags.NonPublic);

		static DisableUnwantedGizmosForInternalComponents()
		{
			foreach (var type in _disableIconsForTypes)
				setIconEnabled.Invoke(null, new object[] { MONO_BEHAVIOR_CLASS_ID, type.Name, 0 });
		}
	}
}
