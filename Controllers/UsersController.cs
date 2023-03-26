using ChatalkApi.Dtos.Users;
using ChatalkApi.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ChatalkApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService userService;

    public UsersController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        try
        {
            var userInfo = await userService.Login(dto.Pseudo, dto.Password);
            if (userInfo == null)
            {
                throw new Exception("Incorrect pseudo or password.");
            }
            return Ok(userInfo);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignupDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        try
        {
            await userService.Signup(dto.Pseudo, dto.Password);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
