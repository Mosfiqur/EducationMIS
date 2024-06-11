using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models.Framework;

namespace UnicefEducationMIS.Data.Configurations.Framework
{
    public class MonitoringFrameworkDbConfigurations : IEntityTypeConfiguration<MonitoringFramework>
    {
        public void Configure(EntityTypeBuilder<MonitoringFramework> builder)
        {
            builder.ToTable(TableNames.MonitoringFrameworks);
            
            builder.Property(p => p.DateCreated)
            .HasColumnType("datetime");

            builder.Property(p => p.LastUpdated)
            .HasColumnType("datetime");
                        
        }
    }


}
