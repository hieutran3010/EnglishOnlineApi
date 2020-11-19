#region

using System.Threading.Tasks;
using AuthService.Firebase.Abstracts;
using AuthService.Firebase.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace EnglishZone.GraphQL.Controllers
{
    [Authorize]
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuth _auth;

        public AccountController(IAuth auth)
        {
            _auth = auth;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            var existedUser = await this._auth.GetUserByEmailAsync(user.Email);
            if (existedUser != null)
            {
                return this.BadRequest("EXISTED_EMAIL");
            }

            if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                var existedUserWithPhone = await this._auth.GetUserByPhoneAsync(user.PhoneNumber);
                if (existedUserWithPhone != null)
                {
                    return this.BadRequest("EXISTED_PHONE");
                }
            }
            
            var newUser = await _auth.RegisterAccountAsync(user);

            return Ok(newUser);
        }


        [HttpGet("{uid}")]
        public async Task<IActionResult> Get(string uid)
        {
            if (string.IsNullOrWhiteSpace(uid))
            {
                return this.BadRequest();
            }

            var user = await this._auth.GetUserAsync(uid);
            return this.Ok(user);
        }
    }
}