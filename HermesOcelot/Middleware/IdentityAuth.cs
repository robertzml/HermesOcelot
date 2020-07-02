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
                var request = ctx.Request;
                var response = ctx.Response;

                // 跳过注册登录请求
                if (Regex.IsMatch(request.Path.Value, "/auth-service/(register|auth).+"))
                {
                    await next.Invoke();
                    return;
                }

                // 获取请求来源
                var referer = request.Headers["Referer"].ToString();
                Console.WriteLine(string.Format("request path: {0}; from: {1}", request.Path.Value, referer));

                // 检查是否有 Authorization header
                if (request.Headers["Authorization"].Count == 0)
                {
                    response.ContentType = "application/json";
                    response.StatusCode = 401;

                    var strResult = authFailed("no authorization header");

                    await response.WriteAsync(strResult);
                    return;
                }

                // 获取token
                JwtHelper jwtHelper = new JwtHelper();
                var token = request.Headers["Authorization"][0];

                // 验证id token
                var jwtState = jwtHelper.ValidateIdToken(token);

                if (jwtState.Success) // id token jwt 验证成功
                {
                    // 生成access token 替换 authorization header
                    var accessToken = jwtHelper.CreateAccessToken(jwtState.Uid);

                    Console.WriteLine(string.Format("valid id token succes, sub: {0}, uid: {1}, generate access token: {2}",
                        jwtState.Subject, jwtState.Uid, accessToken));

                    request.Headers.Remove("Authorization");
                    request.Headers.Add("Authorization", accessToken);

                    await next.Invoke();
                }
                else  // id token jwt 验证失败
                {
                    Console.WriteLine(string.Format("valid id token failed. error: {0}", jwtState.ErrorMessage));

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
