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
    static InterfaceToGenerate GetInterfaceToGenerate(ClassDeclarationSyntax declarationSyntax)
    {
        var className = declarationSyntax.Identifier.Text;
        var classMembers = declarationSyntax.Members;
        var interfaceGeneratorAttribute = declarationSyntax.AttributeLists
            .SelectMany(static a => a.Attributes)
            .FirstOrDefault(static a => a.Name.ToString() == "InterfaceGenerator");
        var interfaceScope = interfaceGeneratorAttribute?.ArgumentList?.Arguments
            .FirstOrDefault(static a => a.NameEquals?.Name.ToString() == "Scope");
        return new InterfaceToGenerate(className, classMembers);
    }

    public static string GenerateExtensionClass(ClassDeclarationSyntax syntax)
    {
        var sb = new StringBuilder();

        //needed for a nullable context.
        sb.AppendLine("#nullable enable");

        var @usings = GenerateUsingString(syntax);
        sb.AppendLine(@usings);

        var @namespace = GenerateNamespaceString(syntax);
        sb.AppendLine(@namespace);

        var interfaceString = GenerateInterfaceString(syntax);
        sb.AppendLine(interfaceString);
        
        var classString = GenerateClassString(syntax);
        sb.AppendLine(classString);

        return sb.ToString();
    }

    private static string GenerateClassString(ClassDeclarationSyntax syntax)
    {
        var className = syntax.Identifier.Text;
        var interfaceName = $"I{className}";
        var modifiers = syntax.Modifiers;
        var baseList = syntax.BaseList;
        var classModifiers = modifiers.Select(static m => m.ToString());
        var hasPartial = classModifiers.Any(static m => m == "partial");
        string BuildInheritanceString()
        {
            var classModifierString = string.Join(" ", classModifiers);
            var declarationString = $"{classModifierString} class {className}";
            declarationString += $" : {interfaceName}";
            declarationString += @"
            {
            }";
            return declarationString;
        }
        var implementsGeneratedInterface = baseList is not null
            && baseList.Types.Any(t => t.ToString() == interfaceName);
        return implementsGeneratedInterface
            ? string.Empty
            : hasPartial
                ? BuildInheritanceString()
                : throw new InvalidOperationException(
                    $"Class `{className}` must be partial or must implement `{interfaceName}`.");
    }

    private static string GenerateInterfaceString(ClassDeclarationSyntax syntax)
    {
        //TODO: Add support for "private" with default implementations. Perhaps through abstract partial members?
        //TODO: Add support for "static" members with default implementations.
        //TODO: Add support for "static abstract" members.
        string[] validModifiers = ["public", "internal", "protected", "partial"];
        var classDeclarationModifiers = syntax.Modifiers
            .Select(static m => m.ToString())
            .Intersect(validModifiers);
        var modifiersString = string.Join(" ", classDeclarationModifiers);
        modifiersString = "public partial";

        var toGenerate = GetInterfaceToGenerate(syntax);
        var values = toGenerate.Values
            .Where(static m => m.Modifiers.Any(static m => m.ToString() == "public"))
            .Select(x => x switch
            {
                PropertyDeclarationSyntax p => GeneratePropertyString(p),
                EventFieldDeclarationSyntax e => GenerateEventFieldString(e),
                EventDeclarationSyntax e => GenerateEventDeclarationString(e),
                IndexerDeclarationSyntax i => GenerateIndexerDeclarationString(i),
                MethodDeclarationSyntax m => GenerateMethodDeclarationString(m),
                _ => ""
            });
        var valuesString = string.Join(@"
                ", values);
        var interfaceName = $"I{toGenerate.Name}";
        var declarationString = $"{modifiersString} interface {interfaceName}";
        declarationString += @"
        {
            " + valuesString + @"
        }";
        return declarationString;
    }

    private static string GenerateUsingString(ClassDeclarationSyntax syntax)
    {
        var usings = syntax.SyntaxTree
            .GetRoot()
            .DescendantNodes()
            .OfType<UsingDirectiveSyntax>();
        var usingString = string.Join(@"
            ", usings.Select(static u => $"{u.ToFullString().Trim()}"));
        return usingString;
    }

    private static string GenerateNamespaceString(ClassDeclarationSyntax syntax)
    {
        var success = SyntaxNodeHelper.TryGetParentSyntax<NamespaceDeclarationSyntax>(syntax, out var ns);
        success |= SyntaxNodeHelper.TryGetParentSyntax<FileScopedNamespaceDeclarationSyntax>(syntax, out var fns);
        var @namespace = ns?.Name.ToString() ?? fns?.Name.ToString() ?? "UnknownNamespace";
        var declarationString = $"namespace {@namespace};";
        return declarationString;
    }

    private static string GenerateMethodDeclarationString(MethodDeclarationSyntax member)
    {
        var returnType = member.ReturnType.ToFullString().Trim();
        var methodName = member.Identifier.Text;
        //TODO: Add support for generic methods.
        //TODO: Add support for constraints
        //TODO: Copy attributes from the original method.
        //TODO: Add support for optional parameters.
        //TODO: Add support for modifiers on parameters.
        var genericParameters = member.TypeParameterList?.Parameters
            .Select(static p => p.Identifier.Text);
        var genericParameterString = string.Empty;
        if(genericParameters != null && genericParameters.Any())
        {
            genericParameterString = $"<{string.Join(", ", genericParameters)}>";
        }
        var parameters = member.ParameterList?.Parameters.Select(static p => p.ToFullString().Trim());
        var parameterString = parameters.Any() ? string.Join(", ", parameters) : "";
        var declarationString = $"{returnType} {methodName}{genericParameterString}({parameterString});";
        declarationString += @"
                ";
        return declarationString;
    }

    private static string GenerateIndexerDeclarationString(IndexerDeclarationSyntax member)
    {
        var returnType = member.Type.ToFullString().Trim();
        var accessors = member.AccessorList?.Accessors
            .Where(x => !x.Modifiers.Any(static m => m.ToString() == "static"))
            .Where(x => x.Modifiers.Any(static m => m.ToString() == "public") || x.Modifiers.Count == 0)
            .Select(x => $"{x.Keyword};");
        var accessorString = !accessors.Any() ? "" : string.Join(" ", accessors);
        var declarationString = $"{returnType} this[int newValue] {{ {accessorString} }}";
        declarationString += @"
                ";
        return declarationString;
    }

    private static string GenerateEventDeclarationString(EventDeclarationSyntax member)
    {
        var eventReturnType = member.Type.ToFullString().Trim();
        var eventName = member.Identifier.Text;
        var declarationString = $"event {eventReturnType} {eventName};";
        declarationString += @"
                ";
        return declarationString;
    }

    private static string GenerateEventFieldString(EventFieldDeclarationSyntax member)
    {
        var eventReturnType = member.Declaration.Type.ToFullString().Trim();
        var variables = member.Declaration.Variables.Select(static v => v.Identifier.Text);
        var variableString = string.Join(@", 
                ", variables);
        var declarationString = $"event {eventReturnType} {variableString};";
        declarationString += @"
                ";
        return declarationString;
    }

    private static string GeneratePropertyString(PropertyDeclarationSyntax member)
    {
        var returnType = member.Type?.ToFullString().Trim() ?? "object";
        var propertyName = member.Identifier.Text;
        var accessors = member.AccessorList?.Accessors
            .Where(x => x is not null)
            .Where(x => !x.Modifiers.Any(static m => m.ToString() == "static"))
            .Where(x => x.Modifiers.Any(static m => m.ToString() == "public") || x.Modifiers.Count == 0)
            .Select(x => $"{x.Keyword};");
        var hasAccessors = accessors != null && accessors.Any();
        var accessorString = hasAccessors ? string.Join(" ", accessors) : "get;";
        var declarationString = $"{returnType} {propertyName} {{ {accessorString } }}";
        declarationString += @"
                ";
        return declarationString;
    }
}
