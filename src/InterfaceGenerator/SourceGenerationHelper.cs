using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace InterfaceGenerator;

//TODO: Add support for interface inheritance.
//TODO: Add support for interface name override.
//TODO: Add support for generic interfaces.
//TODO: Add support for explicit implementations for non-public interfaces.
//TODO: Implement using scraping from the original class.
//TODO: Copy summary documentation from the original class.
public static class SourceGenerationHelper
{
    private const string
        NewLine = "\r\n",
        Indent = "\t",
        Space = " ",
        Empty = "";

    static string JoinNewLine(IEnumerable<string?> values)
        => string.Join(NewLine, values.Where(x => x is not null));
    static string JoinList(IEnumerable<string?>? values)
        => string.Join(", ", values.Where(x => x is not null));
    static string JoinSpace(IEnumerable<string?>? values)
        => string.Join(Space, values.Where(x => x is not null));
    static bool Any(IEnumerable<string?>? values)
        => values is not null && values.Any(x => x is not null);

    static InterfaceToGenerate GetInterfaceToGenerate(ClassDeclarationSyntax declarationSyntax)
    {
        var className = declarationSyntax.Identifier.Text;
        var classMembers = declarationSyntax.Members;
        var interfaceGeneratorAttribute = declarationSyntax.AttributeLists
            .SelectMany(static a => a.Attributes)
            .FirstOrDefault(static a => a.Name.ToString() == "InterfaceGenerator");

        //TODO: Get this working
        var interfaceNameSyntax = interfaceGeneratorAttribute?.ArgumentList?.Arguments
            .FirstOrDefault(static a => a.NameEquals?.Name.ToString() == "Name"
                || a.NameColon?.Name.ToString() == "name"
                || a.IsKind(SyntaxKind.StringLiteralExpression));
        var interfaceName = interfaceNameSyntax is null
            ? $"I{className}"
            : interfaceNameSyntax.Expression switch
            {
                LiteralExpressionSyntax l => l.Token.ValueText,
                _ => throw new InvalidOperationException(
                    $"Invalid name `{interfaceNameSyntax.Expression}`. Valid names are strings.")
            };

        //TODO: Get this working
        var interfaceScopeSyntax = interfaceGeneratorAttribute?.ArgumentList?.Arguments
            .FirstOrDefault(static a => a.NameEquals?.Name.ToString() == "Scope"
                || a.NameColon?.Name.ToString() == "scope"
                || a.IsKind(SyntaxKind.IdentifierName));
        var interfaceScope = interfaceScopeSyntax is null
            ? Scope.Public
            : interfaceScopeSyntax.Expression switch
            {
                MemberAccessExpressionSyntax m => m.Name.ToString() switch
                {
                    "Public" => Scope.Public,
                    "Internal" => Scope.Internal,
                    _ => throw new InvalidOperationException(
                        $"Invalid scope `{m.Name}`. Valid scopes are `Public`, `Internal`, `Protected`, and `Private`.")
                },
                _ => throw new InvalidOperationException(
                    $"Invalid scope `{interfaceScopeSyntax.Expression}`. Valid scopes are `Public`, `Internal`, `Protected`, and `Private`.")
            };
        return new InterfaceToGenerate(interfaceName, classMembers, interfaceScope);
    }

    public static string GenerateExtensionClass(ClassDeclarationSyntax syntax)
    {
        var interfaceToGenerate = GetInterfaceToGenerate(syntax);
        var sb = new StringBuilder();

        //needed for a nullable context.
        sb.AppendLine("#nullable enable");
        sb.AppendLine();

        var @usings = GenerateUsingString(syntax);
        sb.AppendLine(@usings);
        sb.AppendLine();

        var @namespace = GenerateNamespaceString(syntax);
        sb.AppendLine(@namespace);
        sb.AppendLine();

        var interfaceString = GenerateInterfaceString(interfaceToGenerate);
        sb.AppendLine(interfaceString);
        sb.AppendLine();

        var classString = GenerateClassString(syntax, interfaceToGenerate);
        sb.AppendLine(classString);
        sb.AppendLine();

        sb.AppendLine("#nullable restore");
        sb.AppendLine();
        return sb.ToString();
    }

    private static string GenerateClassString(
        ClassDeclarationSyntax syntax,
        InterfaceToGenerate interfaceToGenerate)
    {
        var className = syntax.Identifier.Text;
        var interfaceName = interfaceToGenerate.Name;
        var modifiers = syntax.Modifiers;
        var baseList = syntax.BaseList;
        var classModifiers = modifiers.Select(static m => m.ToString());
        var hasPartial = classModifiers.Any(static m => m == "partial");
        string BuildInheritanceString()
        {
            var classModifierString = Any(classModifiers)
                ? JoinSpace(classModifiers) + Space
                : Empty;
            return $"{classModifierString}class {className} : {interfaceName} {{}}";
        }
        var implementsGeneratedInterface = baseList is not null
            && baseList.Types.Any(t => t.ToTrimmedString() == interfaceName);
        return implementsGeneratedInterface
            ? Empty
            : hasPartial
                ? BuildInheritanceString()
                : throw new InvalidOperationException(
                    $"Class `{className}` must be partial or must implement `{interfaceName}`.");
    }

