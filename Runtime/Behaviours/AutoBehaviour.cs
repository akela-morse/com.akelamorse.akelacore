using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
#if UNITY_EDITOR
using System.Linq;
using System.Reflection;
#endif

namespace Akela.Behaviours
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class RequireFromParentAttribute : Attribute
	{
		public readonly Type m_Type0;
		public readonly Type m_Type1;
		public readonly Type m_Type2;

		public RequireFromParentAttribute(Type requiredComponent)
		{
			m_Type0 = requiredComponent;
		}

		public RequireFromParentAttribute(Type requiredComponent, Type requiredComponent2)
		{
			m_Type0 = requiredComponent;
			m_Type1 = requiredComponent2;
		}

		public RequireFromParentAttribute(Type requiredComponent, Type requiredComponent2, Type requiredComponent3)
		{
			m_Type0 = requiredComponent;
			m_Type1 = requiredComponent2;
			m_Type2 = requiredComponent3;
		}
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class RequireFromChildrenAttribute : Attribute
	{
		public readonly Type m_Type0;
		public readonly Type m_Type1;
		public readonly Type m_Type2;

		public RequireFromChildrenAttribute(Type requiredComponent)
		{
			m_Type0 = requiredComponent;
		}

		public RequireFromChildrenAttribute(Type requiredComponent, Type requiredComponent2)
		{
			m_Type0 = requiredComponent;
			m_Type1 = requiredComponent2;
		}

		public RequireFromChildrenAttribute(Type requiredComponent, Type requiredComponent2, Type requiredComponent3)
		{
			m_Type0 = requiredComponent;
			m_Type1 = requiredComponent2;
			m_Type2 = requiredComponent3;
		}
	}

	public abstract class AutoBehaviour : MonoBehaviour, ISerializationCallbackReceiver
	{
		[SerializeField] private Component[] _dependencies;

		private readonly Dictionary<Type, Component> _dependencies_v = new();

		public void OnBeforeSerialize()
		{
			LoadDependencies();
			OnAfterDeserialize();
		}

		public void OnAfterDeserialize()
		{
			_dependencies_v.Clear();

			foreach (var dependency in _dependencies)
				_dependencies_v.TryAdd(dependency.GetType(), dependency);
		}

		private void LoadDependencies()
		{
#if UNITY_EDITOR
			static bool NotNull<T>(T x) => x != null;
			static T This<T>(T x) => x;

			var dependenciesOnThis = GetType().GetCustomAttributes<RequireComponent>()
				.Where(NotNull)
				.Select(x => new Type[] { x.m_Type0, x.m_Type1, x.m_Type2 })
				.SelectMany(This)
				.Where(NotNull)
				.Select(GetComponent)
				.Where(NotNull);

			var dependenciesOnParent = GetType().GetCustomAttributes<RequireFromParentAttribute>()
				.Where(NotNull)
				.Select(x => new Type[] { x.m_Type0, x.m_Type1, x.m_Type2 })
				.SelectMany(This)
				.Where(NotNull)
				.Select(GetComponentInParent)
				.Where(NotNull);

			var dependenciesOnChildren = GetType().GetCustomAttributes<RequireFromChildrenAttribute>()
				.Where(NotNull)
				.Select(x => new Type[] { x.m_Type0, x.m_Type1, x.m_Type2 })
				.SelectMany(This)
				.Where(NotNull)
				.Select(GetComponentInChildren)
				.Where(NotNull);

			_dependencies = dependenciesOnThis.Union(dependenciesOnParent).Union(dependenciesOnChildren).ToArray();
#endif
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected T My<T>() where T : Component => (T)_dependencies_v[typeof(T)];
	}
}