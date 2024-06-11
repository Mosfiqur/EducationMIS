using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    public class NotificationDbConfiguration : IEntityTypeConfiguration<Core.Models.Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable(TableNames.Notifications);
            builder.HasKey(t => t.Id);

            builder.Property(e => e.Uri).HasColumnType("text");
            builder.Property(e => e.Data).HasColumnType("text");
            builder.Property(e => e.Details).HasColumnType("text");

            builder.HasOne(d => d.NotificationType)
             .WithMany(p => p.Notifications)
             .HasForeignKey(d => d.NotificationTypeId)
             .HasConstraintName("FK__Notification__NotificationTypeId");


        }
    }
}
