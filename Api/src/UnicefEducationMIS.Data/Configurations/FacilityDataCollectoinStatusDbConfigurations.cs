using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Data.Configurations
{
    public class FacilityDataCollectoinStatusDbConfigurations : IEntityTypeConfiguration<FacilityDataCollectionStatus>
    {
        public void Configure(EntityTypeBuilder<FacilityDataCollectionStatus> builder)
        {
            builder.ToTable(TableNames.FacilityDataCollectionStatus);

        }
    }
}
