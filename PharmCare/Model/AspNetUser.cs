﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PharmCare.Model
{
    [Index(nameof(NormalizedEmail), Name = "EmailIndex")]
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetUserTokens = new HashSet<AspNetUserToken>();
            Roles = new HashSet<AspNetRole>();
        }

        [Key]
        public string Id { get; set; } = null!;
        public string Discriminator { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CreatedBy { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(256)]
        public string? UserName { get; set; }
        [StringLength(256)]
        public string? NormalizedUserName { get; set; }
        [StringLength(256)]
        public string? Email { get; set; }
        [StringLength(256)]
        public string? NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PasswordHash { get; set; }
        public string? SecurityStamp { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        [InverseProperty(nameof(AspNetUserClaim.User))]
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        [InverseProperty(nameof(AspNetUserLogin.User))]
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        [InverseProperty(nameof(AspNetUserToken.User))]
        public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty(nameof(AspNetRole.Users))]
        public virtual ICollection<AspNetRole> Roles { get; set; }
    }
}
