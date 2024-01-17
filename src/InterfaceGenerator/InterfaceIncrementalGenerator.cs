using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace InterfaceGenerator;

[Generator(LanguageNames.CSharp)]
public class InterfaceIncrementalGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {   
        var attributes = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
            .Where(static m => m is not null);

        context.RegisterSourceOutput(attributes,
            static (spc, cdl) => 
            {
                if (cdl is not null)
                {
                    Execute(cdl, spc);
                }
            });
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode syntaxNode)
    {
        return syntaxNode is ClassDeclarationSyntax cds
               && cds.AttributeLists.Count > 0
               && cds.AttributeLists
                   .SelectMany(static al => al.Attributes)
                   .Any(static a => a.Name.ToString() == "InterfaceGenerator");
    }

    private static ClassDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext generatorSyntaxContext)
    {
        if (generatorSyntaxContext.Node is not ClassDeclarationSyntax classDeclarationSyntax)
            return null;
        //if not null, get the attribute.
        var attribute = classDeclarationSyntax.AttributeLists
            .SelectMany(static al => al.Attributes)
            .FirstOrDefault(static a => a.Name.ToString() == nameof(InterfaceGeneratorAttribute));

        //change the class declaration to implement the generated interface, if it doesn't already.
        if (attribute is not null)
        {
            if (!classDeclarationSyntax.Modifiers.Any(static m => m.ToString() == "partial"))
                return null;
            var interfaceName = $"I{classDeclarationSyntax.Identifier.Text}";
            if (classDeclarationSyntax.BaseList is null
                || !classDeclarationSyntax.BaseList.Types.Any(t => t.ToString() == interfaceName))
                classDeclarationSyntax = classDeclarationSyntax.AddBaseListTypes(
                    SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(interfaceName)));
        }
        return classDeclarationSyntax;
    }

    private static void Execute(ClassDeclarationSyntax classDeclarationSyntax, SourceProductionContext sourceProductionContext)
    {
        var name = classDeclarationSyntax.Identifier.Text;
        // generate the source code and add it to the output
        string result = SourceGenerationHelper.GenerateExtensionClass(classDeclarationSyntax);
        // Create a separate partial class file for each enum
        sourceProductionContext.AddSource($"{name}.I{name}.g.cs", SourceText.From(result, Encoding.UTF8));
    }
}
