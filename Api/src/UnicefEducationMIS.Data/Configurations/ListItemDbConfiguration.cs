using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    public class ListItemDbConfiguration : IEntityTypeConfiguration<ListItem>
    {
        public void Configure(EntityTypeBuilder<ListItem> builder)
        {
            builder.ToTable(TableNames.ListItem);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).HasMaxLength(2048);
            builder.Property(e => e.Value).HasColumnType("int");

            builder.HasOne(x => x.ColumnList)
                .WithMany(x => x.ListItems)
                .HasForeignKey(x => x.ColumnListId)
                .HasConstraintName("FK__ListItems__ColumnListId");

            builder.Property(e => e.CreatedBy).HasColumnType("int");
            builder.Property(e => e.DateCreated).HasColumnType("datetime");
            builder.Property(e => e.UpdatedBy).HasColumnType("int");
            builder.Property(e => e.LastUpdated).HasColumnType("datetime");

            builder.HasData(new List<ListItem>() { new ListItem { Id = 1, Title = "Level 1", Value = 1, ColumnListId = 1, CreatedBy = 1, DateCreated = new DateTime(2020,1,1) }
            ,new ListItem {Id=2,Title="Level 2",Value=2,ColumnListId=1,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=3,Title="Level 3",Value=3,ColumnListId=1,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=4,Title="Level 4",Value=4,ColumnListId=1,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }

                        ,new ListItem {Id=5, Title = "Slightly Damaged", Value = 1, ColumnListId = 2, CreatedBy = 1, DateCreated = new DateTime(2020,1,1) }
            ,new ListItem {Id=6,Title="Medium Damaged",Value=2,ColumnListId=2,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=7,Title="Heavily Damaged",Value=3,ColumnListId=2,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=8,Title="Repaired",Value=4,ColumnListId=2,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=9,Title="Decommissioned",Value=5,ColumnListId=2,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=10,Title="Others (Mentioned on Remarks)",Value=6,ColumnListId=2,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }


            ,new ListItem {Id=11, Title = "Cyclone", Value = 1, ColumnListId = 3, CreatedBy = 1, DateCreated = new DateTime(2020,1,1) }
            ,new ListItem {Id=12,Title="Flood",Value=2,ColumnListId=3,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=13,Title="Landslide",Value=3,ColumnListId=3,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=14,Title="Landslide & Flood",Value=4,ColumnListId=3,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=15,Title="Landslide, Flood & Storm surge",Value=5,ColumnListId=3,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=16,Title="Lighthening",Value=6,ColumnListId=3,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=17, Title = "Rain", Value = 7, ColumnListId = 3, CreatedBy = 1, DateCreated = new DateTime(2020,1,1) }
            ,new ListItem {Id=18,Title="Rain and Flood",Value=8,ColumnListId=3,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=19,Title="Rain and Storm surge",Value=9,ColumnListId=3,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=20,Title="Rain, Flood and storm surge",Value=10,ColumnListId=3,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=21,Title="Road construction",Value=11,ColumnListId=3,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=22,Title="Storm surge",Value=12,ColumnListId=3,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=23,Title="Lands taken by authority",Value=13,ColumnListId=3,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=24,Title="Others (Mentioned on Remarks)",Value=14,ColumnListId=3,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }

            ,new ListItem {Id=25, Title = "Tied-up/renovate LC that is in potential damage threat", Value = 1, ColumnListId = 4, CreatedBy = 1, DateCreated = new DateTime(2020,1,1) }
            ,new ListItem {Id=26,Title="Repair damaged LC",Value=2,ColumnListId=4,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=27,Title="Others (Mentioned on Remarks)",Value=3,ColumnListId=4,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }

            ,new ListItem {Id=28, Title = "Repaired", Value = 1, ColumnListId = 5, CreatedBy = 1, DateCreated = new DateTime(2020,1,1) }
            ,new ListItem {Id=29,Title="Repair work ongoing",Value=2,ColumnListId=5,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=30,Title="Reallocation",Value=3,ColumnListId=5,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=31,Title="Planned for action",Value=4,ColumnListId=5,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=32,Title="Decommisionned",Value=5,ColumnListId=5,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=33,Title="No action taken",Value=6,ColumnListId=5,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=34, Title = "Others(Mentioned on Remarks)", Value = 7, ColumnListId = 5, CreatedBy = 1, DateCreated = new DateTime(2020,1,1) }


            ,new ListItem {Id=35, Title = "1st time", Value = 1, ColumnListId = 6, CreatedBy = 1, DateCreated = new DateTime(2020,1,1) }
            ,new ListItem {Id=36,Title="2nd time",Value=2,ColumnListId=6,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=37,Title="3rd time",Value=3,ColumnListId=6,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=38,Title="4th time",Value=4,ColumnListId=6,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=39,Title="5th time",Value=5,ColumnListId=6,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=40,Title="6th time",Value=6,ColumnListId=6,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=41, Title = "7th time", Value = 7, ColumnListId = 6, CreatedBy = 1, DateCreated = new DateTime(2020,1,1) }
            ,new ListItem {Id=42,Title="8th time",Value=8,ColumnListId=6,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=43,Title="9th time",Value=9,ColumnListId=6,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=44,Title="10th time",Value=10,ColumnListId=6,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }



            ,new ListItem { Id =45, Title = "Host Communities", Value = 1, ColumnListId = 7, CreatedBy = 1, DateCreated = new DateTime(2020,1,1) }
            ,new ListItem {Id=46,Title="Refugee Communities",Value=2,ColumnListId=7,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=47,Title="Both Communities",Value=3,ColumnListId=7,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }

            ,new ListItem { Id =48, Title = "Learning Center(LC)", Value = 1, ColumnListId = 8, CreatedBy = 1, DateCreated = new DateTime(2020,1,1) }
            ,new ListItem {Id=49,Title="Community Based Learning Facility",Value=2,ColumnListId=8,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=50,Title="Cross Sectoral Shared Learning Facility (CSSLF)",Value=3,ColumnListId=8,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }

            ,new ListItem { Id =51, Title = "Ongoing", Value = 1, ColumnListId = 9, CreatedBy = 1, DateCreated = new DateTime(2020,1,1) }
            ,new ListItem {Id=52,Title="Completed",Value=2,ColumnListId=9,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=53,Title="Planned",Value=3,ColumnListId=9,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=54,Title="Decommission",Value=4,ColumnListId=9,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
 
            ,new ListItem { Id =55, Title = "Male", Value = 1, ColumnListId = 10, CreatedBy = 1, DateCreated = new DateTime(2020,1,1) }
            ,new ListItem {Id=56,Title="Female",Value=2,ColumnListId=10,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }
            ,new ListItem {Id=57,Title="Others",Value=3,ColumnListId=10,CreatedBy=1, DateCreated=new DateTime(2020,1,1) }


            });
        }
    }
}
