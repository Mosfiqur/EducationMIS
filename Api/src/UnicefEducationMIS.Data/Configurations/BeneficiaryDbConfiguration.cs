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
    public class BeneficiaryDbConfiguration : IEntityTypeConfiguration<Core.Models.Beneficiary>
    {
        public void Configure(EntityTypeBuilder<Core.Models.Beneficiary> builder)
        {
            builder.ToTable(TableNames.Beneficiary);
            builder.HasKey(t => t.Id);

            //builder.Property(e => e.Remarks).HasColumnType("text");

            //builder.Property(e => e.LevelOfStudy).HasConversion<int>();
            //builder.Property(e => e.Sex).HasConversion<int>();
            //builder.Property(e => e.DateOfBirth).HasColumnType("datetime");
            //builder.Property(e => e.EnrollmentDate).HasColumnType("datetime");

            //builder.Property(e => e.FacilityId).HasColumnType("bigint");

            builder.Property(e => e.CreatedBy).HasColumnType("int");
            builder.Property(e => e.DateCreated).HasColumnType("datetime");
            builder.Property(e => e.UpdatedBy).HasColumnType("int");
            builder.Property(e => e.LastUpdated).HasColumnType("datetime");


            //builder.HasOne(d => d.BeneficiaryCamp)
            //.WithMany(p => p.BeneficiarieCamps)
            //.HasForeignKey(d => d.BeneficiaryCampId)
            //.HasConstraintName("FK__Beneficiary__BeneficiaryCampId");

            //builder.HasOne(d => d.Block)
            //.WithMany(p => p.Beneficiaries)
            //.HasForeignKey(d => d.BlockId)
            //.HasConstraintName("FK__Beneficiary__BlockId");

            //builder.HasOne(d => d.SubBlock)
            //.WithMany(p => p.Beneficiaries)
            //.HasForeignKey(d => d.SubBlockId)
            //.HasConstraintName("FK__Beneficiary__SubBlockId");
           
        }
    }
}
