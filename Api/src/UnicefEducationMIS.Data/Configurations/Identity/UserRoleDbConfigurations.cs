using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;

namespace UnicefEducationMIS.Data.Configurations.Identity
{
    public class UserRoleDbConfiguratoins : IEntityTypeConfiguration<Core.Models.Identity.UserRole>
    {
        public void Configure(EntityTypeBuilder<Core.Models.Identity.UserRole> builder)
        {
            // Composite key
            builder.HasKey(p => new { p.UserId, p.RoleId });            
            builder.ToTable(TableNames.UserRoles);
        }
    }
}
