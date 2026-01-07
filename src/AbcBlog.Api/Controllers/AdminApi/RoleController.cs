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
    [Route("api/api/role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<AppRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Permissions.Roles.Create)]
        public async Task<IActionResult> CreateRole([FromBody] CreateUpdateRoleRequest request)
        {
            await _roleManager.CreateAsync(new AppRole()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                DisplayName = request.DisplayName,
            });

            return new OkResult();
        }

        [HttpPut("{id}")]
        [ValidateModel]
        [Authorize(Permissions.Roles.Edit)]
        public async Task<IActionResult> UpdateRole([FromBody] CreateUpdateRoleRequest request, Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return NotFound();
            }

            role.Name = request.Name;
            role.DisplayName = request.DisplayName;

            await _roleManager.UpdateAsync(role);

            return Ok();
        }

        [HttpDelete]
        [Authorize(Permissions.Roles.Delete)]
        public async Task<IActionResult> DeletePost([FromBody] Guid[] ids)
        {
            foreach (var id in ids)
            {
                var role = await _roleManager.FindByIdAsync(id.ToString());
                if (role == null)
                {
                    return NotFound();
                }
                await _roleManager.DeleteAsync(role);
            }

            return Ok();
        }

        [HttpGet("{id}")]
        [Authorize(Permissions.Roles.View)]
        public async Task<ActionResult<RoleDto>> GetRoleById(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);

        }

        [HttpGet]
        [Authorize(Permissions.Roles.View)]
        public async Task<ActionResult<PageResult<RoleDto>>> GetRolesPaging(string? keyword, int pageIndex, int pageSize = 10)
        {
            var query = _roleManager.Roles;
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword) || x.DisplayName.Contains(keyword));
            }

            var totelRow = query.Count();
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var data = await _mapper.ProjectTo<RoleDto>(query).ToListAsync();
            var paginationSet = new PageResult<RoleDto>
            {
                Results = data,
                CurrentPage = pageIndex,
                RowCount = totelRow,
                PageSize = pageSize
            };

            return Ok(paginationSet);
        }

        [HttpGet("all")]
        [Authorize(Permissions.Roles.View)]
        public async Task<ActionResult<List<RoleDto>>> GetAllRoles()
        {
            var model = await _mapper.ProjectTo<RoleDto>(_roleManager.Roles).ToListAsync();
            return Ok(model);
        }

    }
}
