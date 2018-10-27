using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Model
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public string Sex { get; set; }
        public DateTime BirthDay { get; set; }

        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        public virtual ICollection<IdentityUserRole<Guid>> AppUserRoles { get; } = new List<IdentityUserRole<Guid>>();

        /// <summary>
        /// Navigation property for the claims this user possesses.
        /// </summary>
        public virtual ICollection<IdentityUserClaim<Guid>> AppUserClaims { get; } = new List<IdentityUserClaim<Guid>>();

        /// <summary>
        /// Navigation property for this users login accounts.
        /// </summary>
        public virtual ICollection<IdentityUserLogin<Guid>> AppUserLogins { get; } = new List<IdentityUserLogin<Guid>>();

        public virtual ICollection<IdentityUserToken<Guid>> AppUserTokens { get; } = new List<IdentityUserToken<Guid>>();

        public virtual ICollection<IdentityRoleClaim<Guid>> AppRoleClaims { get; } = new List<IdentityRoleClaim<Guid>>();

        public string RefreshTokenHash { get; set; }

    }
}
