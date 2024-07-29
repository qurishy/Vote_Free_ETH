using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Voter_Data_API.DTO;
using Voter_Data_API.MODELS;

namespace Voter_Data_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AuthenticationController( UserManager<ApplicationUsers> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }


        [HttpPost]
        [Route("roles/add")]
        public async Task<IActionResult> Createrole([FromBody] CreaRoleRequestDto request)
        {
            var appRole = new ApplicationRole
            {
                Name = request.Role
            };
            var createRole = await _roleManager.CreateAsync(appRole);

            return Ok(createRole);
        }
     

















        //---------------------------------------------------------------------------------------------------
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var result= await RegisterAsync(request);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        private async Task<registerResponsDto> RegisterAsync(RegisterRequestDto request)
        {
            try
            {
                var userExists= await _userManager.FindByEmailAsync(request.Email);
                if (userExists != null) return new registerResponsDto { message="Alreadey exist email", IsSuccess = false };

                // if we get here there is no uer with this email

                userExists = new ApplicationUsers
                {
                    FullName = request.UserName,
                    Email = request.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName= request.Email
                };

                var createuserresult = await _userManager.CreateAsync(userExists, request.Password);

                if (!createuserresult.Succeeded) return new registerResponsDto { message="create user failed", IsSuccess = false };


                //user is created 
                //then add user ro role

                var addusertorole = await _userManager.AddToRoleAsync(userExists, "User");
                if(!addusertorole.Succeeded) return new registerResponsDto {
                    message="user created succesfully but couldnot add user to role", IsSuccess = true };
                //all is still wil

                return new registerResponsDto
                {
                    message="user created succesfully",
                    IsSuccess = true

                };


            }
            catch (Exception ex) {
                return new registerResponsDto
                {
                    message = ex.Message,
                    IsSuccess = false
                };
            }


        }









        /// <summary>
        /// ///////////////////////////////////////////
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType( (int)HttpStatusCode.OK, Type= typeof(LoginResponsDto))]

        public async Task< IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result  = await LoginAsync(request);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        private async Task<LoginResponsDto> LoginAsync(LoginRequestDto request)
        {
            try
            {


                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null) return new LoginResponsDto { message="invalid email", IsSuccess = false };


                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
                };
                var role = await _userManager.GetRolesAsync(user);
                var roleclaims = role.Select(r => new Claim(ClaimTypes.Role, r));
                claims.AddRange(roleclaims);
                var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes("S3cr3tK3y"));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expire = DateTime.Now.AddMinutes(30);

                var token = new JwtSecurityToken
                (
                    issuer: "https://localhost:7112",
                    audience: "https://localhost:7112",
                    claims: claims,
                    expires: expire,
                    signingCredentials: creds
                );
                return new LoginResponsDto
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    email = user.Email,
                    message = "Success  ",
                    Userid = user.Id.ToString()
                };

            }
            catch (Exception ex)
            {
                return new LoginResponsDto
                {
                    message = ex.Message,
                    IsSuccess = false
                };
            }
        }
    }
}
