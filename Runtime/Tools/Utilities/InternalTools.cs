using System;
using System.Reflection;
using Object = UnityEngine.Object;

namespace Akela.Tools
{
	public static class InternalTools
	{
		private const string POSTFIX = "Delegate";

		private delegate bool CurrentThreadIsMainThreadDelegate();

		private static readonly CurrentThreadIsMainThreadDelegate currentThreadIsMainThread;

		static InternalTools()
		{
			currentThreadIsMainThread = BindDelegate<CurrentThreadIsMainThreadDelegate>(typeof(Object), BindingFlags.NonPublic | BindingFlags.Static);
		}

		private static T BindDelegate<T>(Type type, BindingFlags bindingFlags) where T : Delegate
		{
			var name = typeof(T).Name;
			name = name[..^POSTFIX.Length];

			return type.GetMethod(name, bindingFlags)?.CreateDelegate(typeof(T)) as T ?? throw new MissingMethodException($"Failed find method '{name}' in type '{type}'");
		}

		public static bool CurrentThreadIsMainThread() => currentThreadIsMainThread();
	}
}
