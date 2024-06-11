using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models.Identity;

namespace UnicefEducationMIS.Data.Configurations.Identity
{
    public class UserLoginDbConfigurations : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.HasKey(x => x.UserId);            
            builder.Property(p => p.ProviderKey).HasMaxLength(128);
            builder.Property(p => p.LoginProvider).HasMaxLength(128);
            builder.ToTable(TableNames.UserLogins);
        }
    }
}
