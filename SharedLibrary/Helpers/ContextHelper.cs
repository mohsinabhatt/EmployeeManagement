using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace SharedLibrary
{
    public static class ContextHelper
    {
        public static Guid? UserId
        {
            get
            {
                var  accessor = ServiceHelper.GetService<IHttpContextAccessor>();
                return Guid.TryParse(accessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == AppClaimTypes.UserId)?.Value, out Guid userId)
                ? (Guid?)userId
                : null;
            }
        }
    }
}
