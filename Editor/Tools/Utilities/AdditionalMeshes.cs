using Akela.ExtendedPhysics;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace AkelaEditor.Utilities
{
	internal static class AdditionalMeshes
	{
		private static readonly MethodInfo _renameMethod = typeof(Editor).Assembly
			.GetType("UnityEditor.SceneHierarchyWindow")
			.GetMethod("FrameAndRenameNewGameObject", BindingFlags.Static | BindingFlags.NonPublic);

		[MenuItem("GameObject/3D Object/Torus")]
		private static void CreateTorusMesh()
		{
			CreateMeshWithComponent<TorusCollider>("Torus", "Torus");
		}

		private static void CreateMeshWithComponent<T>(string defaultName, string meshName) where T : Component
		{
			var parent = Selection.activeGameObject;

			var newObj = new GameObject(defaultName);

			if (parent)
				newObj.transform.SetParent(parent.transform, false);

			var meshFilter = newObj.AddComponent<MeshFilter>();
			meshFilter.mesh = Resources.Load<Mesh>(meshName);

			var meshRenderer = newObj.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = GraphicsSettings.defaultRenderPipeline != null ?
				GraphicsSettings.defaultRenderPipeline.defaultMaterial :
				AssetDatabase.GetBuiltinExtraResource<Material>("Default-Diffuse.mat");

			newObj.AddComponent<T>();

			Selection.activeGameObject = newObj;

			_renameMethod.Invoke(null, null);
		}
	}
}
