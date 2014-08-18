using CleanCode.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CleanCodeTests
{
    [TestClass]
    public class HelloWorldInserterTests
    {
        [TestMethod]
        public void InsertHelloWorld_Returns_String()
        {
            var userNameGetter = new Mock<IUserNameGetter>();

            var helloWorldInserter = new HelloWorldInserter(userNameGetter.Object);

            var result = helloWorldInserter.InsertHelloWorld();

            Assert.IsFalse(string.IsNullOrEmpty(result));

        }
    }
}
