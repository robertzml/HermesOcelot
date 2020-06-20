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

            Console.WriteLine(token);

            var state = this.jwtHelper.ValidateIdToken(token);

            Assert.IsTrue(state.Success);

            Assert.AreEqual("id token", state.Subject);
        }

        /// <summary>
        /// 验证id token超时
        /// </summary>
        [Test(Description = "验证id token超时")]
        public void TestIdTokenExpire()
        {
            var token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzM4NCJ9.eyJzdWIiOiJpZCB0b2tlbiIsImlzcyI6ImF1dGgiLCJleHAiOjE1OTI2MzI5NjYuMCwidWlkIjoiemhhbmdzYW4ifQ.36dZqEHHZb8xTPHUELiMeGv_gkkXWHXU4aVkoEiEd-WfTGBt7hwQS2mGUSYMeeVx";

            var state = this.jwtHelper.ValidateIdToken(token);

            Assert.IsTrue(state.IsExpire);
        }

        /// <summary>
        /// 验证id token 失败
        /// </summary>
        [Test(Description = "验证id token 失败")]
        public void TestIdTokenError()
        {
            var token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzM4NCJ9.eyJzdWIiOiJpZCB0b2tlbiIsImlzcyI6ImF1dGgiLCJleHAiOjE1OTI2Mzc1NTQuMCwidWlkIjoiemhhbmdzYW4ifQ.Gg8B4pjN-I0XH-X-bYLuVsTr0fv46Ehvv9sERjDtRbJDT71QlSm-jhXuqTD-oILU";

            var state = this.jwtHelper.ValidateIdToken(token);

            Console.WriteLine(state.ErrorMessage);
            Assert.IsFalse(state.Success);
        }
        #endregion //Test
    }
}
