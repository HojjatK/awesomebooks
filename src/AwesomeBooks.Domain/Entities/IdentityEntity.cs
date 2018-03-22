﻿using System.ComponentModel.DataAnnotations;

namespace AwesomeBooks.Domain.Entities
{
    public interface IIdentityEntity
    {
        long Id { get; set; }
        byte[] Timestamp { get; set; }
        string GetEntityName();
    }

    public abstract class IdentityEntity : IIdentityEntity
    {
        /// <summary>
        /// Entity Database Identifier.
        /// </summary>
        /// <remarks>Entity databse identity is supposed to be generated by database automatically when entity is new.</remarks>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Concurrency Token
        /// </summary>
        [Timestamp]
        public byte[] Timestamp { get; set; }

        public abstract string GetEntityName();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var other = (IdentityEntity)obj;
            return Id == other.Id;
        }
    }
}
