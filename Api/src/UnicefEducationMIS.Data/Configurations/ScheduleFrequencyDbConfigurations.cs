using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    public class ScheduleFrequencyDbConfigurations : IEntityTypeConfiguration<Frequency>
    {
             
        public void Configure(EntityTypeBuilder<Frequency> builder)
        {
            builder.ToTable(TableNames.ScheduleFrequencies);
            builder.HasKey(p => p.ScheduleId);
            var valueConverter = new ValueConverter<List<DayOfWeek>, string>(ValueConversions.DayOfWeekListToString, ValueConversions.StringToDayOfWeekList);
            var valueComparer = new ValueComparer<List<DayOfWeek>>((c1, c2) => c1.SequenceEqual(c2), c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), c => c);

            builder.Property(p => p.DaysOfWeek)
                .HasConversion(valueConverter)
                .HasMaxLength(80)
                .Metadata
                .SetValueComparer(valueComparer);

            
        }
    }
}
