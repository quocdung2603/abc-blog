using AbcBlog.Core.Domain.Identity;
using AbcBlog.Core.Models.System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.Reflection;
using System.Security.Claims;

namespace AbcBlog.Api.Extensions
{
    public static class ClaimExtensions
    {
        public static void GetPermissions(this List<RoleClaimsDto> allPermissions, Type policy)
        {
            FieldInfo[] fields = policy.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                var attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                var displayName = field.GetValue(null).ToString();
                var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attribute.Length > 0)
                {
                    var description = (DescriptionAttribute)attribute[0];
                    displayName = description.Description;
                }
                allPermissions.Add(new RoleClaimsDto { Value = field.GetValue(null).ToString(), Type = "Permissions", DisplayName = displayName });
            }
        } 

        public static async Task AddPermissionClaim (this RoleManager<AppRole> roleManager, AppRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a => a.Type  == "Permission" && a.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }
    }
}
