using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models.Framework;

namespace UnicefEducationMIS.Data.Configurations.Framework
{
    public class TargetFrameworkDynamicCellDbConfigurations : IEntityTypeConfiguration<TargetFrameworkDynamicCell>
    {
        public void Configure(EntityTypeBuilder<TargetFrameworkDynamicCell> builder)
        {
            builder.ToTable(TableNames.TargetDynamicCells);

            builder.Property(p => p.DateCreated)
            .HasColumnType("datetime");

            builder.Property(p => p.LastUpdated)
            .HasColumnType("datetime");

        }
    }
}
