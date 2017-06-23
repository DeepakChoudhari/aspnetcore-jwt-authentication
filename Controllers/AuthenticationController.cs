using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspnetCore.Jwt.Authentication.Models;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AspnetCore.Jwt.Authentication.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly JWTSettings _options;

        public AuthenticationController(UserManager<IdentityUser> userManager, 
                                        SignInManager<IdentityUser> signInManager,
                                        IOptions<JWTSettings> optionsAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _options = optionsAccessor.Value;
            
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] Credentials credentials)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = credentials.Email, Email = credentials.Email };

                var result = await _userManager.CreateAsync(user, credentials.Password);
                
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return new JsonResult(
                        new Dictionary<string, object>
                        {
                            { "access_token", GetAccessToken(credentials.Email) },
                            { "id_token", GetIdToken(user) }
                        }
                    );
                }

                return Errors(result);
            }

            return Error("Unexpected error!");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SignIn([FromBody] Credentials credentials)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(credentials.Email,
                                                    credentials.Password, false, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(credentials.Email);
                    return new JsonResult(new Dictionary<string, object>
                    {
                        { "access_token", GetAccessToken(credentials.Email) },
                        { "id_token", GetIdToken(user) }
                    });
                }

                return new JsonResult("Unable to sign in") { StatusCode = 401 };
            }
            return Error("Unexpected error");
        }

        private string GetAccessToken(string email)
        {
            var payload = new Dictionary<string, object>
            {
                { "sub", email },
                { "email", email }
            };
            return GetToken(payload);
        }

        private string GetToken(IDictionary<string, object> payload)
        {
            var secret = _options.SecretKey;

            payload.Add("iss", _options.Issuer);
            payload.Add("aud", _options.Audience);
            payload.Add("nbf", ConvertToUnixTimestamp(DateTime.Now));
            payload.Add("iat", ConvertToUnixTimestamp(DateTime.Now));
            payload.Add("exp", ConvertToUnixTimestamp(DateTime.Now.AddMinutes(2)));

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder jwtEncoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return jwtEncoder.Encode(payload, secret);
        }

        private static double ConvertToUnixTimestamp(DateTime date)
        {
            var originDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - originDate;
            return Math.Floor(diff.TotalMilliseconds);
        }

        private string GetIdToken(IdentityUser user)
        {
            var payload = new Dictionary<string, object>
            {
                { "id", user.Id },
                { "sub", user.Email },
                { "email", user.Email },
                { "emailConfirmed", user.EmailConfirmed }
            };

            return GetToken(payload);
        }

        private JsonResult Error(string message)
        {
            return new JsonResult(message) { StatusCode = 400 };
        }

        private JsonResult Errors(IdentityResult result)
        {
            var items = result.Errors
                        .Select(error => error.Description)
                        .ToArray();
            return new JsonResult(items) { StatusCode = 400 };
        }
    }
}
