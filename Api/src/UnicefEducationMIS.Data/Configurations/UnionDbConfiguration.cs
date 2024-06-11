using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    public class UnionDbConfiguration : IEntityTypeConfiguration<Union>
    {
        public void Configure(EntityTypeBuilder<Union> builder)
        {

            builder.ToTable(TableNames.Union);

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(2048);

            builder.HasOne(x => x.Upazila)
                .WithMany(x => x.Unions)
                .HasForeignKey(x => x.UpazilaId)
                .HasConstraintName("FK__Unions__UpazilaId");

            builder.Property(e => e.CreatedBy).HasColumnType("int");
            builder.Property(e => e.DateCreated).HasColumnType("datetime");
            builder.Property(e => e.UpdatedBy).HasColumnType("int");
            builder.Property(e => e.LastUpdated).HasColumnType("datetime");

        }
    }
}
