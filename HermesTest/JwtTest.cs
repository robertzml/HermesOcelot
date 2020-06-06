using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using JWT.Serializers;

namespace HermesTest
{
    public class JwtTest
    {
        #region Function
        public bool ValidateIdToken(string token)
        {
            try
            {
                const string key = "the shengdangjia hermes project";

                var json = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA384Algorithm()) // symmetric
                    .WithSecret(key)
                    .MustVerifySignature()
                    .Decode<IDictionary<string, object>>(token);

                Console.WriteLine(json);

                return true;
            }
            catch (TokenExpiredException)
            {
                Console.WriteLine("Token has expired");

                return false;
            }
            catch (SignatureVerificationException)
            {
                Console.WriteLine("Token has invalid signature");

                return false;
            }
        }
        #endregion //Function

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestCreateIdToken()
        {
            string uid = "zhangsan";
            const string key = "the shengdangjia hermes project";

            var token = new JwtBuilder()
                .WithAlgorithm(new HMACSHA384Algorithm())
                .WithSecret(key)
                .Subject("id token")
                .Issuer("auth")
                .ExpirationTime(DateTime.Now.AddHours(1))
                .AddClaim("uid", uid)
                .Encode();

            Console.WriteLine(token);

            Assert.Pass();
        }

        [Test]
        public void TestValidIdToken()
        {
            string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzM4NCJ9" +
                ".eyJzdWIiOiJpZCB0b2tlbiIsImlzcyI6ImF1dGgiLCJleHAiOjE1OTE0MzU2NTcuMCwidWlkIjoiemhhbmdzYW4ifQ" +
                ".2mPd-5U50gtGqMcqTreKQeiBIqapxnMpX9zUnE0nTH53u1GwsxXY5aTtjb9LjRhp";

            var result = ValidateIdToken(token);

            Assert.IsTrue(result);
        }
    }
}
