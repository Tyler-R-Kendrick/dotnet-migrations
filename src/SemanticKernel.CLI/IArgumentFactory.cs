using System.CommandLine;

namespace SKCLI;

internal interface IArgumentFactory
{
    Argument<T> Create<T>(string name,
        string? description = default,
        T? defaultValue = default,
        ArgumentArity? arity = default,
        bool isHidden = false)
    {
        description ??= "";
        var argument = new Argument<T>(name, description)
        {
            Arity = arity ?? ArgumentArity.ZeroOrOne,
            IsHidden = isHidden,
        };
        argument.SetDefaultValue(defaultValue);
        return argument;
    }
}

internal class ArgumentFactory : IArgumentFactory
{
}