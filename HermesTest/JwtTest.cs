using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using HermesOcelot.Base;

namespace HermesTest
{
    public class JwtTest
    {
        #region Field
        JwtHelper jwtHelper;
        #endregion //Field
        

        [SetUp]
        public void Setup()
        {
            this.jwtHelper = new JwtHelper();
        }

        #region Test
        /// <summary>
        /// 创建id token
        /// </summary>
        [Test(Description = "创建id token")]
        public void TestCreateIdToken()
        {
            string uid = "zhangsan";

            var token = this.jwtHelper.CreateIdToken(uid);

            Console.WriteLine(token);

            Assert.Pass();
        }

        /// <summary>
        /// 验证id token
        /// </summary>
        [Test(Description = "验证id token")]
        public void TestValidIdToken()
        {
            string uid = "zhangsan";
            var token = this.jwtHelper.CreateIdToken(uid);

            var state = this.jwtHelper.ValidateIdToken(token);

            Assert.IsTrue(state.Success);
        }
        #endregion //Test
    }
}
