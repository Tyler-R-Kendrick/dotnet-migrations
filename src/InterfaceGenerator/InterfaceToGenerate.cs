using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace InterfaceGenerator;

public readonly struct InterfaceToGenerate(string name,
    SyntaxList<MemberDeclarationSyntax> values, Scope scope = Scope.Public)
{
    public string Name { get; } = name;
    public Scope Scope { get; } = scope;
    public SyntaxList<MemberDeclarationSyntax> Values { get; } = values;
}
