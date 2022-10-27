﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PharmCare.Model
{
    [Index(nameof(UserId), Name = "IX_AspNetUserClaims_UserId")]
    public partial class AspNetUserClaim
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string? ClaimType { get; set; }
        public string? ClaimValue { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(AspNetUser.AspNetUserClaims))]
        public virtual AspNetUser User { get; set; } = null!;
    }
}
