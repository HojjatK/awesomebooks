using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AwesomeBooks.Domain.Entities.Identity
{
    public class User : IdentityUser<long>, IAggregateRoot
    {
        public Guid Uid { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public virtual ICollection<UserRole> Roles { get; } = new List<UserRole>();

        public virtual ICollection<UserClaim> Claims { get; } = new List<UserClaim>();

        public virtual ICollection<UserLogin> Logins { get; } = new List<UserLogin>();

        public virtual ICollection<UserToken> Tokens { get; } = new List<UserToken>();

        public string GetEntityName()
        {
            return nameof(Role);
        }
    }
}
