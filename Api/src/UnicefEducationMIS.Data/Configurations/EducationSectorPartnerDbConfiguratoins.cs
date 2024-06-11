using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    public class EducationSectorPartnerDbConfiguratoins : IEntityTypeConfiguration<EducationSectorPartner>
    {
        public void Configure(EntityTypeBuilder<EducationSectorPartner> builder)
        {
            builder.ToTable(TableNames.EducationSectorPartners);

            builder.HasKey(p => p.Id);
            builder.Property(p => p.PartnerName).HasMaxLength(256);
            builder.Ignore(p => p.PartnerType);

            builder.Property(p => p.DateCreated)
                .HasColumnType("datetime");

            builder.Property(p => p.LastUpdated)
                .HasColumnType("datetime");

                   
        }
    }
}
