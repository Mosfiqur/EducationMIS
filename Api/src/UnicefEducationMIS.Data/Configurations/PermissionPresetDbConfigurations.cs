using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    public class PermissionPresetDbConfigurations : IEntityTypeConfiguration<PermissionPreset>
    {
        public void Configure(EntityTypeBuilder<PermissionPreset> builder)
        {            
            builder.HasKey(p => p.Id);

            


            builder.ToTable(TableNames.PermissionPresets);
        }
    }
}
