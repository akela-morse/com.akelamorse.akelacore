using System;

namespace Akela.Behaviours
{
	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
	public sealed class GenerateHashForEveryFieldAttribute : Attribute { }
}
