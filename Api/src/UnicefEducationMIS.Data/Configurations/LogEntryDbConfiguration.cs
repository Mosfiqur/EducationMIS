using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Configurations
{
    public class LogEntryDbConfiguration:IEntityTypeConfiguration<LogEntry>
    {
        public void Configure(EntityTypeBuilder<LogEntry> builder)
        {
            builder.ToTable(TableNames.LogEntry); 
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserName).HasMaxLength(256);
            builder.Property(x => x.TimeStampUtc).IsRequired(true).HasColumnType("datetime");
            builder.Property(x => x.Category).HasMaxLength(128);
            builder.Property(x => x.Level).HasColumnType("int");
            builder.Property(x => x.Text).HasColumnType("text"); 
            builder.Property(x => x.StateText).HasMaxLength(1024);
            builder.Ignore(x => x.EventId);
            builder.Ignore(x => x.Exception);
            builder.Ignore(x => x.State);
            builder.Ignore(x => x.StateProperties);
            builder.Ignore(x => x.Scopes);
        }
    }
}
