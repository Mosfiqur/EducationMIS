using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Data.Configurations
{
    public class FacilityDbConfiguration : IEntityTypeConfiguration<Core.Models.Facility>
    {
        public void Configure(EntityTypeBuilder<Core.Models.Facility> builder)
        {
            builder.ToTable("Facility");
            builder.HasKey(t => t.Id);


            builder.Property(e => e.CreatedBy).HasColumnType("int");
            builder.Property(e => e.DateCreated).HasColumnType("datetime");
            builder.Property(e => e.UpdatedBy).HasColumnType("int");
            builder.Property(e => e.LastUpdated).HasColumnType("datetime");


        }
    }
}
