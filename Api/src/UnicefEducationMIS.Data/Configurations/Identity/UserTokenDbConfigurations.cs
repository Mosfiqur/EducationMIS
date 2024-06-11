using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;

namespace UnicefEducationMIS.Data.Configurations.Identity
{
    public class UserTokenDbConfigurations : IEntityTypeConfiguration<Core.Models.Identity.UserToken>
    {
        public void Configure(EntityTypeBuilder<Core.Models.Identity.UserToken> builder)
        {
            // Composite key
            builder.HasKey(p => new { p.UserId, p.LoginProvider, p.Name });            
            builder.Property(p => p.LoginProvider).HasMaxLength(128);
            builder.Property(p => p.Name).HasMaxLength(128);
            builder.ToTable(TableNames.UserTokens);
        }
    }
}
