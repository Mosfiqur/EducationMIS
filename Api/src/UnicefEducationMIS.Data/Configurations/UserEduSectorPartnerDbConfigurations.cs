using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    public class UserEduSectorPartnerDbConfigurations : IEntityTypeConfiguration<UserEduSectorPartner>
    {
        public void Configure(EntityTypeBuilder<UserEduSectorPartner> builder)
        {

            builder.ToTable(TableNames.UserEduSectorPartners);

            builder.HasKey(p => new { p.UserId, p.PartnerId, p.PartnerType });
            builder.Property(p => p.PartnerType)
                .HasConversion<int>();
        }
    }
}
