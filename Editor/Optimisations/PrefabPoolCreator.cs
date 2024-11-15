using Akela.Optimisations;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AkelaEditor.Optimisations
{
	internal static class PrefabPoolCreator
	{
		[MenuItem("Assets/Create/Prefab Pool", true)]
		private static bool CanCreatePrefabPool()
		{
			if (Selection.gameObjects.Length == 0)
				return false;

			var selectedObject = Selection.gameObjects[0];

			if (!selectedObject || !selectedObject.TryGetComponent<PooledPrefab>(out _))
				return false;

			return true;
		}

		[MenuItem("Assets/Create/Prefab Pool", priority = -210)]
		private static void CreatePrefabPool()
		{
			var pooledPrefab = Selection.gameObjects[0].GetComponent<PooledPrefab>();

			var prefabPool = ScriptableObject.CreateInstance<PrefabPool>();
			prefabPool.SetPrefab(pooledPrefab);

			var path = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(
				Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0])) ?? string.Empty,
				pooledPrefab.gameObject.name + " Pool.asset"
			));

			AssetDatabase.CreateAsset(prefabPool, path);

			Selection.activeObject = prefabPool;
		}
	}
}
