using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models.Views;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Data.Configurations
{
    public class FacilityViewDbConfigurations : IEntityTypeConfiguration<FacilityView>

    {
        public void Configure(EntityTypeBuilder<FacilityView> builder)
        {
            builder.HasNoKey()
                .ToView(TableNames.FacilityView);
        }
    }
}
