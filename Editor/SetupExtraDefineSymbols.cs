using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;

namespace AkelaEditor
{
    [InitializeOnLoad]
    internal static class SetupExtraDefineSymbols
    {
        private static readonly (string symbol, string assembly)[] _symbolAssemblyCouple = new (string symbol, string assembly)[]
        {
            ("AKELA_FMOD", "FMODUnity"),
            ("AKELA_VINSPECTOR", "VInspector")
        };

        static SetupExtraDefineSymbols()
        {
            var assemblyNames = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetName().Name).ToList();
            var selectedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

            PlayerSettings.GetScriptingDefineSymbols(selectedBuildTarget, out var symbolsArr);

            var symbols = symbolsArr.ToList();
            var isDirty = false;

            foreach (var (symbol, assembly) in _symbolAssemblyCouple)
            {
                if (assemblyNames.Contains(assembly) && !symbols.Contains(symbol))
                {
                    symbols.Add(symbol);
                    isDirty = true;
                }
                else if (!assemblyNames.Contains(assembly) && symbols.Contains(symbol))
                {
                    symbols.Remove(symbol);
                    isDirty = true;
                }
            }

            if (!isDirty)
                return;

            PlayerSettings.SetScriptingDefineSymbols(selectedBuildTarget, symbols.ToArray());
        }
    }
}