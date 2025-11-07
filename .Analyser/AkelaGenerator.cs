using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        const string FROMTHIS_SYMBOL_NAME = "Akela.Behaviours.FromThisAttribute";
        const string FROMPARENTS_SYMBOL_NAME = "Akela.Behaviours.FromParentsAttribute";
        const string FROMCHILDREN_SYMBOL_NAME = "Akela.Behaviours.FromChildrenAttribute";
        const string MONITOR_SYMBOL_NAME = "Akela.Behaviours.GenerateHashForEveryFieldAttribute";
        const string HIDESCRIPTFIELD_SYMBOL_NAME = "Akela.Behaviours.HideScriptFieldAttribute";
        const string INTERNAL_WRAPPER_SYMBOL_NAME = "Akela.Tools.InternalWrapperAttribute";
        const string INTERNAL_METHOD_SYMBOL_NAME = "Akela.Tools.InternalMethodAttribute";

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new AkelaSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is AkelaSyntaxReceiver receiver) || receiver.Classes.Count == 0)
                return;

            var symbols = receiver.Classes
                .Select(x => (classDeclaration: x, semanticModel: context.Compilation.GetSemanticModel(x.SyntaxTree)))
                .Select(x => (INamedTypeSymbol)x.semanticModel.GetDeclaredSymbol(x.classDeclaration))
                .Where(x => x != null)
                .ToList();

            ProcessSerializableClasses(ref context, symbols.Where(x => !x.IsStatic && !x.IsGenericType && x.DeclaredAccessibility == Accessibility.Public));
            ProcessWrapperClasses(ref context, symbols.Where(x => !x.IsAbstract));
        }

        private static void ProcessSerializableClasses(ref GeneratorExecutionContext context, IEnumerable<INamedTypeSymbol> symbols)
        {
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

                // Dependencies check
                if (SymbolIsInstantiableFrom(symbol, MONOBEHAVIOUR_SYMBOL_NAME))
                {
                    var dependencyFields = symbol.GetMembers()
                        .Where(x =>
                            x.Kind == SymbolKind.Field
                        )
                        .Cast<IFieldSymbol>()
                        .Where(x =>
                            (
                                x.Type is INamedTypeSymbol y && SymbolIsInstantiableFrom(y, COMPONENT_SYMBOL_NAME) ||
                                x.Type is IArrayTypeSymbol z && SymbolIsInstantiableFrom((INamedTypeSymbol)z.ElementType, COMPONENT_SYMBOL_NAME)
                            ) &&
                            FieldIsSerializable(x)
                        )
                        .Select(x => (
                                field: x,
                                attr: x.GetAttributes()
                                    .Select(a => a.AttributeClass?.ToDisplayString())
                                    .Where(a => a == FROMTHIS_SYMBOL_NAME || a == FROMCHILDREN_SYMBOL_NAME || a == FROMPARENTS_SYMBOL_NAME)
                            )
                        )
                        .Where(x => x.attr.Count() == 1)
                        .Select(x => (x.field, x.attr.First()));

                    var sourceString = GenerateDependencies(symbol, dependencyFields);

                    if (!string.IsNullOrEmpty(sourceString))
                        context.AddSource($"{symbol.Name}_dependencies.g.cs", SourceText.From(sourceString, Encoding.UTF8));
                }
            }
        }

        private static void ProcessWrapperClasses(ref GeneratorExecutionContext context, IEnumerable<INamedTypeSymbol> symbols)
        {
            foreach (var symbol in symbols)
            {
                var attr = symbol.GetAttributes().FirstOrDefault(a => a.AttributeClass?.ToDisplayString() == INTERNAL_WRAPPER_SYMBOL_NAME);

                if (attr == null)
                    continue;

                var neighbouringType = attr.ConstructorArguments.Length > 0 ? (attr.ConstructorArguments[0].Value as INamedTypeSymbol)?.ToDisplayString() : null;
                var typeName = attr.ConstructorArguments.Length > 1 ? attr.ConstructorArguments[1].Value as string : null;

                if (neighbouringType == null || typeName == null)
                    continue;

                context.AddSource($"{symbol.Name}_delegates.g.cs", SourceText.From(GenerateWrapperDelegates(symbol, neighbouringType, typeName), Encoding.UTF8));
            }
        }

        #region Contextual Generators
        private static string GenerateSingleton(INamedTypeSymbol symbol)
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

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private static string GenerateDependencies(INamedTypeSymbol symbol, IEnumerable<(IFieldSymbol f, string a)> fields)
        {
            // Sanity Check
            if (!fields.Any())
                return null;

            // Generate source
            var source = new StringBuilder();

            source.Append(
                @"using UnityEngine;

"
            );

            AppendClassHeader(source, symbol, "ISerializationCallbackReceiver");

            source.Append(
                $@"
        public void OnAfterDeserialize() {{ }}
        
        public void OnBeforeSerialize()
        {{
#if UNITY_EDITOR
            if (!this.gameObject)
                return;
"
            );

            foreach (var dependencyField in fields)
            {
                string methodCall;

                if (dependencyField.a == FROMCHILDREN_SYMBOL_NAME)
                {
                    if (dependencyField.f.Type is IArrayTypeSymbol arraySymbol)
                        methodCall = $"GetComponentsInChildren<{arraySymbol.ElementType.ToDisplayString()}>";
                    else
                        methodCall = $"GetComponentInChildren<{dependencyField.f.Type.ToDisplayString()}>";
                }
                else if (dependencyField.a == FROMPARENTS_SYMBOL_NAME)
                {
                    if (dependencyField.f.Type is IArrayTypeSymbol arraySymbol)
                        methodCall = $"GetComponentsInParent<{arraySymbol.ElementType.ToDisplayString()}>";
                    else
                        methodCall = $"GetComponentInParent<{dependencyField.f.Type.ToDisplayString()}>";
                }
                else
                {
                    if (dependencyField.f.Type is IArrayTypeSymbol arraySymbol)
                        methodCall = $"GetComponents<{arraySymbol.ElementType.ToDisplayString()}>";
                    else
                        methodCall = $"GetComponent<{dependencyField.f.Type.ToDisplayString()}>";
                }

                source.Append(
                    $@"
            {dependencyField.f.Name} = {methodCall}();"
                );
            }

            source.Append(
                $@"
#endif
        }}"
            );

            AppendClassFooter(source, symbol);

            return source.ToString();
        }

        private static string GenerateMonitoringHash(INamedTypeSymbol symbol)
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

        private static string GenerateHideScriptFieldEditor(INamedTypeSymbol symbol)
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

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        private static string GenerateWrapperDelegates(INamedTypeSymbol symbol, string neighbouringType, string typeName)
        {
            var source = new StringBuilder();

            source.Append(
                @"using System;
using System.Reflection;

"
            );

            AppendClassHeader(source, symbol);

            source.Append(
                $@"
        private static readonly Type generated_wrappedType = typeof({neighbouringType}).Assembly.GetType(""{typeName}"");
"
            );

            // Get all delegates
            var delegates = EnumerateTypeHierarchy(symbol)
                .SelectMany(x => x.GetTypeMembers())
                .Where(x => x.TypeKind == TypeKind.Delegate && x.DelegateInvokeMethod != null);

            var initialisationCode = new StringBuilder();

            foreach (var @delegate in delegates)
            {
                var attr = @delegate.GetAttributes().FirstOrDefault(a => a.AttributeClass?.ToDisplayString() == INTERNAL_METHOD_SYMBOL_NAME);

                if (attr == null)
                    continue;

                var methodIsStatic = attr.NamedArguments.Where(x => x.Key == "IsStatic").Select(x => x.Value).FirstOrDefault().Value as bool? == true;
                var methodIsPrivate = attr.NamedArguments.Where(x => x.Key == "IsPrivate").Select(x => x.Value).FirstOrDefault().Value as bool? == true;

                string methodName;

                if (attr.NamedArguments.Any(x => x.Key == "MethodName"))
                    methodName = attr.NamedArguments.First(x => x.Key == "MethodName").Value.ToString();
                else
                    methodName = @delegate.Name.Substring(0, @delegate.Name.Length - 8);

                var returnType = @delegate.DelegateInvokeMethod?.ReturnsVoid ?? false ? "void" : @delegate.DelegateInvokeMethod?.ReturnType.ToDisplayString();

                var typedArgs = string.Join(", ", @delegate.DelegateInvokeMethod?.Parameters.Select(x =>
                    $"{RefKindToString(x.RefKind)}{x.Type.ToDisplayString()} {x.Name}" +
                    (x.HasExplicitDefaultValue ? x.ExplicitDefaultValue is string ? $" = \"{x.ExplicitDefaultValue}\"" : $" = {x.ExplicitDefaultValue}" : string.Empty)
                ));

                var args = string.Join(", ", @delegate.DelegateInvokeMethod?.Parameters.Select(x =>
                    $"{RefKindToString(x.RefKind)}{x.Name}"
                ));

                var bindingFlags = (
                                       methodIsPrivate ?
                                           "BindingFlags.NonPublic" :
                                           "BindingFlags.Public"
                                   ) +
                                   " | " +
                                   (
                                       methodIsStatic ?
                                           "BindingFlags.Static" :
                                           "BindingFlags.Instance"
                                   );

                var argsAsArray = "new Type[] { " +
                                  string.Join(", ", @delegate.DelegateInvokeMethod?.Parameters.Select(x =>
                                      $"typeof({x.Type.ToDisplayString()})" +
                                      (x.RefKind != RefKind.None ? ".MakeByRefType()" : string.Empty))
                                  ) +
                                  " }";

                var staticModifier = symbol.IsStatic || methodIsStatic ? "static " : string.Empty;

                source.Append(
                    $@"
        private static readonly MethodInfo {methodName}_methodInfo = generated_wrappedType.GetMethod(""{methodName}"", {bindingFlags}, null, {argsAsArray}, null);
        private {staticModifier}readonly {@delegate.Name} {methodName}_asDelegate;
        public {staticModifier}{returnType} {methodName}({typedArgs}) => {methodName}_asDelegate({args});
"
                );

                initialisationCode.Append(
                    @"
#if UNITY_EDITOR
            try {
#endif"
                );

                if (symbol.IsStatic || methodIsStatic)
                {
                    initialisationCode.Append(
                        $@"
                {methodName}_asDelegate = ({@delegate.Name}){methodName}_methodInfo.CreateDelegate(typeof({@delegate.Name}));
"
                    );
                }
                else
                {
                    initialisationCode.Append(
                        $@"
                {methodName}_asDelegate = ({@delegate.Name}){methodName}_methodInfo.CreateDelegate(typeof({@delegate.Name}), internalObject);"
                    );
                }

                initialisationCode.Append(
                    $@"
#if UNITY_EDITOR
            }} catch (NullReferenceException) {{ UnityEngine.Debug.LogError(""Exception rose during execution of internal wrapper methods. One of your methods might be ill-formed. Method name: '{methodName}'""); }}
#endif"
                );
            }

            source.Append(
                $@"
        {(symbol.IsStatic ? "static" : "public")} {symbol.Name}()
        {{"
            );

            if (!symbol.IsStatic)
            {
                source.Append(
                    @"
            var internalObject = Activator.CreateInstance(generated_wrappedType);
"
                );
            }

            source.Append($@"{initialisationCode}
        }}
");

            AppendClassFooter(source, symbol);

            return source.ToString();
        }

        private static void AppendClassHeader(StringBuilder source, INamedTypeSymbol symbol, params string[] additionalInterfaces)
        {
            var namespaceName = symbol.ContainingNamespace.ToDisplayString();

            var fullModifierList = new StringBuilder(AccessibilityToString(symbol.DeclaredAccessibility));

            if (symbol.IsStatic)
                fullModifierList.Append(" static");

            if (symbol.IsExtern)
                fullModifierList.Append(" extern");

            if (symbol.IsAbstract)
                fullModifierList.Append(" abstract");

            if (symbol.IsSealed)
                fullModifierList.Append(" sealed");

            if (symbol.IsReadOnly)
                fullModifierList.Append(" readonly");

            if (namespaceName != GLOBAL_NAMESPACE)
            {
                source.Append(
                    $@"namespace {namespaceName}
{{"
                );
            }

            source.Append(
                $@"
    {fullModifierList} partial class {symbol.Name}");

            string genericConstraints;

            if (symbol.IsGenericType)
            {
                var genericParameters = "<" +
                                        string.Join(", ", symbol.TypeParameters.Select(x =>
                                            (x.Variance == VarianceKind.Out ? "out " : x.Variance == VarianceKind.In ? "in " : string.Empty) +
                                            x.Name)) +
                                        ">";

                genericConstraints = string.Join(" ", symbol.TypeParameters
                    .Where(x => x.HasConstructorConstraint || x.HasNotNullConstraint || x.HasReferenceTypeConstraint || x.HasUnmanagedTypeConstraint || x.HasValueTypeConstraint || x.ConstraintTypes.Length > 0)
                    .Select(x =>
                    {
                        var constraints = new List<string>();

                        if (x.HasConstructorConstraint)
                            constraints.Add("new()");

                        if (x.HasNotNullConstraint)
                            constraints.Add("notnull");

                        if (x.HasReferenceTypeConstraint)
                            constraints.Add("class" + (x.ReferenceTypeConstraintNullableAnnotation == NullableAnnotation.Annotated ? "?" : string.Empty));

                        if (x.HasUnmanagedTypeConstraint)
                            constraints.Add("unmanaged");

                        if (x.HasValueTypeConstraint)
                            constraints.Add("struct");

                        constraints.AddRange(x.ConstraintTypes.Select((y, i) => y.Name + (x.ConstraintNullableAnnotations[i] == NullableAnnotation.Annotated ? "?" : string.Empty)));

                        return $"where {x.Name} : " + string.Join(", " + constraints);
                    }));

                source.Append(genericParameters);
            }
            else
            {
                genericConstraints = null;
            }

            if (additionalInterfaces.Length > 0)
                source.Append(" : " + string.Join(", ", additionalInterfaces));

            if (!string.IsNullOrEmpty(genericConstraints))
                source.Append(" " + genericConstraints);

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
                case TypeKind.Enum: return true;

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

                default: return false;
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

        private static string AccessibilityToString(Accessibility accessibility)
        {
            switch (accessibility)
            {
                case Accessibility.Private: return "private";
                case Accessibility.ProtectedAndInternal: return "protected internal";
                case Accessibility.Protected: return "protected";
                case Accessibility.Internal: return "internal";
                case Accessibility.Public: return "public";
                default: return null;
            }
        }

        private static string RefKindToString(RefKind refKind)
        {
            switch (refKind)
            {
                case RefKind.None: return string.Empty;
                case RefKind.Ref: return "ref ";
                case RefKind.Out: return "out ";
                case RefKind.In: return "in ";
                default: return null;
            }
        }
        #endregion
    }
}