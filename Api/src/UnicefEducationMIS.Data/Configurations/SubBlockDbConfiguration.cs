using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    public class SubBlockDbConfiguration : IEntityTypeConfiguration<SubBlock>
    {
        public void Configure(EntityTypeBuilder<SubBlock> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(2048);

            builder.HasOne(x => x.Block)
                .WithMany(x => x.SubBlocks)
                .HasForeignKey(x => x.BlockId)
                .HasConstraintName("FK__SubBlocks__BlockId");

            builder.ToTable(TableNames.SubBlock);

            builder.Property(e => e.CreatedBy).HasColumnType("int");
            builder.Property(e => e.DateCreated).HasColumnType("datetime");
            builder.Property(e => e.UpdatedBy).HasColumnType("int");
            builder.Property(e => e.LastUpdated).HasColumnType("datetime");

        }
    }
}
