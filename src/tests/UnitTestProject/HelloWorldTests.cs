namespace UnitTestProject
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using HelloWorld;
    using NUnit.Framework;
    
    [TestClass]
    public class HelloWorldTests
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestFixtureSetUp]
        public void Initialize()
        {
        }

        /// <summary>
        /// Tests the clean up.
        /// </summary>
        [TestFixtureTearDown]
        public void Cleanup()
        {
        }

        [TestCase]
        public void HelloWorldMainTest()
        {            
            Program.Add(1, 2);
        }
    }
}