    private static string GenerateInterfaceString(InterfaceToGenerate interfaceToGenerate)
    {
        //TODO: Add support for "private" with default implementations. Perhaps through abstract partial members?
        //TODO: Add support for "static" members with default implementations.
        //TODO: Add support for "static abstract" members.
        string[] validModifiers = ["public", "internal", "protected", "partial"];
        // var classDeclarationModifiers = syntax.Modifiers
        //     .Select(static m => m.ToString())
        //     .Intersect(validModifiers);
        // var modifiersString = string.Join(" ", classDeclarationModifiers);
        var modifiersString = "public partial";
        var values = interfaceToGenerate.Values
            .Where(static m => m.Modifiers.Any(static m => m.ToString() == "public"))
            .Select(x => x switch
            {
                PropertyDeclarationSyntax p => GeneratePropertyString(p),
                EventFieldDeclarationSyntax e => GenerateEventFieldString(e),
                EventDeclarationSyntax e => GenerateEventDeclarationString(e),
                IndexerDeclarationSyntax i => GenerateIndexerDeclarationString(i),
                MethodDeclarationSyntax m => GenerateMethodDeclarationString(m),
                _ => Empty
            })
            .Select(static v => $"{Indent}{v}");
        var valuesString = JoinNewLine(values);
        var interfaceName = interfaceToGenerate.Name;
        var declarationString = $"{modifiersString} interface {interfaceName}";
        return $"{declarationString}{NewLine}{{{NewLine}{valuesString}{NewLine}}}";
    }

    private static string GenerateXmlDocString(CSharpSyntaxNode syntax)
    {
        var trivias = syntax.GetLeadingTrivia();
        var xmlCommentTrivia = trivias
            .Where(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia)
                || t.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia))
            .Select(t => t.GetStructure())
            .Select(t => t?.ToTrimmedString())
            .Where(t => t is not null);
        return xmlCommentTrivia.Any()
            ? JoinNewLine(xmlCommentTrivia) + NewLine
            : Empty;
    }

    private static string GenerateUsingString(ClassDeclarationSyntax syntax)
    {
        var usings = syntax.SyntaxTree
            .GetRoot()
            .DescendantNodes()
            .OfType<UsingDirectiveSyntax>()
            .Select(static u => $"{u.ToTrimmedString()}");
        return JoinNewLine(usings);
    }

    private static string GenerateNamespaceString(ClassDeclarationSyntax syntax)
    {
        var success = SyntaxNodeHelper.TryGetParentSyntax<NamespaceDeclarationSyntax>(syntax, out var ns);
        success |= SyntaxNodeHelper.TryGetParentSyntax<FileScopedNamespaceDeclarationSyntax>(syntax, out var fns);
        if(!success)
            throw new InvalidOperationException(
                $"Class `{syntax.Identifier.Text}` must be declared in a namespace.");
        var @namespace = ns?.Name.ToString() ?? fns?.Name.ToString() ?? "GeneratedNamespace";
        return $"namespace {@namespace};";
    }

    private static string GenerateMethodDeclarationString(MethodDeclarationSyntax member)
    {
        var returnType = member.ReturnType.ToTrimmedString();
        var methodName = member.Identifier.Text;
        //TODO: Add support for generic methods.
        //TODO: Add support for constraints
        //TODO: Copy attributes from the original method.
        //TODO: Add support for optional parameters.
        //TODO: Add support for modifiers on parameters.
        var genericParameters = member.TypeParameterList?.Parameters
            .Select(static p => p.Identifier.Text);
        var genericParameterString = Any(genericParameters)
            ? $"<{JoinList(genericParameters)}>"
            : Empty;
        var parameters = member.ParameterList?.Parameters.Select(static p => p.ToTrimmedString());
        var parameterString = Any(parameters) ? JoinList(parameters) : Empty;
        return $"{returnType} {methodName}{genericParameterString}({parameterString});";
    }

    private static string GenerateIndexerDeclarationString(IndexerDeclarationSyntax member)
    {
        var returnType = member.ToTrimmedString();
        var accessors = member.AccessorList?.Accessors
            .FilterInstanceMembers()
            .Select(x => $"{x.Keyword};");
        var accessorString = Any(accessors) ? Empty : JoinSpace(accessors);
        return $"{returnType} this[int newValue] {{ {accessorString} }}";
    }

    private static string GenerateEventDeclarationString(EventDeclarationSyntax member)
    {
        var eventReturnType = member.ToTrimmedString();
        var eventName = member.Identifier.Text;
        return $"event {eventReturnType} {eventName};";
    }

    private static string GenerateEventFieldString(EventFieldDeclarationSyntax member)
    {
        var declaration = member.Declaration;
        var eventReturnType = declaration.ToTrimmedString();
        var variables = declaration.Variables.Select(static v => v.Identifier.Text);
        var variableString = JoinNewLine(variables);
        return $"event {eventReturnType} {variableString};";
    }

    private static string GeneratePropertyString(PropertyDeclarationSyntax member)
    {
        var returnType = member.ToTrimmedString() ?? "object";
        var propertyName = member.Identifier.Text;
        var accessors = member.AccessorList
            ?.Accessors
            .FilterInstanceMembers()
            .Select(x => $"{x.Keyword};");
        var accessorString = Any(accessors) ? JoinSpace(accessors) : "get;";
        var docString = GenerateXmlDocString(member);
        return $"{docString}{NewLine}{returnType} {propertyName} {{ {accessorString } }}";
    }

    private static IEnumerable<AccessorDeclarationSyntax> FilterInstanceMembers(
        this SyntaxList<AccessorDeclarationSyntax> syntaxList)
        => syntaxList
            .Where(x => x != null)
            .Where(x => !x!.Modifiers.Any(static m => m.ToString() == "static"))
            .Where(x => x!.Modifiers.Any(static m => m.ToString() == "public") || x.Modifiers.Count == 0);
            
    private static string ToTrimmedString(this SyntaxNode syntax)
        => syntax.ToFullString().Trim();

    private static string ToTrimmedString(this BasePropertyDeclarationSyntax syntax)
        => syntax.Type?.ToTrimmedString() ?? "object";
}
