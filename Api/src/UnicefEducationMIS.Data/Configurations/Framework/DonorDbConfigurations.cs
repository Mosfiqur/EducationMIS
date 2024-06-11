using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models.Framework;

namespace UnicefEducationMIS.Data.Configurations.Framework
{
    public class DonorDbConfigurations : IEntityTypeConfiguration<Donor>
    {
        public void Configure(EntityTypeBuilder<Donor> builder)
        {
            builder.ToTable(TableNames.Donor);
            builder.Property(p => p.DateCreated).HasColumnType("datetime");
            builder.Property(p => p.LastUpdated).HasColumnType("datetime");
        }
    }
}