using AspnetCore.Jwt.Authentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            }

            return Error("Unexpected error!");
        }

        private string GetAccessToken(string email)
        {            
            return string.Empty;
        }

        private string GetIdToken(IdentityUser user)
        {
            return string.Empty;
        }

        private JsonResult Error(string message)
        {
            return new JsonResult(message) { StatusCode = 400 };
        }
    }
}
