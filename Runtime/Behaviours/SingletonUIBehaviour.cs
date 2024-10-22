using System.Reflection;
using UnityEngine;

namespace Akela.Behaviours
{
	public abstract class SingletonUIBehaviour<T> : MonoBehaviour where T : SingletonUIBehaviour<T>
	{
		private static readonly MethodInfo CurrentThreadIsMainThread = typeof(Object).GetMethod("CurrentThreadIsMainThread", BindingFlags.NonPublic | BindingFlags.Static);

		public static T Main { get; private set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Field hiding")]
		public new RectTransform transform => (RectTransform)base.transform;

		public SingletonUIBehaviour() : base()
		{
			if (!(bool)CurrentThreadIsMainThread.Invoke(null, null))
				return;

			Main = (T)this;
		}
	}
}