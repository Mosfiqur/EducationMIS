using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    public class UpazilaDbConfiguration : IEntityTypeConfiguration<Upazila>
    {
        public void Configure(EntityTypeBuilder<Upazila> builder)
        {

            builder.ToTable(TableNames.Upazila);

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(2048);

            builder.HasOne(a => a.District)
                .WithMany(a => a.Upazilas)
                .HasForeignKey(a => a.DistrictId)
                .HasConstraintName("FK__Upazila__DistrictId");


            builder.Property(p => p.DateCreated)
                .HasColumnType("datetime");

            builder.Property(p => p.LastUpdated)
                .HasColumnType("datetime");

        }
    }
}
