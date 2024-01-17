using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace InterfaceGenerator;

public struct InterfaceToGenerate
{
    public string Name { get; }
    public SyntaxList<MemberDeclarationSyntax> Values { get; }

    public InterfaceToGenerate(string name, SyntaxList<MemberDeclarationSyntax> values)
    {
        Name = name;
        Values = values;
    }
}
