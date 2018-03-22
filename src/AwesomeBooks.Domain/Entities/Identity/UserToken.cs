using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AwesomeBooks.Domain.Entities.Identity
{
    public class UserToken : IdentityUserToken<long>
    {
        [Timestamp]
        public byte[] Timestamp { get; set; }

        public string GetEntityName()
        {
            return nameof(RoleClaim);
        }
    }
}
