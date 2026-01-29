namespace Examples.Tests;

public class DummyTest
{
    [Fact]
    public void When_ExecutedFromNonTargetEnvironment_Then_StillSucceeds()
    {
        Assert.True(true);
    }
}
