using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Data.Configurations
{
    class BeneciaryDataCollectionStatusDbConfiguration : IEntityTypeConfiguration<Core.Models.BeneficiaryDataCollectionStatus>
    {
        public void Configure(EntityTypeBuilder<BeneficiaryDataCollectionStatus> builder)
        {
            builder.ToTable(TableNames.BeneciaryDataCollectionStatus);
            builder.HasKey(t => t.Id);

            builder.Property(e => e.BeneficiaryId).HasColumnType("bigint");
            builder.Property(e => e.Status).HasConversion<int>();
            builder.HasOne(d => d.Beneficiary)
             .WithMany(p => p.BeneciaryDataCollectionStatuses)
             .HasForeignKey(d => d.BeneficiaryId)
             .HasConstraintName("FK__BeneciaryDataCollectionStatuses__BeneficiaryId");
            
            builder.HasOne(d => d.ScheduleInstance)
             .WithMany(p => p.BeneciariesDataCollectionStatus)
             .HasForeignKey(d => d.InstanceId)
             .HasConstraintName("FK__BeneciaryDataCollectionStatuses__InstanceId");
                        
        }
    }
}
