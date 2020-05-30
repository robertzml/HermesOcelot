using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;

namespace HermesOcelot
{
    public class IdentityAuth
    {
        public static async Task AuthIdToken(HttpContext ctx, System.Func<System.Threading.Tasks.Task> next)
        {
            var req = ctx.Request;

            if (req.Headers["Authorization"][0] == "1")
            {
                var response = ctx.Response;
                response.ContentType = "application/json";
                response.StatusCode = 403;
                response.Headers["Answer"] = "miss";

                var strResult = "some miss";
                await ctx.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(strResult));

                //var error = new UnauthenticatedError("Request for authenticated route was unauthenticated");
                //ctx.Errors.Add(error);
            }
            else
                await next.Invoke();
        }
    }
}
