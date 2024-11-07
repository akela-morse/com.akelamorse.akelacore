using System;

namespace Akela.Behaviours
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class FromParentsAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class FromChildrenAttribute : Attribute { }
}
