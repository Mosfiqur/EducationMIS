using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    public class CampCoordinateDbConfiguration : IEntityTypeConfiguration<CampCoordinate>
    {
        public void Configure(EntityTypeBuilder<CampCoordinate> builder)
        {
            builder.ToTable(TableNames.CampCoordinate);
            builder.HasKey(t => t.Id);
            builder.Property(x => x.Longitude).HasColumnType("double");
            builder.Property(x => x.Latitude).HasColumnType("double");

            builder.Property(e => e.CreatedBy).HasColumnType("int");
            builder.Property(e => e.DateCreated).HasColumnType("datetime");
            builder.Property(e => e.UpdatedBy).HasColumnType("int");
            builder.Property(e => e.LastUpdated).HasColumnType("datetime");
        }
    }
}
