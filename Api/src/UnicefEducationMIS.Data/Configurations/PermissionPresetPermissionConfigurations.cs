using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    public class PermissionPresetPermissionConfigurations : IEntityTypeConfiguration<PermissionPresetPermission>
    {
        public void Configure(EntityTypeBuilder<PermissionPresetPermission> builder)
        {
            builder.HasKey(x => new { x.PermissionId, x.PermissionPresetId });
            builder.ToTable(TableNames.PermissionPresetPermission);
        }
    }
}
