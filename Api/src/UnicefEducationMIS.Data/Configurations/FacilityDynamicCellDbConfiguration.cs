using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    class FacilityDynamicCellDbConfiguration : IEntityTypeConfiguration<Core.Models.FacilityDynamicCell>
    {
        public void Configure(EntityTypeBuilder<FacilityDynamicCell> builder)
        {
            builder.HasKey(p => p.Id);
            builder.ToTable(TableNames.FacilityDynamicCell);

            builder.Property(e => e.CreatedBy).HasColumnType("int");
            builder.Property(e => e.DateCreated).HasColumnType("datetime");
            builder.Property(e => e.UpdatedBy).HasColumnType("int");
            builder.Property(e => e.LastUpdated).HasColumnType("datetime");

            builder.HasOne(x => x.Facility)
                .WithMany(x => x.FacilityDynamicCells)
                .HasForeignKey(x => x.FacilityId)
                .HasConstraintName("FK__FacilityDynamicCells__FacilityId");

            builder.HasOne(x => x.EntityDynamicColumn)
                .WithMany(x => x.FacilityDynamicCells)
                .HasForeignKey(x => x.EntityDynamicColumnId)
                .HasConstraintName("FK__FacilityDynamicCells__EntityDynamicColumnId");

            builder.HasOne(x => x.Instance)
                .WithMany(x => x.FacilityDynamicCells)
                .HasForeignKey(x => x.InstanceId)
                .HasConstraintName("FK__FacilityDynamicCells__InstanceId");

           
        }
    }
}
