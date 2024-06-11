using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models.Framework;

namespace UnicefEducationMIS.Data.Configurations.Framework
{
    class AgeGroupDbConfiguration : IEntityTypeConfiguration<AgeGroup>
    {
        public void Configure(EntityTypeBuilder<AgeGroup> builder)
        {
            builder.ToTable(TableNames.AgeGroup);

            builder.Property(p => p.DateCreated)
            .HasColumnType("datetime");

            builder.Property(p => p.LastUpdated)
            .HasColumnType("datetime");
        }
    }
}
