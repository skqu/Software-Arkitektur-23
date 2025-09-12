namespace test;

using AnotherNamespace;

[TestClass]
public sealed class AnotherClassTest
{
    [TestMethod]
    public void TestMethod1()
    {
        // Arrange
        AnotherClass anotherClass = new AnotherClass();

        // Act
        bool result = anotherClass.IsAlive();

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void testChangeAlive()
    {
        // Arrange
        AnotherClass anotherClass = new AnotherClass();
        anotherClass.changeAlive(false);

        // Act
        bool act = anotherClass.IsAlive();

        // Assert
        Assert.IsTrue(act);
    }

}
