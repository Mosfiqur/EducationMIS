using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
   public class InstanceMappingDbConfiguration : IEntityTypeConfiguration<InstanceMapping>
    {
        public void Configure(EntityTypeBuilder<InstanceMapping> builder)
        {
            builder.ToTable(TableNames.InstanceMapping);
            builder.HasKey(x => x.BeneficiaryInstanceId);
            builder.Ignore(a => a.Id);
        }
    }
}
