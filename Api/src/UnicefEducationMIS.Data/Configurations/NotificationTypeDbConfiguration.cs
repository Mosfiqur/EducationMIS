using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    public class NotificationTypeDbConfiguration : IEntityTypeConfiguration<Core.Models.NotificationType>
    {
        public void Configure(EntityTypeBuilder<NotificationType> builder)
        {
            builder.ToTable(TableNames.NotificationTypes);
            builder.HasKey(t => t.Id);
            builder.Property(e => e.Name).HasColumnType("text");


            builder.HasData(new List<NotificationType>() {
                new NotificationType() {Id=1,Name= "Beneficiary Instance Start",CreatedBy=1,DateCreated=DateTime.Now },
            new NotificationType() {Id=2,Name= "Facility Instance Start",CreatedBy=1,DateCreated=DateTime.Now },
            new NotificationType() {Id=3,Name= "Recollect Facility",CreatedBy=1,DateCreated=DateTime.Now },
            new NotificationType() {Id=4,Name= "Recollect Beneficiary",CreatedBy=1,DateCreated=DateTime.Now },
            new NotificationType() {Id=5,Name= "Assign Teacher",CreatedBy=1,DateCreated=DateTime.Now }});
        }
    }
}
