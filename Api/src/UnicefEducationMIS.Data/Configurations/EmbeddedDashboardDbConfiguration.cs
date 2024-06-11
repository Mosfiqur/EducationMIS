using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    public class EmbeddedDashboardDbConfiguration : IEntityTypeConfiguration<Core.Models.EmbeddedDashboard>
    {
        public void Configure(EntityTypeBuilder<EmbeddedDashboard> builder)
        {
            builder.ToTable(TableNames.EmbeddedDashboard);
            builder.HasKey(t => t.Id);

            builder.Property(e => e.Name).HasColumnType("text");
            builder.Property(e => e.Link).HasColumnType("text");
        }
    }
}
