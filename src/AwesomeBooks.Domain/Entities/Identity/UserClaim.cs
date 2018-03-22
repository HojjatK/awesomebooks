using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AwesomeBooks.Domain.Entities.Identity
{
    public class UserClaim : IdentityUserClaim<long>
    {
        [Timestamp]
        public byte[] Timestamp { get; set; }

        public string GetEntityName()
        {
            return nameof(RoleClaim);
        }
    }
}
