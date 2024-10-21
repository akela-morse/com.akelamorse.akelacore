using Akela.Optimisations;
using System.Linq;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AkelaEditor.Optimisations
{
	[EditorTool("Culling System Tool", typeof(CullingSystem))]
	internal class CullingSystemTool : EditorTool, IDrawSelectedHandles
	{
		private readonly BoxBoundsHandle _boxHandle = new();
		private GUIContent _icon;

		public override GUIContent toolbarIcon => _icon;

		private void OnEnable()
		{
			_icon = EditorGUIUtility.IconContent("EditCollider");
		}

		private void OnDisable()
		{
			_icon = null;
		}

		public override void OnToolGUI(EditorWindow window)
		{
			foreach (var target in targets.Cast<CullingSystem>())
			{
				var serializedObject = new SerializedObject(target);

				var boundsProperty = serializedObject.FindProperty("_bounds");

				using (new Handles.DrawingScope(Color.white, target.transform.localToWorldMatrix))
				{
					_boxHandle.center = boundsProperty.boundsValue.center;
					_boxHandle.size = boundsProperty.boundsValue.size;

					EditorGUI.BeginChangeCheck();

					_boxHandle.DrawHandle();

					if (EditorGUI.EndChangeCheck())
					{
						serializedObject.Update();
						boundsProperty.boundsValue = new(_boxHandle.center, _boxHandle.size);
						serializedObject.ApplyModifiedProperties();
					}
				}
			}
		}

		public void OnDrawHandles()
		{
			foreach (var target in targets.Cast<CullingSystem>())
			{
				var serializedObject = new SerializedObject(target);

				var bounds = serializedObject.FindProperty("_bounds").boundsValue;
				var subdivisions = serializedObject.FindProperty("_subdivisions").intValue;

				var sphereRadius = Mathf.Max(bounds.extents.x, bounds.extents.y, bounds.extents.z) / subdivisions;
				var sphereOffset = Vector3.one * sphereRadius;

				using (new Handles.DrawingScope(target.transform.localToWorldMatrix))
				{
					Handles.color = Color.white;
					Handles.DrawWireCube(bounds.center, bounds.size);

					Handles.color = new(.7f, .4f, 1f);

					for (var z = 0; z < subdivisions; ++z)
					{
						for (var y = 0; y < subdivisions; ++y)
						{
							for (var x = 0; x < subdivisions; ++x)
							{
								var sphereCenter = sphereOffset + new Vector3(
									Mathf.Lerp(bounds.min.x, bounds.max.x, x / (float)subdivisions),
									Mathf.Lerp(bounds.min.y, bounds.max.y, y / (float)subdivisions),
									Mathf.Lerp(bounds.min.z, bounds.max.z, z / (float)subdivisions)
								);

								Handles.DrawWireDisc(sphereCenter, Vector3.right, sphereRadius);
								Handles.DrawWireDisc(sphereCenter, Vector3.up, sphereRadius);
								Handles.DrawWireDisc(sphereCenter, Vector3.forward, sphereRadius);
							}
						}
					}
				}
			}
		}
	}
}
