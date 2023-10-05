// using System.CommandLine;
// using System.CommandLine.Binding;
// using System.CommandLine.Parsing;

// namespace SKCLI;

// sealed class DynamicBinder(IEnumerable<string> propNames) : BinderBase<IDictionary<string, object?>>
// {
//     private readonly IDictionary<string, Argument> _props
//         = propNames.ToDictionary(x => x, x => (Argument)new Argument<string>(x));

//     protected override IDictionary<string, object?> GetBoundValue(BindingContext bindingContext)
//     {
//         var results = new Dictionary<string, object?>();
//         var parseResult = bindingContext.ParseResult;
//         foreach(var (key, prop) in _props)
//         {
//             var result = parseResult.GetValueForArgument(prop);
//             var value = bindingContext.ParseResult.Diagram();
//             Console.WriteLine($"{key}: {result}: {value}");
//             results.Add(key, result);
//         }
//         return results;
//     }
// }
