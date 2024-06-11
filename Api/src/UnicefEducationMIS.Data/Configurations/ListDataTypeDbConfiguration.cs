using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    public class ListDataTypeDbConfiguration : IEntityTypeConfiguration<ListDataType>
    {
        public void Configure(EntityTypeBuilder<ListDataType> builder)
        {
            builder.ToTable(TableNames.List);

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(2048);

            builder.Property(e => e.CreatedBy).HasColumnType("int");
            builder.Property(e => e.DateCreated).HasColumnType("datetime");
            builder.Property(e => e.UpdatedBy).HasColumnType("int");
            builder.Property(e => e.LastUpdated).HasColumnType("datetime");

            builder.HasData(new List<ListDataType> { new ListDataType() { Id = 1, Name = "Education Level", CreatedBy = 1, DateCreated = DateTime.Now } });

            builder.HasData(new List<ListDataType> { new ListDataType() { Id = 2, Name = "Damage Status", CreatedBy = 1, DateCreated = DateTime.Now } });
            builder.HasData(new List<ListDataType> { new ListDataType() { Id = 3, Name = "Damage Caused By", CreatedBy = 1, DateCreated = DateTime.Now } });
            builder.HasData(new List<ListDataType> { new ListDataType() { Id = 4, Name = "Action Required", CreatedBy = 1, DateCreated = DateTime.Now } });
            builder.HasData(new List<ListDataType> { new ListDataType() { Id = 5, Name = "Taken Action", CreatedBy = 1, DateCreated = DateTime.Now } });
            builder.HasData(new List<ListDataType> { new ListDataType() { Id = 6, Name = "How many time the facility damage", CreatedBy = 1, DateCreated = DateTime.Now } });

            builder.HasData(new List<ListDataType> { new ListDataType() { Id = 7, Name = "Targeted Population", CreatedBy = 1, DateCreated = DateTime.Now } });
            builder.HasData(new List<ListDataType> { new ListDataType() { Id = 8, Name = "Facility Type", CreatedBy = 1, DateCreated = DateTime.Now } });
            builder.HasData(new List<ListDataType> { new ListDataType() { Id = 9, Name = "Facility Status", CreatedBy = 1, DateCreated = DateTime.Now } });
            builder.HasData(new List<ListDataType> { new ListDataType() { Id = 10, Name = "Sex", CreatedBy = 1, DateCreated = DateTime.Now } });
           

        }
    }
}
