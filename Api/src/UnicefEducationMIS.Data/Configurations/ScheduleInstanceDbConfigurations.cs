using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Data.Configurations
{
    public class ScheduleInstanceDbConfigurations : IEntityTypeConfiguration<Instance>
    {
        public void Configure(EntityTypeBuilder<Instance> builder)
        {
            builder.ToTable(TableNames.ScheduleInstances);
            builder.Property(p => p.Title).HasMaxLength(256);            
            builder.Property(p => p.DateCreated)
            .HasColumnType("datetime");

            builder.Property(p => p.LastUpdated)
            .HasColumnType("datetime");
                       
        }
    }
}
