using System.Security.Claims;

namespace coderepo_api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID claim not found in token.");

            return int.Parse(userIdClaim.Value);
        }
    }
}
