using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;

namespace UnicefEducationMIS.Data.Configurations
{
    public class GridViewDbConfiguration : IEntityTypeConfiguration<Core.Models.GridView>
    {
        
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Core.Models.GridView> builder)
        {
            builder.ToTable(TableNames.GridView);
            builder.HasKey(e => e.Id);
            builder.Property(e => e.EntityTypeId).HasConversion<int>();

            builder.Property(e => e.Name).HasMaxLength(2048);
            builder.Property(e => e.CreatedBy).HasColumnType("int");
            builder.Property(e => e.DateCreated).HasColumnType("datetime");
            builder.Property(e => e.UpdatedBy).HasColumnType("int");
            builder.Property(e => e.LastUpdated).HasColumnType("datetime");
        }
    }
}
