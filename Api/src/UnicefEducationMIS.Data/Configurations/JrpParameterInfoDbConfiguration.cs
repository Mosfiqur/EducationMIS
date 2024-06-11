using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    class JrpParameterInfoDbConfiguration : IEntityTypeConfiguration<JrpParameterInfo>
    {
        public void Configure(EntityTypeBuilder<JrpParameterInfo> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.ObjectiveIndicator)
                .WithMany(x => x.JrpParameterInfos)
                .HasForeignKey(x => x.TargetId)
                .HasConstraintName("FK__JrpParameterInfo__TargetId");
            builder.HasOne(x => x.EntityDynamicColumn)
                .WithMany(x => x.JrpParameterInfos)
                .HasForeignKey(x => x.IndicatorId)
                .HasConstraintName("FK__JrpParameterInfo__IndicatorId");
            //builder.Property(e => e.Formula).HasConversion<int>();

            builder.Property(e => e.Name).HasMaxLength(500);

            builder.ToTable(TableNames.JrpParameterInfo);

            builder.Property(e => e.CreatedBy).HasColumnType("int");
            builder.Property(e => e.DateCreated).HasColumnType("datetime");
            builder.Property(e => e.UpdatedBy).HasColumnType("int");
            builder.Property(e => e.LastUpdated).HasColumnType("datetime");

        }
    }
}
