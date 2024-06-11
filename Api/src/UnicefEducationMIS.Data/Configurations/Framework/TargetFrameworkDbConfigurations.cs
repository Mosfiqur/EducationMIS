using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models.Framework;

namespace UnicefEducationMIS.Data.Configurations.Framework
{
    public class TargetFrameworkDbConfigurations : IEntityTypeConfiguration<TargetFramework>
    {
        public void Configure(EntityTypeBuilder<TargetFramework> builder)
        {
            builder.ToTable(TableNames.TargetFrameworks);

            builder.Property(p => p.DateCreated)
            .HasColumnType("datetime");

            builder.Property(p => p.LastUpdated)
            .HasColumnType("datetime");

            builder.Property(p => p.StartMonth)
                .HasConversion<int>();

            builder.Property(p => p.EndMonth)
                .HasConversion<int>();

            builder.Property(p => p.Gender)
                .HasConversion<int>();
            builder.Property(p => p.TargetedPopulation)
                .HasConversion<int>();
        }
    }
}
