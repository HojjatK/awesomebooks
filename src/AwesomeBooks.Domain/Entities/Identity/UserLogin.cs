using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AwesomeBooks.Domain.Entities.Identity
{
    public class UserLogin : IdentityUserLogin<long>
    {
        [Timestamp]
        public byte[] Timestamp { get; set; }

        public string GetEntityName()
        {
            return nameof(RoleClaim);
        }
    }
}
