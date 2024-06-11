using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models.Framework;

namespace UnicefEducationMIS.Data.Configurations.Framework
{
    public class BudgetFrameworkDynamicCellDbConfigurations : IEntityTypeConfiguration<BudgetFrameworkDynamicCell>
    {
        public void Configure(EntityTypeBuilder<BudgetFrameworkDynamicCell> builder)
        {
            builder.ToTable(TableNames.BudgetDynamicCells);
            builder.Property(p => p.DateCreated)
            .HasColumnType("datetime");

            builder.Property(p => p.LastUpdated)
            .HasColumnType("datetime");
        }
    }
}
