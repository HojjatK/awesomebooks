using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AwesomeBooks.Domain.Entities.Identity
{
    public class Role : IdentityRole<long>, IAggregateRoot
    {
        public Guid Uid { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public virtual ICollection<RoleClaim> Claims { get; } = new List<RoleClaim>();

        public string GetEntityName()
        {
            return nameof(Role);
        }
    }
}
