using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BuyWithOffer
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly JwtConfig jwtConfig;
        private readonly byte[] secret;
        private readonly MailService mailService;
        IUnitOfWork unitOfWork;
        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IUnitOfWork unitOfWork, IOptionsMonitor<JwtConfig> jwtConfig, MailService mailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtConfig = jwtConfig.CurrentValue;
            this.secret = Encoding.ASCII.GetBytes(this.jwtConfig.Secret);
            this.mailService = mailService;
            this.unitOfWork = unitOfWork;
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel input)
        {
            if (ModelState.IsValid)
            {
                Log.Information($"{User.Identity?.Name}: User logged in => {input.Email}");
                var user = await userManager.FindByNameAsync(input.Email);
                if(user == null)
                {
                    ModelState.AddModelError("Login", "Lutfen email adresinizi kontrol ediniz!");
                    return BadRequest(ModelState);
                }
                var loginResult = await signInManager.PasswordSignInAsync(input.Email, input.Password, false, false);
                if (!loginResult.Succeeded)
                {
                    await userManager.AccessFailedAsync(user);
                    await unitOfWork.CompleteAsync();

                    // 3. hatali giriste hesap bloke oluyor ve sayac sifirlaniyor.
                    // o yuzden maili 2. hatali giristen sonra gonderdim.
                    if (user.AccessFailedCount >= 2)
                    {
                        // 2 kez hatali parola girilince hesap bloke maili olusturup gonderir.
                        var tempMail = mailService.createBlockedMail(user.Email).Result;
                        Email toSendMail = tempMail.Result;
                        await mailService.sendMail(toSendMail);

                    }
                    ModelState.AddModelError("Login", "Lutfen sifrenizi kontrol ediniz!");
                    return BadRequest(ModelState);
                }               
                return Ok(GetTokenResponse(user));
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("SignOut")]
        public new async Task<IActionResult> SignOut()
        {
            Log.Information($"{User.Identity?.Name}: User sign out");
            await signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePassword input)
        {
            Log.Information($"{User.Identity?.Name}: User changes password");
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();
                var response = await userManager.ChangePasswordAsync(user, input.OldPassword, input.NewPassword);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterViewModel input)
        {
            if (ModelState.IsValid)
            {
                Log.Information($"{User.Identity?.Name}: Register new user => {input.Email}");
                var newUser = new User
                {
                    UserName = input.Email,
                    Email = input.Email,
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    EmailConfirmed = true,
                    TwoFactorEnabled = false,
                    PhoneNumber = input.PhoneNumber
                };

                var registerUser = await userManager.CreateAsync(newUser, input.Password);
                if (registerUser.Succeeded)
                {
                    await signInManager.SignInAsync(newUser, isPersistent: false);
                    var Findeduser = await userManager.FindByNameAsync(newUser.UserName);

                    var tempMail = mailService.createWelcomeMail(newUser.Email).Result;
                    Email toSendMail = tempMail.Result;
                    await mailService.sendMail(toSendMail);

                    return Ok(GetTokenResponse(Findeduser));
                }
                AddErrors(registerUser);
            }
            return BadRequest(ModelState);
        }

        private Task<User> GetCurrentUserAsync()
        {
            return userManager.GetUserAsync(HttpContext.User);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("error", err.Description);
            }
        }
        private JwtTokenResult GetTokenResponse(User user)
        {
            var token = GenerateAccessToken(user);
            JwtTokenResult result = new JwtTokenResult
            {
                AccessToken = token,
                ExpireInSeconds = jwtConfig.AccessTokenExpiration * 60,   // as second
                UserId = user.Id
            };
            return result;
        }
        private string GenerateAccessToken(User account)
        {
            // Get claim value
            Claim[] claims = GetClaim(account);

            var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);

            var jwtToken = new JwtSecurityToken(
                jwtConfig.Issuer,
                shouldAddAudienceClaim ? jwtConfig.Audience : string.Empty,
                claims,
                expires: DateTime.Now.AddMinutes(jwtConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return accessToken;
        }
        private static Claim[] GetClaim(User account)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.UserName),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim("AccountId", account.Id.ToString()),
            };

            return claims;
        }
    }
}