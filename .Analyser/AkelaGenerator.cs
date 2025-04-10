using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace AkelaAnalyser
{
    [Generator]
    internal class AkelaGenerator : ISourceGenerator
    {
        const string GLOBAL_NAMESPACE = "<global namespace>";
        const string UNITY_NAMESPACE = "UnityEngine";
        const string UNITYOBJECT_SYMBOL_NAME = "UnityEngine.Object";
        const string MONOBEHAVIOUR_SYMBOL_NAME = "UnityEngine.MonoBehaviour";
        const string SCRIPTABLEOBJECT_SYMBOL_NAME = "UnityEngine.ScriptableObject";
        const string COMPONENT_SYMBOL_NAME = "UnityEngine.Component";
        const string SERIALIZEFIELD_SYMBOL_NAME = "UnityEngine.SerializeField";
        const string HIDEFIELD_SYMBOL_NAME = "UnityEngine.HideInInspector";

        const string SINGLETON_SYMBOL_NAME = "Akela.Behaviours.SingletonAttribute";
        const string DEPENDENCY_SYMBOL_NAME = "Akela.Behaviours.WithDependenciesAttribute";
        const string FROMPARENTS_SYMBOL_NAME = "Akela.Behaviours.FromParentsAttribute";
        const string FROMCHILDREN_SYMBOL_NAME = "Akela.Behaviours.FromChildrenAttribute";
        const string MONITOR_SYMBOL_NAME = "Akela.Behaviours.GenerateHashForEveryFieldAttribute";
        const string HIDESCRIPTFIELD_SYMBOL_NAME = "Akela.Behaviours.HideScriptFieldAttribute";

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new AkelaSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is AkelaSyntaxReceiver receiver) || receiver.Classes.Count == 0)
                return;

            var symbols = receiver.Classes
                .Select(x => (behaviour: x, semanticModel: context.Compilation.GetSemanticModel(x.SyntaxTree)))
                .Select(x => (INamedTypeSymbol)x.semanticModel.GetDeclaredSymbol(x.behaviour))
                .Where(x => x != null);

            foreach (var symbol in symbols)
            {
                var attributes = symbol.GetAttributes();

                foreach (var attr in attributes)
                {
                    switch (attr.AttributeClass?.ToDisplayString())
                    {
                        case SINGLETON_SYMBOL_NAME:
                            if (!SymbolIsInstantiableFrom(symbol, MONOBEHAVIOUR_SYMBOL_NAME) || symbol.IsAbstract)
                                continue;

                            context.AddSource($"{symbol.Name}_singleton.g.cs", SourceText.From(GenerateSingleton(symbol), Encoding.UTF8));
                            break;

                        case DEPENDENCY_SYMBOL_NAME:
                            if (!SymbolIsInstantiableFrom(symbol, MONOBEHAVIOUR_SYMBOL_NAME))
                                continue;

                            var dependencyContainerType = attr.ConstructorArguments.Length > 0 ? attr.ConstructorArguments[0].Value as INamedTypeSymbol : null;
                            var sourceString = GenerateDependencies(context, symbol, dependencyContainerType);

                            if (string.IsNullOrEmpty(sourceString))
                                continue;

                            context.AddSource($"{symbol.Name}_dependencies.g.cs", SourceText.From(sourceString, Encoding.UTF8));
                            break;

                        case MONITOR_SYMBOL_NAME:
                            if (!SymbolIsInstantiableFrom(symbol, MONOBEHAVIOUR_SYMBOL_NAME) || symbol.IsAbstract)
                                continue;

                            context.AddSource($"{symbol.Name}_fieldMonitoring.g.cs", SourceText.From(GenerateMonitoringHash(symbol), Encoding.UTF8));
                            break;

                        case HIDESCRIPTFIELD_SYMBOL_NAME:
                            if (!SymbolIsInstantiableFrom(symbol, MONOBEHAVIOUR_SYMBOL_NAME, SCRIPTABLEOBJECT_SYMBOL_NAME))
                                continue;

                            context.AddSource($"{symbol.Name}_hideScriptFieldEditor.g.cs", SourceText.From(GenerateHideScriptFieldEditor(symbol), Encoding.UTF8));
                            break;
                    }
                }
            }
        }

        #region Contextual Generators
        private string GenerateSingleton(INamedTypeSymbol symbol)
        {
            var source = new StringBuilder();

            source.Append(
                @"using Akela.Behaviours;

"
            );

            AppendClassHeader(source, symbol, "IInitialisableBehaviour");

            source.Append(
                $@"
        public static {symbol.Name} Main {{ get; private set; }}

        public void InitialiseBehaviour()
        {{
            Main = this;
        }}"
            );

            AppendClassFooter(source, symbol);

            return source.ToString();
        }

        private string GenerateDependencies(GeneratorExecutionContext context, INamedTypeSymbol symbol, INamedTypeSymbol dependencyContainerType)
        {
            // Sanity check
            if
            (
                dependencyContainerType == null ||
                dependencyContainerType.DeclaredAccessibility != Accessibility.Public ||
                !dependencyContainerType.IsValueType ||
                dependencyContainerType.IsGenericType ||
                !dependencyContainerType.IsSerializable
            )
            {
                context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
                        "AKELIB1101", // 1000 - com.akela.core ; 0100 - Akela.Behaviours
                        "Invalid behaviour dependencies",
                        "Type {0} cannot be used as a dependency container for component {1}. A dependency container must be a public, serializable, non-generic value type (struct).",
                        "Akela.Behaviours",
                        DiagnosticSeverity.Error,
                        true),
                    symbol.Locations.FirstOrDefault(),
                    dependencyContainerType?.ToDisplayString() ?? "undefined",
                    symbol.ToDisplayString()
                ));

                return null;
            }

            // Get all fields from the container
            var containerFields = dependencyContainerType.GetMembers()
                .Where(x =>
                    x.Kind == SymbolKind.Field &&
                    !x.IsStatic &&
                    x.DeclaredAccessibility == Accessibility.Public
                )
                .Cast<IFieldSymbol>()
                .Where(x =>
                    x.Type is INamedTypeSymbol y && SymbolIsInstantiableFrom(y, COMPONENT_SYMBOL_NAME) ||
                    x.Type is IArrayTypeSymbol z && SymbolIsInstantiableFrom((INamedTypeSymbol)z.ElementType, COMPONENT_SYMBOL_NAME)
                );

            // Generate source
            var source = new StringBuilder();

            source.Append(
                @"using UnityEngine;

"
            );

            AppendClassHeader(source, symbol, "ISerializationCallbackReceiver");

            source.Append(
                $@"
        [SerializeField, HideInInspector] protected {dependencyContainerType.ToDisplayString()} dep;
        
        public void OnAfterDeserialize() {{ }}
        
        public void OnBeforeSerialize()
        {{
#if UNITY_EDITOR
            dep = new {dependencyContainerType.ToDisplayString()}
            {{"
            );

            bool fromParents = false, fromChildren = false;

            foreach (var field in containerFields)
            {
                string methodCall;

                var fieldAttributes = field.GetAttributes();
                var hasParentAttr = fieldAttributes.Any(x => x.AttributeClass?.ToDisplayString() == FROMPARENTS_SYMBOL_NAME);
                var hasChildAttr = fieldAttributes.Any(x => x.AttributeClass?.ToDisplayString() == FROMCHILDREN_SYMBOL_NAME);

                if (hasParentAttr && hasChildAttr)
                {
                    context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor(
                            "AKELIB1102", // 1000 - com.akela.core ; 0100 - Akela.Behaviours
                            "Invalid behaviour dependencies",
                            "Using both [FromParents] and [FromChildren] on a dependency container field is not allowed.",
                            "Akela.Behaviours",
                            DiagnosticSeverity.Error,
                            true),
                        field.Locations.FirstOrDefault()
                    ));

                    continue;
                }

                if (!fromParents && hasParentAttr)
                {
                    fromParents = true;
                    fromChildren = false;
                }
                else if (!fromChildren && hasChildAttr)
                {
                    fromChildren = true;
                    fromParents = false;
                }

                if (fromChildren)
                {
                    if (field.Type is IArrayTypeSymbol arraySymbol)
                        methodCall = $"GetComponentsInChildren<{arraySymbol.ElementType.ToDisplayString()}>";
                    else
                        methodCall = $"GetComponentInChildren<{field.Type.ToDisplayString()}>";
                }
                else if (fromParents)
                {
                    if (field.Type is IArrayTypeSymbol arraySymbol)
                        methodCall = $"GetComponentsInParent<{arraySymbol.ElementType.ToDisplayString()}>";
                    else
                        methodCall = $"GetComponentInParent<{field.Type.ToDisplayString()}>";
                }
                else
                {
                    if (field.Type is IArrayTypeSymbol arraySymbol)
                        methodCall = $"GetComponents<{arraySymbol.ElementType.ToDisplayString()}>";
                    else
                        methodCall = $"GetComponent<{field.Type.ToDisplayString()}>";
                }

                source.Append(
                    $@"
                {field.Name} = {methodCall}(),"
                );
            }

            source.Append(
                $@"
            }};
#endif
        }}"
            );

            AppendClassFooter(source, symbol);

            return source.ToString();
        }

        private string GenerateMonitoringHash(INamedTypeSymbol symbol)
        {
            var source = new StringBuilder();

            source.Append(
                @"using Akela.Behaviours;
using UnityEngine;

"
            );

            AppendClassHeader(source, symbol);

            source.Append(
                $@"
        public override int GetHashCode()
        {{
            unchecked
            {{
                int hash = 17;
"
            );

            // Get all serialized fields
            var serializedFields = EnumerateTypeHierarchy(symbol)
                .SelectMany(x => x.GetMembers())
                .Where(x => x.Kind == SymbolKind.Field)
                .Cast<IFieldSymbol>()
                .Where(FieldIsSerializable);

            foreach (var field in serializedFields)
            {
                var fieldName = field.Name;

                if (field.Type.IsReferenceType)
                {
                    source.Append(
                        $@"
                hash = hash * 23 + (this.{fieldName} == null ? 0 : this.{fieldName}.GetHashCode());"
                    );
                }
                else
                {
                    source.Append(
                        $@"
                hash = hash * 23 + this.{fieldName}.GetHashCode();"
                    );
                }
            }

            source.Append(
                $@"

                return hash;
            }}
        }}"
            );

            AppendClassFooter(source, symbol);

            return source.ToString();
        }

        private string GenerateHideScriptFieldEditor(INamedTypeSymbol symbol)
        {
            var source = new StringBuilder();

            source.Append(
                @"#if UNITY_EDITOR
using UnityEditor;

"
            );

            AppendEditorClassHeader(source, symbol);

            source.Append(
                $@"
        public override void OnInspectorGUI()
        {{
            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject, ""m_Script"");
            serializedObject.ApplyModifiedProperties();
        }}"
            );

            AppendClassFooter(source, symbol);

            source.Append(@"
#endif");

            return source.ToString();
        }

        private static void AppendClassHeader(StringBuilder source, INamedTypeSymbol symbol, params string[] additionalInterfaces)
        {
            var namespaceName = symbol.ContainingNamespace.ToDisplayString();

            if (namespaceName != GLOBAL_NAMESPACE)
            {
                source.Append(
                    $@"namespace {namespaceName}
{{"
                );
            }

            source.Append(
                $@"
    public partial class {symbol.Name}");

            if (additionalInterfaces.Length > 0)
                source.Append(" : " + string.Join(", ", additionalInterfaces));

            source.Append(
                $@"
    {{"
            );
        }

        private static void AppendClassFooter(StringBuilder source, INamedTypeSymbol symbol)
        {
            var namespaceName = symbol.ContainingNamespace.ToDisplayString();

            source.Append(
                $@"
    }}"
            );

            if (namespaceName != GLOBAL_NAMESPACE)
            {
                source.Append(
                    $@"
}}"
                );
            }
        }

        private static void AppendEditorClassHeader(StringBuilder source, INamedTypeSymbol symbol, params string[] additionalInterfaces)
        {
            var namespaceName = symbol.ContainingNamespace.ToDisplayString();

            if (namespaceName != GLOBAL_NAMESPACE)
            {
                source.Append(
                    $@"namespace {namespaceName}
{{"
                );
            }

            source.Append(
                $@"
    [CustomEditor(typeof({symbol.Name}), isFallback = true)]
    internal sealed class {symbol.Name}_HideScriptFieldEditor : Editor");

            if (additionalInterfaces.Length > 0)
                source.Append(", " + string.Join(", ", additionalInterfaces));

            source.Append(
                $@"
    {{"
            );
        }
        #endregion

        #region Tools
        private static bool SymbolIsInstantiableFrom(INamedTypeSymbol classSymbol, params string[] baseClass)
        {
            var isValidType = false;
            var baseType = classSymbol.BaseType;

            while (baseType != null && !isValidType)
            {
                if (baseClass.Contains(baseType.ToDisplayString()))
                    isValidType = true;
                else
                    baseType = baseType.BaseType;
            }

            if (!isValidType)
                return false;

            return true;
        }

        private static bool FieldIsSerializable(IFieldSymbol field)
        {
            var attr = field.GetAttributes();

            if
            (
                field.DeclaredAccessibility != Accessibility.Public &&
                (
                    attr.Any(x => x.AttributeClass?.ToDisplayString() == HIDEFIELD_SYMBOL_NAME) ||
                    attr.All(x => x.AttributeClass?.ToDisplayString() != SERIALIZEFIELD_SYMBOL_NAME)
                ) ||
                field.IsStatic ||
                field.IsConst ||
                field.IsReadOnly ||
                !field.Type.IsType ||
                field.Type.IsAnonymousType
            )
            {
                return false;
            }

            switch (field.Type.TypeKind)
            {
                case TypeKind.Enum:
                    return true;

                case TypeKind.Class:
                    return field.Type.SpecialType == SpecialType.System_String ||
                           ((INamedTypeSymbol)field.Type).IsSerializable ||
                           SymbolIsInstantiableFrom((INamedTypeSymbol)field.Type, UNITYOBJECT_SYMBOL_NAME);

                case TypeKind.Struct:
                    return (field.Type.IsUnmanagedType && !field.Type.IsTupleType && field.Type.Kind != SymbolKind.PointerType) ||
                           ((INamedTypeSymbol)field.Type).IsSerializable ||
                           field.Type.ContainingNamespace.ToDisplayString() == UNITY_NAMESPACE;

                case TypeKind.Array:
                    return ((IArrayTypeSymbol)field.Type).ElementType is INamedTypeSymbol elemType &&
                           (
                               elemType.IsSerializable ||
                               SymbolIsInstantiableFrom(elemType, UNITYOBJECT_SYMBOL_NAME)
                           );

                default:
                    return false;
            }
        }

        private static IEnumerable<INamedTypeSymbol> EnumerateTypeHierarchy(INamedTypeSymbol leaf)
        {
            do
            {
                yield return leaf;
                leaf = leaf.BaseType;
            }
            while (leaf != null);
        }
        #endregion
    }
}