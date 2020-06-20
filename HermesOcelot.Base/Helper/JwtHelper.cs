using System;
using System.Collections.Generic;
using System.Text;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using JWT.Serializers;

namespace HermesOcelot.Base
{
    /// <summary>
    /// JWT 工具类
    /// </summary>
    public class JwtHelper
    {
        #region Field
        /// <summary>
        /// JWT 密钥
        /// </summary>
        private const string key = "the shengdangjia hermes project";
        #endregion //Field

        #region Function
        public static DateTime TimeStampToDateTime(long timeStamp)
        {
            var dto = DateTimeOffset.FromUnixTimeSeconds(timeStamp);
            return dto.ToLocalTime().DateTime;
        }
        #endregion //Function

        #region Method
        /// <summary>
        /// 生成id token
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns>id token</returns>
        /// <remarks>
        /// 超时时间1小时
        /// </remarks>
        public string CreateIdToken(string uid)
        {
            try
            {
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
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// 验证id token
        /// </summary>
        /// <param name="token">id token</param>
        /// <returns></returns>
        public JwtState ValidateIdToken(string token)
        {
            try
            {
                var json = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA384Algorithm())
                    .WithSecret(key)
                    .MustVerifySignature()
                    .Decode<IDictionary<string, object>>(token);

                DateTime exp = TimeStampToDateTime(Convert.ToInt64(json["exp"]));
                return new JwtState
                {
                    Subject = json["sub"].ToString(),
                    Success = true,
                    IsExpire = false,
                    Uid = json["uid"].ToString(),
                    ExpireTime = exp
                };
            }
            catch (TokenExpiredException e)
            {
                return new JwtState
                {
                    Success = false,
                    IsExpire = true,
                    ExpireTime = e.Expiration.Value.ToLocalTime(),
                    ErrorMessage = "已超时"
                };
            }
            catch (SignatureVerificationException)
            {
                return new JwtState { Success = false, IsExpire = false, ErrorMessage = "签名验证失败" };
            }
            catch (Exception e)
            {
                return new JwtState { Success = false, IsExpire = false, ErrorMessage = e.Message }; ;
            }
        }

        /// <summary>
        /// 生成access token
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns>access token</returns>
        /// <remarks>
        /// 10分钟有效期
        /// </remarks>
        public string CreateAccessToken(string uid)
        {
            try
            {
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

        /// <summary>
        /// 验证access token
        /// </summary>
        /// <param name="token">access token</param>
        /// <returns></returns>
        public JwtState ValidateAccessToken(string token)
        {
            try
            {
                var json = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA384Algorithm())
                    .WithSecret(key)
                    .MustVerifySignature()
                    .Decode<IDictionary<string, object>>(token);

                DateTime exp = TimeStampToDateTime(Convert.ToInt64(json["exp"]));
                return new JwtState
                {
                    Subject = json["sub"].ToString(),
                    Success = true,
                    IsExpire = false,
                    Uid = json["uid"].ToString(),
                    ExpireTime = exp
                };
            }
            catch (TokenExpiredException e)
            {
                return new JwtState
                {
                    Success = false,
                    IsExpire = true,
                    ExpireTime = e.Expiration.Value.ToLocalTime(),
                    ErrorMessage = "已超时"
                };
            }
            catch (SignatureVerificationException)
            {
                return new JwtState { Success = false, IsExpire = false, ErrorMessage = "签名验证失败" };
            }
            catch (Exception e)
            {
                return new JwtState { Success = false, IsExpire = false, ErrorMessage = e.Message }; ;
            }
        }
        #endregion //Method
    }
}
