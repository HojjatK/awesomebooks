﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AwesomeBooks.Domain.Entities.Identity
{
    public class RoleClaim : IdentityRoleClaim<long>
    {
        [Timestamp]
        public byte[] Timestamp { get; set; }

        public string GetEntityName()
        {
            return nameof(RoleClaim);
        }
    }
}
