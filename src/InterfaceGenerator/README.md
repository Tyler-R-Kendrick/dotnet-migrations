# Interface Generator

Generates interfaces for classes.

## How to use this generator

Use the ```InterfaceIncrementalGeneratorAttribute``` on a class marked as partial.

```csharp
[InterfaceIncrementalGenerator(Scope.Public)]
public partial class MyClass
{
    public int MyProperty { get; set; }
    public void MyMethod()
    {
        ...
    }
}
```

You may avoid the partial declaration by making the class implement the interface.
The interface will prefix the name of the class with a capital "I".

```csharp
[InterfaceIncrementalGenerator(Scope.Public)]
public class MyClass : IMyClass
{
    public int MyProperty { get; set; }
    public void MyMethod()
    {
        ...
    }
}
```
