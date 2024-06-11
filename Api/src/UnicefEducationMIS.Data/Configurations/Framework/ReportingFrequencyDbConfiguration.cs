using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models.Framework;

namespace UnicefEducationMIS.Data.Configurations.Framework
{
    class ReportingFrequencyDbConfiguration : IEntityTypeConfiguration<ReportingFrequency>
    {
        public void Configure(EntityTypeBuilder<ReportingFrequency> builder)
        {
            builder.ToTable(TableNames.ReportingFrequency);

            builder.Property(p => p.DateCreated)
            .HasColumnType("datetime");

            builder.Property(p => p.LastUpdated)
            .HasColumnType("datetime");

        }
    }
}
