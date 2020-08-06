#region

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Firebase.Abstracts;
using AuthService.Firebase.Contracts;
using GraphQLDoorNet.Abstracts;
using HelenExpress.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#endregion

namespace HelenExpress.GraphQL.Controllers
{
    [Authorize]
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuth _auth;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IAuth auth, IUnitOfWork unitOfWork)
        {
            _auth = auth;
            _unitOfWork = unitOfWork;
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

            var userRoleRepository = _unitOfWork.GetRepository<UserRole>();
            await userRoleRepository.AddAsync(new UserRole
            {
                UserId = newUser.Id,
                Role = string.Join('|', user.Roles)
            });

            await _unitOfWork.SaveChangesAsync();

            return Ok(newUser);
        }

        [HttpGet]
        public async Task<List<User>> Get([FromQuery] string[] roles)
        {
            if (roles == null || !roles.Any()) 
            {
                return await _auth.GetUsersAsync();
            }

            var userRoleRepository = _unitOfWork.GetRepository<UserRole>();
            var userIds = await userRoleRepository.GetQueryable().Where(ur => roles.Contains(ur.Role))
                .Select(ur => ur.UserId).ToArrayAsync();

            return await _auth.GetUsersByIds(userIds);
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

        [HttpPatch("{uid}")]
        public async Task<IActionResult> Patch(string uid, [FromBody] User user)
        {
            await this._auth.UpdateUserAsync(uid, user);
            var userRoleRepo = this._unitOfWork.GetRepository<UserRole>();
            var userRole = await userRoleRepo.GetQueryable().FirstOrDefaultAsync(ur => ur.UserId == user.Id);
            if (userRole != null)
            {
                userRole.Role = string.Join('|', user.Roles);
                userRoleRepo.Update(userRole);
                await this._unitOfWork.SaveChangesAsync();
            }
            return this.Ok();
        }
    }
}