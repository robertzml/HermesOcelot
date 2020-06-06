﻿using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HermesOcelot
{
    public class IdentityAuth
    {
        public static async Task AuthIdToken(HttpContext ctx, System.Func<System.Threading.Tasks.Task> next)
        {
            var req = ctx.Request;

            if (Regex.IsMatch(req.Path.Value, "/account-service/(register|auth).+"))
            {
                await next.Invoke();
                return;
            }

            if (req.Headers["Authorization"].Count == 0)
            {
                var response = ctx.Response;
                response.ContentType = "application/json";
                response.StatusCode = 401;
                response.Headers["Answer"] = "no authorization header";

                var strResult = authFailed();

                await response.WriteAsync(strResult);
                return;
            }

            if (req.Headers["Authorization"][0] == "1")
            {
                var response = ctx.Response;
                response.ContentType = "application/json";
                response.StatusCode = 401;
                response.Headers["Answer"] = "miss";

                var strResult = authFailed();

                await response.WriteAsync(strResult);
            }
            else
                await next.Invoke();
        }

        private static string authFailed()
        {
            JwtHelper jwtHelper = new JwtHelper();
            var responseData = new ResponseData
            {
                ErrorCode = 1,
                Message = "Auth failed",
                Result = jwtHelper.CreateIdToken("abc")
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
