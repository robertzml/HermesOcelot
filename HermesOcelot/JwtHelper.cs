using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("the shengdangjia hermes project"));
                var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha384);

                var claims = new Claim[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, "id token"),
                new Claim("uid", uid)
                };

                var token = new JwtSecurityToken(
                    issuer: "auth",
                    expires: DateTime.Now.AddHours(1),
                    claims: claims,
                    signingCredentials: sign
                    );

                var result = new JwtSecurityTokenHandler().WriteToken(token);
                return result;
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }

        //public bool ValidateIdToken(string token)
        //{
        //    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("the shengdangjia hermes project"));
        //    var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha384);

        //    var handler = new JwtSecurityTokenHandler();
        //    var validationParameters = new TokenValidationParameters()
        //    {
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = sign.Key,

        //        ValidateIssuer = true,
        //        ValidIssuer = "auth"
        //    };

        //    handler.ValidateToken(token, validationParameters)
        //}
    }
}
