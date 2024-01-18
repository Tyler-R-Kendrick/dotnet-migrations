namespace InterfaceGenerator;

[AttributeUsage(AttributeTargets.Class)]
public class InterfaceGeneratorAttribute(Scope scope = Scope.Public) : Attribute
{
    public Scope Scope { get; } = scope;
}
