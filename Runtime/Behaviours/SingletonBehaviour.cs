using System.Reflection;
using UnityEngine;

namespace Akela.Behaviours
{
	public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
	{
		private static readonly MethodInfo CurrentThreadIsMainThread = typeof(Object).GetMethod("CurrentThreadIsMainThread", BindingFlags.NonPublic | BindingFlags.Static);

		public static T Main { get; private set; }

		public SingletonBehaviour() : base()
		{
			if (!(bool)CurrentThreadIsMainThread.Invoke(null, null))
				return;

			Main = (T)this;
		}
	}
}