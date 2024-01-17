namespace InterfaceGenerator;

[AttributeUsage(AttributeTargets.Class)]
public class InterfaceGeneratorAttribute(Scope scope) : Attribute
{
    public Scope Scope { get; } = scope;
}
