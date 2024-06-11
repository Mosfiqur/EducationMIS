using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    public class UserLevelDbConfigurations : IEntityTypeConfiguration<UserLevel>
    {
        public void Configure(EntityTypeBuilder<UserLevel> builder)
        {

            builder.ToTable(TableNames.UserLevels);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.LevelName)                
                .HasMaxLength(128);

            builder.Property(p => p.Rank)
                .HasConversion<int>();

            builder.Property(p => p.DateCreated)
                .HasColumnType("datetime");

            builder.Property(p => p.LastUpdated)
                .HasColumnType("datetime");
      
        }
    }
}
