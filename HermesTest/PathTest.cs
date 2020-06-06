using NUnit.Framework;
using System.Text.RegularExpressions;

namespace HermesTest
{
    public class PathTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestMatch()
        {
            string input = "/account-service/auth/login";

            var match = Regex.IsMatch(input, "/account-service/(register|auth).+");

            Assert.IsTrue(match);
        }
    }
}