#pragma warning disable UNT0014 // Invalid type for call to GetComponent, Justification = Need support for interfaces
using UnityEngine;

namespace Akela.Tools
{
	public static class ComponentExtensions
	{
		public static bool TryGetComponentInChildren<T>(this GameObject thisGameObject, out T component) where T : class
		{
			component = thisGameObject.GetComponentInChildren<T>();

			return component != null;
		}

		public static bool TryGetComponentInParent<T>(this GameObject thisGameObject, out T component) where T : class
		{
			component = thisGameObject.GetComponentInParent<T>();

			return component != null;
		}

		public static bool TryGetComponentInChildren<T>(this Component thisComponent, out T component) where T: class
		{
			component = thisComponent.GetComponentInChildren<T>();

			return component != null;
		}

		public static bool TryGetComponentInParent<T>(this Component thisComponent, out T component) where T : class
		{
			component = thisComponent.GetComponentInParent<T>();

			return component != null;
		}
	}
}
