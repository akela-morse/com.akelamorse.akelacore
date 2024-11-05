using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace AkelaAnalyser
{
	internal class AkelaSyntaxReceiver : ISyntaxReceiver
	{
		public List<ClassDeclarationSyntax> Classes { get; } = new List<ClassDeclarationSyntax>();

		public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
		{
			// Only test for public, non-abstract, non-static, non-generic classes

			if (!(syntaxNode is ClassDeclarationSyntax classDeclaration))
				return;

			if (
				!classDeclaration.Modifiers.Any(x => x.IsKind(SyntaxKind.PublicKeyword)) || // Public
				!classDeclaration.Modifiers.All(x => !x.IsKind(SyntaxKind.StaticKeyword)) || // Non-static
				!classDeclaration.Modifiers.All(x => !x.IsKind(SyntaxKind.AbstractKeyword)) || // Non-abstract
				!(classDeclaration.TypeParameterList is null) // Non-generic
			)
			{
				return;
			}

			Classes.Add(classDeclaration);
		}
	}
}
