using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace AkelaEditor.Tools
{
	public static class SymlinkMaker
	{
		const string LAST_DIR_KEY = "AkelaSymlinkMakerLastDir";

		[MenuItem("Assets/Create/Symlink...", false, -30)]
		static void MakeSymlink()
		{
			var lastDirectoryPath = EditorPrefs.GetString(LAST_DIR_KEY, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
			var assetPath = EditorUtility.OpenFilePanel("Create Symlink to Asset", lastDirectoryPath, string.Empty);

			if (string.IsNullOrEmpty(assetPath))
				return;

			EditorPrefs.SetString(LAST_DIR_KEY, Path.GetDirectoryName(assetPath));

			var fileName = Path.GetFileNameWithoutExtension(assetPath);
			var fileExt = Path.GetExtension(assetPath);

			string destinationPath;

			if (Selection.assetGUIDs.Length == 0)
				destinationPath = Path.Combine(Application.dataPath, fileName);
			else
				destinationPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]), fileName);

			destinationPath = Regex.Replace(destinationPath, @"[\\\/]", Path.DirectorySeparatorChar.ToString()) + fileExt;

			var process = new Process()
			{
				StartInfo = new ProcessStartInfo
				{
					WindowStyle = ProcessWindowStyle.Hidden,
					FileName = "cmd.exe",
					Arguments = @$"/C mklink ""{destinationPath}"" ""{assetPath}"""
				}
			};

			process.Start();
			process.WaitForExit();

			AssetDatabase.Refresh();
		}
	}
}
