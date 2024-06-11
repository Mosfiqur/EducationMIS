using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;

namespace UnicefEducationMIS.Data.Configurations.Identity
{
    class RoleDbConfigurations : IEntityTypeConfiguration<Core.Models.Identity.Role>
    {
        public void Configure(EntityTypeBuilder<Core.Models.Identity.Role> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasMaxLength(128);
            builder.Property(p => p.NormalizedName).HasMaxLength(128);
            builder.ToTable(TableNames.Roles);
        }
    }
}
