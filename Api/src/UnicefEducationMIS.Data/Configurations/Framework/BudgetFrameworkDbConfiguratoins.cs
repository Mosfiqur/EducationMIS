using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models.Framework;

namespace UnicefEducationMIS.Data.Configurations.Framework
{
    public class BudgetFrameworkDbConfiguratoins : IEntityTypeConfiguration<BudgetFramework>
    {
        public void Configure(EntityTypeBuilder<BudgetFramework> builder)
        {
            builder.ToTable(TableNames.BudgetFrameworks);

            builder.Property(p => p.DateCreated)
            .HasColumnType("datetime");

            builder.Property(p => p.LastUpdated)
            .HasColumnType("datetime");
        }
    }


}
