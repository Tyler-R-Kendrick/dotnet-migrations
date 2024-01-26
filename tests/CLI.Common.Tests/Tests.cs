using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace CLI.Common.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(@"
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            using System.Threading.Tasks;

            namespace ConsoleApp1
            {
                class Program
                {
                    T Add<T>(T a, T b)
                        where T : INumber<T> => a + b;
                    static void Main(string[] args)
                    {
                    }
                }
            }
        ");
        var root = syntaxTree.GetRoot();
        var usings = root.DescendantNodes().OfType<UsingDirectiveSyntax>();
        var classDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>().First();
        var methodDeclarations = classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>();
        foreach(var method in methodDeclarations)
        {
            var genericParameters = method.TypeParameterList?.Parameters
                .Select(static p => p.Identifier.Text);
            if(genericParameters != null && genericParameters.Any())
                Console.WriteLine($"Generic parameters: {string.Join(", ", genericParameters)}");
            var declarationString = GenerateMethodDeclarationString(method);
            Console.WriteLine(declarationString);
        }
        Assert.Pass();
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
        var parameterString = parameters!.Any() && parameters != null ? string.Join(", ", parameters) : "";
        var declarationString = $"{returnType} {methodName}{genericParameterString}({parameterString});";
        declarationString += @"
                ";
        return declarationString;
    }
}
