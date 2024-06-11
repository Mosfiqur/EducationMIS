using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    public class UserDynamicCellDbConfigurations : IEntityTypeConfiguration<UserDynamicCell>
    {
        public void Configure(EntityTypeBuilder<UserDynamicCell> builder)
        {
            builder.ToTable(TableNames.UserDynamicCells);

            builder.Property(p => p.DateCreated)
                .HasColumnType("datetime");

            builder.Property(p => p.LastUpdated)
                .HasColumnType("datetime");
        }
    }
}