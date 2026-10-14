//using System.Security.Claims;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using TrainingCenter.Data;
//using TrainingCenter.DTOs.Auth;
//using TrainingCenter.Entities;
//using TrainingCenter.Services;
//[ApiController]
//[Route("api/auth")]
//public class OriginalBadAuthController : ControllerBase
//{
//    private readonly TrainingCenterDbContext _context;
//    [HttpPost("login")]
//    public IActionResult Login(LoginRequest request)
//    {
//        var user = _context.Users.FirstOrDefault(x => x.Email == request.Email);
//        if (user == null) return Ok("wrong email");
//        if (user.PasswordHash != request.Password) return Ok("wrong password");
//        var token = "fake-token-" + user.Id;
//        return Ok(new { token = token, user = user });
//    }
//    [HttpPost("register")]
//    public IActionResult Register(RegisterRequest request)
//    {
//        var user = new ApplicationUser();
//        user.FullName = request.FullName;
//        user.Email = request.Email;
//        user.PasswordHash = request.Password;
//        _context.Users.Add(user);
//        _context.SaveChanges();
//        return Ok(user);
//    }
//}