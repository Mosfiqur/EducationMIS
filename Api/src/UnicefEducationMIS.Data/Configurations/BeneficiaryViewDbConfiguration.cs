using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnicefEducationMIS.Core.Models.Views;

namespace UnicefEducationMIS.Data.Configurations
{
    public class BeneficiaryViewDbConfiguration : IEntityTypeConfiguration<Core.Models.Views.BeneficiaryView>
    {
        public void Configure(EntityTypeBuilder<BeneficiaryView> builder)
        {
            builder.HasNoKey().ToView("View_BeneficiaryFixedColumns");
            CultureInfo provider = CultureInfo.InvariantCulture;
            builder.Property(a => a.Disabled).HasConversion(a => a.ToString(),
                a => a.ToLower()=="Yes".ToLower()?true:false);

            builder.Property(a => a.DateOfBirth).HasConversion(a => a.ToString(),a=> DateTime.Parse(a));
            builder.Property(a => a.EnrollmentDate).HasConversion(a => a.ToString(), a => DateTime.Parse(a));
           
        }
    }
}
