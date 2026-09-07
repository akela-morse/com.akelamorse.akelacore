using System.Reflection;
#if UNITY_EDITOR
using System.Runtime.CompilerServices;
#endif

[assembly: AssemblyTitle("AkelaCore.Runtime")]
[assembly: AssemblyDescription("My Unity toolbox.")]
[assembly: AssemblyProduct("Akela Core")]
[assembly: AssemblyCompany("Akela Morse")]
[assembly: AssemblyVersion("1.0.0")]

#if UNITY_EDITOR
[assembly: InternalsVisibleTo("AkelaCore.Editor")]
#endif