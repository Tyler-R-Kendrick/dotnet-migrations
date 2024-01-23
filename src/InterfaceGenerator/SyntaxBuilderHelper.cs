using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace InterfaceGenerator;

public static class SyntaxBuilderHelper
{
    public const string
        NewLine = "\r\n",
        Indent = "\t",
        Space = " ",
        Empty = "";

    public static string JoinNewLine(IEnumerable<string?> values)
        => string.Join(NewLine, values.Where(x => x is not null));
    public static string JoinList(IEnumerable<string?>? values)
        => string.Join(", ", values.Where(x => x is not null));
    public static string JoinSpace(IEnumerable<string?>? values)
        => string.Join(Space, values.Where(x => x is not null));
    public static bool Any(IEnumerable<string?>? values)
        => values is not null && values.Any(x => x is not null);

    public static string ToTrimmedString(this SyntaxNode syntax)
        => syntax.ToFullString().Trim();

    public static string ToTrimmedString(this BasePropertyDeclarationSyntax syntax)
        => syntax.Type?.ToTrimmedString() ?? "object";

    public static StringBuilder GenerateLine(this StringBuilder builder, Func<string> func)
    {
        builder.AppendLine(func());
        builder.AppendLine();
        return builder;
    }
}
