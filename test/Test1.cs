using AnotherNamespace;
namespace test;



[TestClass]
public sealed class Test1
{

    [TestMethod]
    public void TestMethod1()
    {
        /// Arrange
        AnotherClass anotherClass = new AnotherClass();

        /// Act
        bool result = anotherClass.IsAlive();
        /// Assert  
        
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void TestMethod2()
    {
        /// Arrange
        AnotherClass anotherClass = new AnotherClass();

        /// Act
        bool result = anotherClass.IsAlive();
        result = anotherClass.IsAlive();

        /// Assert
        Assert.IsTrue(result);
    }
}
