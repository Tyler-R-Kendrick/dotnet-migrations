using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace InterfaceGenerator;

public readonly struct InterfaceToGenerate(string name, 
    SyntaxList<MemberDeclarationSyntax> values)
{
    public string Name { get; } = name;
    public Scope Scope { get; } = Scope.Public;
    public SyntaxList<MemberDeclarationSyntax> Values { get; } = values;
}
