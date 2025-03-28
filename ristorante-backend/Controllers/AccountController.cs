﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ristorante_backend.Models;
using ristorante_backend.Services;

namespace ristorante_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtAuthenticationService _jwt;
        private readonly UserService _userService;

        public AccountController(JwtAuthenticationService jwt, UserService userService)
        {
            this._jwt = jwt;
            this._userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserModel user)
        {
            var result = await _userService.RegisterAsync(user);
            if (!result)
            {
                return BadRequest(new { Message = "Registrazione fallita!" });
            }

            return Ok(new { Message = "Registrazione avvenuta con successo!" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserModel user)
        {
            var token = await _jwt.Authenticate(user.Email, user.Password);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(new
            {
                Token = token,
                ExpirationUtc = DateTime.UtcNow.AddMinutes(_jwt._jwtSettings.DurationInMinutes)
            });
        }
    }
}