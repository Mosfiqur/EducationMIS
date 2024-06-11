using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    public class InstanceIndicatorDbConfiguration : IEntityTypeConfiguration<InstanceIndicator>
    {
        public void Configure(EntityTypeBuilder<InstanceIndicator> builder)
        {
            builder.ToTable(TableNames.InstanceIndicator);

            builder.HasKey(x => new { x.EntityDynamicColumnId, x.InstanceId });

            builder.HasOne(x => x.Instance)
                .WithMany(x => x.InstanceIndicators)
                .HasForeignKey(x => x.InstanceId)
                .HasConstraintName("FK_InstanceIndicators_InstanceId");

            builder.HasOne(x => x.EntityDynamicColumn)
                .WithMany(x => x.InstanceIndicators)
                .HasForeignKey(x => x.EntityDynamicColumnId)
                .HasConstraintName("FK_InstanceIndicators_EntityDynamicColumnId");

            builder.Property(e => e.CreatedBy).HasColumnType("int");
            builder.Property(e => e.DateCreated).HasColumnType("datetime");
            builder.Property(e => e.UpdatedBy).HasColumnType("int");
            builder.Property(e => e.LastUpdated).HasColumnType("datetime");

           
        }
    }
}
