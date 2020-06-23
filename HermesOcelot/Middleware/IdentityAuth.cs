using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HermesOcelot.Base;

namespace HermesOcelot
{
    /// <summary>
    /// 请求认证类
    /// </summary>
    public class IdentityAuth
    {
        #region Function
        /// <summary>
        /// 认证失败返回
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns></returns>
        private static string authFailed(string message)
        {
            var responseData = new ResponseData
            {
                ErrorCode = 1,
                Message = message,
                Result = ""
            };

            var serializeOptions = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var strResult = JsonSerializer.Serialize(responseData, serializeOptions);

            return strResult;
        }
        #endregion //Function

        #region Method
        /// <summary>
        /// 认证id token
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public static async Task AuthIdToken(HttpContext ctx, System.Func<System.Threading.Tasks.Task> next)
        {
            try
            {
                var req = ctx.Request;
                var response = ctx.Response;

                // 跳过注册登录请求
                if (Regex.IsMatch(req.Path.Value, "/auth-service/(register|auth).+"))
                {
                    await next.Invoke();
                    return;
                }

                // 检查是否有 Authorization header
                if (req.Headers["Authorization"].Count == 0)
                {
                    response.ContentType = "application/json";
                    response.StatusCode = 401;

                    var strResult = authFailed("no authorization header");

                    await response.WriteAsync(strResult);
                    return;
                }

                // 获取token
                JwtHelper jwtHelper = new JwtHelper();
                var token = req.Headers["Authorization"][0];

                // 验证id token
                var jwtState = jwtHelper.ValidateIdToken(token);

                if (jwtState.Success) // id token jwt 验证成功
                {
                    Console.WriteLine(string.Format("id token sub: {0}", jwtState.Subject));

                    // 生成access token 替换 authorization header
                    var accessToken = jwtHelper.CreateAccessToken(jwtState.Uid);
                    Console.WriteLine(string.Format("access token: {0}", accessToken));

                    req.Headers.Remove("Authorization");
                    req.Headers.Add("Authorization", accessToken);

                    await next.Invoke();
                }
                else  // id token jwt 验证失败
                {
                    response.ContentType = "application/json";
                    response.StatusCode = 401;

                    var strResult = authFailed(jwtState.ErrorMessage);

                    await response.WriteAsync(strResult);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                await next.Invoke();
            }
        }
        #endregion //Method
    }
}
