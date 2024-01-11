using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.SkillDefinition;

namespace SKCLI;

internal static class KernelHelperExtensions
{
    public static IDictionary<string, IEnumerable<FunctionView>> GetFunctionViews(this IKernel kernel)
    {
        var skills = kernel.Skills.GetFunctionsView();
        var semantic = skills.SemanticFunctions;
        var native = skills.NativeFunctions;
        return semantic.Concat(native)
            .ToDictionary(x => x.Key, x => x.Value.AsEnumerable());
    }

    public static IEnumerable<string> GetRegisteredFunctions(this IKernel kernel)
        => GetFunctionViews(kernel).SelectMany(x => x.Value.Select(y => y.Name));

    public static IEnumerable<string> GetRegisteredFunctions(this IKernel kernel, string plugin)
        => GetFunctionViews(kernel)[plugin].Select(x => x.Name);

    public static IEnumerable<string> GetRegisteredPlugins(this IKernel kernel)
        => GetFunctionViews(kernel).Keys;
}
