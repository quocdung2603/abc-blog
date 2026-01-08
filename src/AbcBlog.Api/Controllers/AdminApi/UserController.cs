using AbcBlog.Api.Extensions;
using AbcBlog.Api.Filters;
using AbcBlog.Core.Domain.Identity;
using AbcBlog.Core.Models;
using AbcBlog.Core.Models.System;
using AbcBlog.Core.SeedWorks.Constants;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AbcBlog.Api.Controllers.AdminApi
{
    [Route("api/admin/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [Authorize(Permissions.Users.View)]
        public async Task<ActionResult<UserDto>> GetUserById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            var userDto = _mapper.Map<AppUser, UserDto>(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            userDto.Roles = userRoles;
            return Ok(userDto);
        }

        [HttpGet("paging")]
        [Authorize(Permissions.Users.View)]
        public async Task<ActionResult<PageResult<UserDto>>> GetAllUserPaging(string? keyword, int pageIndex, int pageSize = 10)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.FirstName.Contains(keyword)
                                    || x.LastName.Contains(keyword)
                                    || x.Email.Contains(keyword)
                                    || x.PhoneNumber.Contains(keyword));
            }

            var totalRow = await query.CountAsync();

            query = query.OrderByDescending(x => x.DateCreated)
               .Skip((pageIndex - 1) * pageSize)
               .Take(pageSize);

            var pagedResponse = new PageResult<UserDto>
            {
                Results = await _mapper.ProjectTo<UserDto>(query).ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return Ok(pagedResponse);
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Permissions.Users.Create)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var user = _mapper.Map<CreateUserRequest, AppUser>(request);
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(string.Join("<br>", result.Errors.Select(x => x.Description)));
        }

        [HttpPut("{id}")]
        [ValidateModel]
        [Authorize(Permissions.Users.Edit)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request, Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            _mapper.Map(request, user);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(string.Join("<br>", result.Errors.Select(x => x.Description)));
        }

        [HttpDelete]
        [Authorize(Permissions.Users.Delete)]
        public async Task<IActionResult> DeleteUsers([FromBody] Guid[] ids)
        {
            foreach (var id in ids)
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return NotFound();
                }
                await _userManager.DeleteAsync(user);
            }
            return Ok();
        }

        [HttpPut("password-change-current-user")]
        [ValidateModel]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(User.GetUserId().ToString());
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(string.Join("<br>", result.Errors.Select(x => x.Description)));
        }

        [HttpPost("set-password/{id}")]
        [Authorize(Permissions.Users.Edit)]
        public async Task<IActionResult> SetPasswordReset([FromBody] SetPasswordReset model, Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(string.Join("<br>", result.Errors.Select(x => x.Description)));
        }

        [HttpPost("change-email/{id}")]
        [Authorize(Permissions.Users.Edit)]
        public async Task<IActionResult> ChangeEmail(Guid id, [FromBody] ChangeEmailReset request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, request.Email);
            var result = await _userManager.ChangeEmailAsync(user, request.Email, token);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(string.Join("<br>", result.Errors.Select(x => x.Description)));
        }

        [HttpPut("{id}/assign-user")]
        [ValidateModel]
        [Authorize(Permissions.Users.Edit)]
        public async Task<IActionResult> AssignRoleToUser(string id, [FromBody] string[] roles)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removedResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            var addedResult = await _userManager.AddToRolesAsync(user, roles);

            if (!addedResult.Succeeded || !removedResult.Succeeded)
            {
                List<IdentityError> addedErrorList = addedResult.Errors.ToList();
                List<IdentityError> removedErrorList = removedResult.Errors.ToList();

                var errorList = new List<IdentityError>();
                errorList.AddRange(addedErrorList);
                errorList.AddRange(removedErrorList);

                return BadRequest(string.Join("<br/>", errorList.Select(x => x.Description)));
            }

            return Ok();
        }
    }
}
