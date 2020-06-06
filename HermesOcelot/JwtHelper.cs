using System;
using System.Collections.Generic;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using JWT.Serializers;

namespace HermesOcelot
{
    public class JwtHelper
    {
        /// <summary>
        /// 生成id token
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns>id token</returns>
        public string CreateIdToken(string uid)
        {
            try
            {
                const string key = "the shengdangjia hermes project";
               
                var token = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA384Algorithm())
                    .WithSecret(key)
                    .Subject("id token")
                    .Issuer("auth")
                    .ExpirationTime(DateTime.Now.AddHours(1))
                    .AddClaim("uid", uid)
                    .Encode();

                return token;
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }



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
    }
}
