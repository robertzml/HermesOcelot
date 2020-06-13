using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HermesOcelot
{
    /// <summary>
    /// 请求认证类
    /// </summary>
    public class IdentityAuth
    {
        public static async Task AuthIdToken(HttpContext ctx, System.Func<System.Threading.Tasks.Task> next)
        {
            var req = ctx.Request;

            if (Regex.IsMatch(req.Path.Value, "/auth-service/(register|auth).+"))
            {
                await next.Invoke();
                return;
            }

            if (req.Headers["Authorization"].Count == 0)
            {
                var response = ctx.Response;
                response.ContentType = "application/json";
                response.StatusCode = 401;
                // response.Headers["Answer"] = "no authorization header";

                var strResult = authFailed("no authorization header");

                await response.WriteAsync(strResult);
                return;
            }

            var token = req.Headers["Authorization"][0];

            JwtHelper jwtHelper = new JwtHelper();
            var valid = jwtHelper.ValidateIdToken(token);
            if (valid)
            {
                try
                {
                    var accessToken = jwtHelper.CreateAccessToken("");
                    Console.WriteLine(accessToken);
                    // req.Headers.Add("Access Token", accessToken);
                    req.Headers.Remove("Authorization");

                    req.Headers.Add("Authorization", accessToken);
                    
                    await next.Invoke();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    await next.Invoke();
                }
            }
            else
            {
                var response = ctx.Response;
                response.ContentType = "application/json";
                response.StatusCode = 401;
                // response.Headers["Answer"] = "authorization failed";

                var strResult = authFailed("authorization failed");

                await response.WriteAsync(strResult);
            }
        }

        private static string authFailed(string message)
        {
            JwtHelper jwtHelper = new JwtHelper();
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
    }
}
