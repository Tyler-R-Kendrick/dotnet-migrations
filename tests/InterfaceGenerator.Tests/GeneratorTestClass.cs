namespace InterfaceGenerator.Tests;

[InterfaceGenerator(Scope.Public)]
public partial class GeneratorTestClass(int initialValue) 
{
    #pragma warning disable CA2211 // Non-constant fields should not be visible
    public static readonly GeneratorTestClass Instance = new(0);
    private int _testField = initialValue;
    #pragma warning restore CA2211 // Non-constant fields should not be visible
    public int this[int newValue] { get => TestProperty; set => TestMethod(newValue); }
    public int TestProperty { get; init; }
    public int TestProperty2 { get; private set; } = initialValue;
    public int TestProperty3 { get; } = initialValue;
    public int TestProperty4 { get; set; } = initialValue;
    public int TestProperty6 => _testField;
    public int TestProperty5 { get => _testField; set => _testField = value; }

    private EventHandler _testEvent = delegate { };
    public event EventHandler TestEvent
    {
        add => _testEvent += value;
        remove
        {
            if (value != null && _testEvent != null)
            {
#pragma warning disable CS8601 // Possible null reference assignment.
                _testEvent -= value;
#pragma warning restore CS8601 // Possible null reference assignment.
            }
        }
    }
    public event EventHandler TestEvent2 = delegate { };

    public void TestMethod(int value)
    {
        _testField = value;
    }
}


[InterfaceGenerator(Scope.Public)]
public partial class GeneratorTestClass2 : IGeneratorTestClass2
{
    public int MyProp { get; set; }
}

// [InterfaceGenerator(Scope.Public)]
// public class GeneratorTestClass3
// {
//     public int MyProp { get; set; }
// }