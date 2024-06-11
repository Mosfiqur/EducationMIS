using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    public class PermissionDbConfigurations : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {

            builder.ToTable(TableNames.Permissions);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.PermissionName)
                .HasMaxLength(128);

            builder.Property(p => p.DateCreated)
                .HasColumnType("datetime");

            builder.Property(p => p.LastUpdated)
                .HasColumnType("datetime");

        }
    }
}
