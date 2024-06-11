using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;

namespace UnicefEducationMIS.Data.Configurations.Identity
{
    public class UserClaimDbConfigurations : IEntityTypeConfiguration<Core.Models.Identity.UserClaim>
    {
        public void Configure(EntityTypeBuilder<Core.Models.Identity.UserClaim> builder)
        {
            builder.HasKey(p => p.Id);
            builder.ToTable(TableNames.UserClaims);
        }
    }
}
