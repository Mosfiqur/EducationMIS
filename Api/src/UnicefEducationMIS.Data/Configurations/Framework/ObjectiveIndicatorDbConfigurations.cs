using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models.Framework;

namespace UnicefEducationMIS.Data.Configurations.Framework
{
    public class ObjectiveIndicatorDbConfigurations : IEntityTypeConfiguration<ObjectiveIndicator>
    {
        public void Configure(EntityTypeBuilder<ObjectiveIndicator> builder)
        {
            builder.ToTable(TableNames.ObjectiveIndicators);
            builder.Property(p => p.DateCreated)
                .HasColumnType("datetime");
            builder.Property(p => p.LastUpdated)
                .HasColumnType("datetime");

            builder.Property(p => p.StartDate)
                .HasColumnType("datetime");

            builder.Property(p => p.EndDate)
                .HasColumnType("datetime");

            builder.Property(p => p.Indicator)
                .HasMaxLength(512);

            builder.Property(p => p.Unit)
                .HasMaxLength(512);

        }
    }
}
