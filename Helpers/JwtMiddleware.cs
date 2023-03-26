using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ChatalkApi.Dtos.Users;
using ChatalkApi.Services.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ChatalkApi.Helpers;

public class JwtMiddleware
{
    private readonly RequestDelegate next;
    private readonly AppSettings appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettingsOptions)
    {
        this.next = next;
        this.appSettings = appSettingsOptions.Value;
    }

    public async Task Invoke(HttpContext context, IUserService userService)
    {
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
        if (!string.IsNullOrEmpty(token))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationResult = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            });
            if (tokenValidationResult.IsValid)
            {
                var pseudo = (tokenValidationResult.SecurityToken as JwtSecurityToken).Claims.FirstOrDefault(c => c.Type == "pseudo").Value;
                var user = await userService.GetByPseudo(pseudo);
                if (user != null)
                {
                    context.Items["User"] = new UserInfoDto
                    {
                        Pseudo = user.Pseudo
                    };
                }
            }
        }
        await next(context);
    }
}