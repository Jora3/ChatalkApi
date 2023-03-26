using ChatalkApi.Dtos.Users;
using ChatalkApi.Models;

namespace ChatalkApi.Services.Contracts;

public interface IUserService
{
    string GenerateJwtToken(string pseudo);
    Task<User> GetByPseudo(string pseudo);
    Task Signup(string pseudo, string password);
    Task<UserInfoDto> Login(string pseudo, string password);
}
