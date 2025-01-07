using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Users;

namespace MoShaabn.CleanArch.Interfaces
{
    public class CustomCurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomCurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? Id => Guid.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : (Guid?)null;
        public string UserName => _httpContextAccessor.HttpContext?.User.Identity?.Name;

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

        public string? Name => _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        public string? SurName => _httpContextAccessor.HttpContext?.User?.FindFirstValue("surname");
        public string? PhoneNumber => _httpContextAccessor.HttpContext?.User?.FindFirstValue("phone_number");
        public bool PhoneNumberVerified => _httpContextAccessor.HttpContext?.User?.FindFirstValue("phone_verified") == "true";
        public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
        public bool EmailVerified => _httpContextAccessor.HttpContext?.User?.FindFirstValue("email_verified") == "true";
        public Guid? TenantId => Guid.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirstValue("tenant_id"), out var tenantId) ? tenantId : (Guid?)null;
        public string[] Roles => _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray() ?? Array.Empty<string>();


        public Claim? FindClaim(string claimType)
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(claimType);
        }

        public Claim[] FindClaims(string claimType)
        {
            return _httpContextAccessor.HttpContext?.User?.FindAll(claimType).ToArray() ?? Array.Empty<Claim>();
        }

        public Claim[] GetAllClaims()
        {
            return _httpContextAccessor.HttpContext?.User?.Claims.ToArray() ?? Array.Empty<Claim>();
        }

        public bool IsInRole(string roleName)
        {
            return _httpContextAccessor.HttpContext?.User?.IsInRole(roleName) ?? false;
        }
        // Implement other ICurrentUser methods as necessary
    }

}