//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Security.Claims;
//using System.Text.Encodings.Web;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.WebUtilities;
//using Microsoft.Extensions.Logging;
//using Bluebird.Core.Starter.Domain.Contracts.Integrations;

//namespace Bluebird.Core.Starter.Controllers.V1
//{
//    [ApiController]
//    [Produces("application/json")]
//    [Route("[controller]")]
//    public class AccountController : ControllerBase
//    {
//        private readonly ILogger<AccountController> _logger;
//        private readonly IAuthenticationTokenIntegration _jwtService;
//        // private readonly RoleManager<Role> _roleManager;
//        private readonly UserManager<IdentityUser> _userManager;
//        private readonly SignInManager<IdentityUser> _signInManager;

//        public AccountController(ILogger<AccountController> logger, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IAuthenticationTokenIntegration jwtService)
//        {
//            _logger = logger;
//            _jwtService = jwtService;
//            _userManager = userManager;
//            _signInManager = signInManager;
//        }

//        /// <summary>
//        /// Logs a user into the system and supplies the user with a JWT. See https://jwt.io
//        /// </summary>
//        [HttpPost]
//        [Route("Login")]
//        public async Task<ActionResult<AuthenticationTokenResult>> Login(string userName, string password)
//        {
//            // This does not count login failures towards account lockout
//            // To enable password failures to trigger account lockout,
//            // set lockoutOnFailure: true
//            var result = await _signInManager.PasswordSignInAsync(userName, password, false, lockoutOnFailure: false);
//            if (!result.Succeeded)
//                return Unauthorized();

//            _logger.LogInformation("User logged in.");

//            var user = await _userManager.FindByNameAsync(userName);
//            var claims = new List<Claim>() { new Claim("name", userName), new Claim("userid", user.Id) };
//            var tokens = _jwtService.GenerateTokens(userName, claims.ToArray(), DateTime.Now);
//            return Ok(tokens);
//        }

//        /// <summary>
//        /// Registers new user
//        /// </summary>
//        [Authorize]
//        [HttpPost]
//        [Route("Register")]
//        public async Task<IActionResult> Register(string username, string email)
//        {
//            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
//                return BadRequest("All fields are required (username and email)");
//            var user = new IdentityUser { UserName = username, Email = email };
//            var result = await _userManager.CreateAsync(user, Utils.GenerateRandomPassword()); // Create temp password
//            if (!result.Succeeded)
//                return BadRequest(result.Errors);
//            _logger.LogInformation("User created a new account with password.");
//            return Ok();
//        }

//        /// <summary>
//        /// Emails an OTP to user
//        /// </summary>
//        [Authorize]
//        [HttpPost]
//        [Route("EmailOTP")]
//        public async Task<IActionResult> EmailOTP(string username)
//        {
//            if (string.IsNullOrWhiteSpace(username))
//                return BadRequest("All fields are required");
//            var user = await _userManager.FindByNameAsync(username);
//            if(user == null)
//                return BadRequest("User does not exist");
//            var code = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
//            var email = System.IO.File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EmailTemplates", "RegistrationEmail.html"));
//            email = email.Replace("%0", username);
//            email = email.Replace("%1", code);
//            await _emailService.SendEmailAsync(user.Email, null, "Register your user", email);
//            return Ok();
//        }

//        /// <summary>
//        /// Confirms an OTP
//        /// </summary>
//        [Authorize]
//        [HttpPost]
//        [Route("ConfirmOTP")]
//        public async Task<IActionResult> ConfirmOTP(string username, string token)
//        {
//            if (string.IsNullOrWhiteSpace(username))
//                return BadRequest("All fields are required");
//            var user = await _userManager.FindByNameAsync(username);
//            if (user == null)
//                return BadRequest("User does not exist");
//            var isVerified = await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, token);
//            if (!isVerified)
//                return Unauthorized();
//            return Ok();
//        }


//        /// <summary>
//        /// Emails a user with reset password
//        /// </summary>
//        [HttpPost]
//        [Route("EmailReset")]
//        [AllowAnonymous]
//        public async Task<IActionResult> EmailReset(string emailAddress, string callbackUrlRoute)
//        {
//            if (string.IsNullOrWhiteSpace(emailAddress) || string.IsNullOrWhiteSpace(callbackUrlRoute))
//                return BadRequest("All fields are required (username and callbackUrlRoute)");

//            var user = await _userManager.FindByEmailAsync(emailAddress);
//            if (user == null)
//                return Forbid();
//            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
//            var callbackUrl = QueryHelpers.AddQueryString(callbackUrlRoute, new Dictionary<string, string> { { "userId", user.Id }, { "code", code } });
//            var email = System.IO.File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EmailTemplates", "RegistrationEmail.html"));
//            email = email.Replace("%0", user.UserName);
//            email = email.Replace("%1", HtmlEncoder.Default.Encode(callbackUrl));
//            await _emailService.SendEmailAsync(user.Email, null, "Register your user", email);
//            return Ok();
//        }

//        /// <summary>
//        /// Reset Password
//        /// </summary>
//        [HttpPost]
//        [Route("ConfirmReset")]
//        [AllowAnonymous]
//        public async Task<IActionResult> ConfirmReset(string userId, string code, string password)
//        {
//            if (userId == null || code == null)
//                return BadRequest();

//            var user = await _userManager.FindByIdAsync(userId);
//            if (user == null)
//                return Forbid();
//            var resultChangePassword = await _userManager.ResetPasswordAsync(user, code, password);
//            if (!resultChangePassword.Succeeded)
//                return BadRequest(resultChangePassword.Errors);

//            var userUpdateResult = await _userManager.UpdateAsync(user);
//            if (!userUpdateResult.Succeeded)
//                return BadRequest(userUpdateResult.Errors);

//            return Ok(userUpdateResult);
//        }

//        /// <summary>
//        /// Refreshes an Access Token
//        /// </summary>
//        [HttpPost]
//        [Route("RefreshToken")]
//        public async Task<IActionResult> Refresh(string refreshToken, string accessToken)
//        {
//            var tokens = _jwtService.Refresh(refreshToken, accessToken, DateTime.Now);
//            return Ok(tokens);
//        }

        

//    }
//}
