using System;
using System.Collections.Generic;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using JWT.Serializers;

namespace HermesOcelot
{
    /// <summary>
    /// JWT 工具类
    /// </summary>
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

        /// <summary>
        /// 验证id token
        /// </summary>
        /// <param name="token">id token</param>
        /// <returns></returns>
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

                // Console.WriteLine(json);

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
                Console.WriteLine("Token has invalid format");
                return false;
            }
        }

        /// <summary>
        /// 生成access token
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
        /// <remarks>
        /// 10分钟有效期
        /// </remarks>
        public string CreateAccessToken(string uid)
        {
            try
            {
                const string key = "the shengdangjia hermes project";

                var token = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA384Algorithm())
                    .WithSecret(key)
                    .Subject("access token")
                    .Issuer("auth")
                    .ExpirationTime(DateTime.Now.AddMinutes(10))
                    .AddClaim("uid", uid)
                    .Encode();

                return token;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
