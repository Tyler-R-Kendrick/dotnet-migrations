namespace InterfaceGenerator;

[AttributeUsage(AttributeTargets.Class)]
public class InterfaceGeneratorAttribute(
    Scope scope = Scope.Public,
    string? name = default) : Attribute
{
    public Scope Scope { get; } = scope;
    public string? Name { get; } = name;
}
