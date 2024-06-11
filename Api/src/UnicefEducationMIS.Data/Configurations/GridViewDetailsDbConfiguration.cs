using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    public class GridViewDetailsDbConfiguration : IEntityTypeConfiguration<Core.Models.GridViewDetails>
    {
        public void Configure(EntityTypeBuilder<GridViewDetails> builder)
        {
            builder.ToTable(TableNames.GridViewDetails);
            builder.HasKey(t => t.Id);
          
            builder.Property(e => e.CreatedBy).HasColumnType("int");
            builder.Property(e => e.DateCreated).HasColumnType("datetime");
            builder.Property(e => e.UpdatedBy).HasColumnType("int");
            builder.Property(e => e.LastUpdated).HasColumnType("datetime");

            builder.Property(e => e.EntityDynamicColumnId).HasColumnType("bigint");
            builder.Property(e => e.ColumnOrder).HasColumnType("int");
            builder.Property(e => e.GridViewId).HasColumnType("bigint");

            builder.HasOne(d => d.EntityDynamicColumn)
                  .WithMany(p => p.BeneficiaryViewDetails)
                  .HasForeignKey(d => d.EntityDynamicColumnId)
                  .HasConstraintName("FK__BeneficiaryViewDetails__EntityColumnId");

            builder.HasOne(d => d.BeneficiaryView)
               .WithMany(p => p.GridViewDetails)
               .HasForeignKey(d => d.GridViewId)
               .HasConstraintName("FK__BeneficiaryViewDetails__BeneficiaryGridViewId");
        }
    }
}
