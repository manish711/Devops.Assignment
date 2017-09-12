namespace UnitTestProject
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using HelloWorld;

    [TestClass]
    public class HelloWorldTests
    {
        [TestMethod]
        public void HelloWorldMainTest()
        {
            Program.Add(1, 2);
        }
    }
}
