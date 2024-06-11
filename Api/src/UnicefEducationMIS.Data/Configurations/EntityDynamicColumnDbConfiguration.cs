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
    public class EntityDynamicColumnDbConfiguration : IEntityTypeConfiguration<Core.Models.EntityDynamicColumn>
    {
        public void Configure(EntityTypeBuilder<EntityDynamicColumn> builder)
        {
            builder.ToTable(TableNames.EntityDynamicColumn);
            builder.HasKey(t => t.Id);
            builder.Property(e => e.Name).HasMaxLength(2048);
            builder.Property(e => e.EntityTypeId).HasConversion<int>();
            builder.HasOne(d => d.ColumnList)
                .WithMany(p => p.EntityDynamicColumns)
                .HasForeignKey(d => d.ColumnListId)
                .HasConstraintName("FK__EntityDynamicColumns__ColumnListId");

            builder.Property(e => e.ColumnType).HasConversion<int>();
            builder.Property(e => e.Unit).HasMaxLength(1024);
            builder.Property(e => e.IsFixed).HasColumnType("BOOLEAN");
            builder.Property(e => e.IsMultiValued).HasColumnType("BOOLEAN");

            builder.Property(e => e.CreatedBy).HasColumnType("int");
            builder.Property(e => e.DateCreated).HasColumnType("datetime");
            builder.Property(e => e.UpdatedBy).HasColumnType("int");
            builder.Property(e => e.LastUpdated).HasColumnType("datetime");

            //builder.HasOne(d => d.UserCreated)
            //  .WithMany(p => p.EntityDynamicColumnCreated)
            //  .HasForeignKey(d => d.CreatedBy)
            //  .HasConstraintName("FK__EntityDynamicColumn__CreatedBy");
            //builder.HasOne(d => d.UserUpdated)
            //  .WithMany(p => p.EntityDynamicColumnUpdated)
            //  .HasForeignKey(d => d.UpdatedBy)
            //  .HasConstraintName("FK__EntityDynamicColumn__UpdatedBy");

        }
    }
}
