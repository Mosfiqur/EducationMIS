using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    public class BeneficiaryDynamicCellDbConfiguration : IEntityTypeConfiguration<Core.Models.BeneficiaryDynamicCell>
    {
        public void Configure(EntityTypeBuilder<Core.Models.BeneficiaryDynamicCell> builder)
        {
            builder.ToTable(TableNames.BeneficiaryDynamicCell);
            builder.HasKey(t => t.Id);
            builder.Property(e => e.Value).HasColumnType("text");
            builder.Property(e => e.Status).HasConversion<int>();
            builder.HasOne(d => d.EntityDynamicColumn)
                   .WithMany(p => p.DynamicCell)
                   .HasForeignKey(d => d.EntityDynamicColumnId)
                   .HasConstraintName("FK__DynamicCe__EntityColumnId");

            builder.HasOne(d => d.Beneficiary)
                   .WithMany(p => p.BenificiaryDynamicCells)
                   .HasForeignKey(d => d.BeneficiaryId)
                   .HasConstraintName("FK__BeenficiaryDynamicCell__BeneficairyId");


            builder.Property(e => e.BeneficiaryId).HasColumnType("bigint");

            builder.Property(e => e.CreatedBy).HasColumnType("int");
            builder.Property(e => e.DateCreated).HasColumnType("datetime");
            builder.Property(e => e.UpdatedBy).HasColumnType("int");
            builder.Property(e => e.LastUpdated).HasColumnType("datetime");

            //builder.HasOne(d => d.UserCreated)
            //  .WithMany(p => p.DynamicCellCreated)
            //  .HasForeignKey(d => d.CreatedBy)
            //  .HasConstraintName("FK__DynamicCell__CreatedBy");
            //builder.HasOne(d => d.UserUpdated)
            //  .WithMany(p => p.DynamicCellUpdated)
            //  .HasForeignKey(d => d.UpdatedBy)
            //  .HasConstraintName("FK__DynamicCell__UpdatedBy");
           
        }
    }
}
