using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NewsAppClasses;
using NewsAppClasses.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewsAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public AuthorizationController(IConfiguration configuration, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<TokenDTO>> Login(LogInDTO credentials)
        {
            //find user
            var user = await _userManager.FindByNameAsync(credentials.UserName);
            if (user == null)
            {
                return NotFound();
            }
            //check password
            var isAuthenitcated = await _userManager.CheckPasswordAsync(user, credentials.Password);
            if (!isAuthenitcated)
            {
                return Unauthorized();
            }

            //get claims to create token
            var claimsList = await _userManager.GetClaimsAsync(user);

            //1- key (must be in bytes) 
            var secretKeyString = _configuration.GetValue<string>("SecretKey") ?? string.Empty;
            var secretKeyInBytes = Encoding.ASCII.GetBytes(secretKeyString);
            //key can be symmetric or asymmetric
            var secretKey = new SymmetricSecurityKey(secretKeyInBytes);

            //Combination SecretKey, HashingAlgorithm
            var siginingCreedentials = new SigningCredentials(secretKey,
                SecurityAlgorithms.HmacSha256Signature);

            //add expiration as a claim
            var expiry = DateTime.Now.AddDays(1);

            var token = new JwtSecurityToken(
                claims: claimsList,
                expires: expiry,
                signingCredentials: siginingCreedentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new TokenDTO(tokenString, expiry));
        }


        [HttpPost]
        [Route("RegisterAdmin")]
        public async Task<ActionResult> RegisterAdmin(RegisterDTO credentials)
        {
            var AddUser = new User
            {
                UserName = credentials.UserName,
                Email = credentials.Email,
               
            };
            //create identity
            var result = await _userManager.CreateAsync(AddUser, credentials.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, AddUser.Id),
                new Claim(ClaimTypes.Role,"Admin" )
            };

            await _userManager.AddClaimsAsync(AddUser, claims);

            return NoContent();


        }

       
      
    
    }
}
