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
    public class ScheduleDbConfigurations : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.ToTable(TableNames.Schedules);

            builder.Property(p => p.ScheduleName).HasMaxLength(256);
            builder.Property(p => p.DateCreated)
            .HasColumnType("datetime");

            builder.Property(p => p.LastUpdated)
            .HasColumnType("datetime");

            builder.Property(p => p.ScheduleFor)
                .HasConversion<int>();


            builder.HasOne<Frequency>(p => p.Frequency)
                 .WithOne(p => p.Schedule)
                 .HasForeignKey<Frequency>(p => p.ScheduleId);

        }
    }
}
