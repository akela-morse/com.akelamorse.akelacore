using System;
using System.Collections.Generic;

namespace AkelaEditor.Tools
{
	public static class ReflectionExtensions
	{
		public static IEnumerable<Type> GetBaseTypes(this Type type)
		{
			var t = type;

			while ((t = t.BaseType) != null)
				yield return t;
		}
	}
}
