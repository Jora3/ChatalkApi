using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChatalkApi.Dtos.Users;
using ChatalkApi.Helpers;
using ChatalkApi.Models;
using ChatalkApi.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ChatalkApi.Services.Concretes;

public class UserService : IUserService
{
    private readonly ChatalkDbContext dbContext;
    private readonly AppSettings appSettings;
    private readonly PasswordHasher<User> passwordHasher;

    public UserService(ChatalkDbContext dbContext, IOptions<AppSettings> appSettingsOptions)
    {
        this.dbContext = dbContext;
        this.appSettings = appSettingsOptions.Value;
        this.passwordHasher = new PasswordHasher<User>();
    }

    public string GenerateJwtToken(string pseudo)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("pseudo", pseudo)
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Secret)), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<User> GetByPseudo(string pseudo)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Pseudo.ToLower() == pseudo.ToLower());
    }

    public async Task<UserInfoDto> Login(string pseudo, string password)
    {
        var existing = await GetByPseudo(pseudo);
        if (existing == null)
        {
            return null;
        }

        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(existing, existing.Password, password);
        if (passwordVerificationResult != PasswordVerificationResult.Success)
        {
            return null;
        }

        return new UserInfoDto
        {
            Pseudo = existing.Pseudo,
            Token = GenerateJwtToken(existing.Pseudo)
        };
    }

    public async Task Signup(string pseudo, string password)
    {
        var existing = await GetByPseudo(pseudo);
        if (existing != null)
        {
            throw new Exception("User with same pseudo already exists.");
        }
        var user = new User
        {
            Pseudo = pseudo
        };
        user.Password = passwordHasher.HashPassword(user, password);
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }
}