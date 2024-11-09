using Akela.ExtendedPhysics;
using Akela.Tools;
using System.Linq;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Rendering;

namespace AkelaEditor.ExtendedPhysics
{
	[EditorTool("Edit bounding volume", typeof(TorusCollider))]
	internal class TorusColliderEditor : EditorTool, IDrawSelectedHandles
	{
		private readonly SphereBoundsHandle _innerRadiusHandle = new();
		private readonly SphereBoundsHandle _outerRadiusHandle = new();
		private readonly SphereBoundsHandle[] _heightHandles = new SphereBoundsHandle[] { new(), new(), new(), new() };

		private GUIContent _toolIcon;
		private bool _toolActive;

		public override GUIContent toolbarIcon => _toolIcon;

		private void OnEnable()
		{
			_toolIcon = EditorGUIUtility.IconContent("EditCollider");
			_toolIcon.tooltip = "Edit bounding volume.";
		}

		public override void OnActivated()
		{
			_toolActive = true;
		}

		public override void OnWillBeDeactivated()
		{
			_toolActive = false;
		}

		public override void OnToolGUI(EditorWindow window)
		{
			if (window is not SceneView)
				return;

			foreach (TorusCollider target in targets.Cast<TorusCollider>())
			{
				var mainAxis = target.direction.ToVector3();
				var rightAxis = target.direction.RightRelative();
				var upAxis = target.direction.UpRelative();
				var heightAxes = new[] { upAxis, rightAxis, -upAxis, -rightAxis };
				var heightHandleAxes = (PrimitiveBoundsHandle.Axes)0.SetBit((int)target.direction);
				var handleAxes = target.direction switch
				{
					Axis.X => PrimitiveBoundsHandle.Axes.Y | PrimitiveBoundsHandle.Axes.Z,
					Axis.Y => PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Z,
					Axis.Z => PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Y,
					_ => PrimitiveBoundsHandle.Axes.None
				};

				using (new Handles.DrawingScope(CustomColliderEditor.IDLE_COLOR, target.transform.localToWorldMatrix))
				{
					_innerRadiusHandle.center = target.center;
					_innerRadiusHandle.radius = target.radius - target.thickness;
					_innerRadiusHandle.axes = handleAxes;
					_innerRadiusHandle.wireframeColor = CustomColliderEditor.TRANSPARENT;

					_outerRadiusHandle.center = target.center;
					_outerRadiusHandle.radius = target.radius + target.thickness;
					_outerRadiusHandle.axes = handleAxes;
					_outerRadiusHandle.wireframeColor = CustomColliderEditor.TRANSPARENT;

					for (var i = 0; i < 4; ++i)
					{
						_heightHandles[i].center = target.center + heightAxes[i] * target.radius;
						_heightHandles[i].radius = target.thickness;
						_heightHandles[i].axes = heightHandleAxes;
						_heightHandles[i].wireframeColor = CustomColliderEditor.TRANSPARENT;
					}

					EditorGUI.BeginChangeCheck();

					_innerRadiusHandle.DrawHandle();
					_outerRadiusHandle.DrawHandle();

					for (var i = 0; i < 4; ++i)
						_heightHandles[i].DrawHandle();

					if (EditorGUI.EndChangeCheck())
					{
						Undo.RecordObject(target, "Change TorusCollider bounding shape");

						// Inner radius
						var innerRadiusDelta = (target.radius - target.thickness - _innerRadiusHandle.radius) / 2f;

						if (innerRadiusDelta != 0f && (target.thickness > 0f || innerRadiusDelta > 0f))
						{
							target.radius -= innerRadiusDelta;
							target.thickness += innerRadiusDelta;

							target.OnValidate();
							return;
						}

						// Outer radius
						var outerRadiusDelta = target.radius + target.thickness - _outerRadiusHandle.radius;

						if (outerRadiusDelta != 0f)
						{
							target.radius -= outerRadiusDelta;
							target.center = _outerRadiusHandle.center;

							target.OnValidate();
							return;
						}

						// Height
						for (var i = 0; i < 4; ++i)
						{
							var heightDelta = target.thickness - _heightHandles[i].radius;

							if (heightDelta == 0f || (target.thickness <= 0f && heightDelta > 0f))
								continue;

							target.thickness -= heightDelta;
							target.center = _heightHandles[i].center - heightAxes[i] * target.radius;

							target.OnValidate();
							return;
						}
					}
				}
			}
		}

		public void OnDrawHandles()
		{
			var prevZTest = Handles.zTest;

			Handles.zTest = _toolActive ? CompareFunction.Always : CompareFunction.LessEqual;

			foreach (TorusCollider target in targets.Cast<TorusCollider>())
			{
				var mainAxis = target.direction.ToVector3();
				var rightAxis = target.direction.RightRelative();
				var upAxis = target.direction.UpRelative();

				using (new Handles.DrawingScope(target.isActiveAndEnabled ? CustomColliderEditor.IDLE_COLOR : CustomColliderEditor.DISABLED_COLOR, target.transform.localToWorldMatrix))
				{
					Handles.DrawWireDisc(target.center, mainAxis, target.radius - target.thickness);
					Handles.DrawWireDisc(target.center, mainAxis, target.radius + target.thickness);
					Handles.DrawWireDisc(target.center + mainAxis * target.thickness, mainAxis, target.radius);
					Handles.DrawWireDisc(target.center - mainAxis * target.thickness, mainAxis, target.radius);

					Handles.DrawWireDisc(target.center + rightAxis * target.radius, upAxis, target.thickness);
					Handles.DrawWireDisc(target.center - rightAxis * target.radius, upAxis, target.thickness);
					Handles.DrawWireDisc(target.center + upAxis * target.radius, rightAxis, target.thickness);
					Handles.DrawWireDisc(target.center - upAxis * target.radius, rightAxis, target.thickness);
				}
			}

			Handles.zTest = prevZTest;
		}
	}
}
