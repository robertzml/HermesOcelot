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
            catch(Exception)
            {
                Console.WriteLine("Token format is wrong.");
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
            //string token = "1eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzM4NCJ9." +
            //    "eyJzdWIiOiJpZCB0b2tlbiIsInVpZCI6ImNjNWM2YjZlLTQxMTEtNGEyZC05YTQ1LTBiNjFmMWI0NDczNyIsImlzcyI6ImF1dGgiLCJleHAiOjE1OTIwMzE0Mjl9.Ww9SnTyb-" +
            //    "oQIEm_6hvqvS2h4mFNDQV1ooj0-0Qedyc4qoZN9YlnbXxzlNWmDEhF";

            string token = "123";

            var result = ValidateIdToken(token);

            Assert.IsFalse(result);
        }
    }
}
