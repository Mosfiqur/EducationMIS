using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;

namespace UnicefEducationMIS.Data.Configurations.Identity
{
    public class UserDbConfigurations : IEntityTypeConfiguration<Core.Models.Identity.User>
    {
        public void Configure(EntityTypeBuilder<Core.Models.Identity.User> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.UserName).HasMaxLength(128);
            builder.Property(p => p.NormalizedUserName).HasMaxLength(128);
            builder.Property(p => p.Email).HasMaxLength(128);
            builder.Property(p => p.NormalizedEmail).HasMaxLength(128);
            builder.ToTable(TableNames.Users);
        }
    }
}
