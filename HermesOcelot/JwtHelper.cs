using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace HermesOcelot
{
    public class JwtHelper
    {
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
    }
}
