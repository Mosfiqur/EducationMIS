using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models.Framework;

namespace UnicefEducationMIS.Data.Configurations.Framework
{
    public class MonitoringFrameworkDynamicCellDbConfigurations : IEntityTypeConfiguration<MonitoringFrameworkDynamicCell>
    {
        public void Configure(EntityTypeBuilder<MonitoringFrameworkDynamicCell> builder)
        {
            builder.ToTable(TableNames.MonitoringDynamicCells);

            builder.Property(p => p.DateCreated)
            .HasColumnType("datetime");

            builder.Property(p => p.LastUpdated)
            .HasColumnType("datetime");

        }
    }
}
