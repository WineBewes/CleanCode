using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using CleanCode.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CleanCodeTests
{
    [TestClass]
    public class UserNameGetterTests
    {
        [TestMethod]
        public void GetUserName_Returns_String()
        {
            var userNameGetter = new UserNameGetter();

            var userName = userNameGetter.GetUserName();

            Assert.IsFalse(string.IsNullOrEmpty(userName));
        }
    }
}
