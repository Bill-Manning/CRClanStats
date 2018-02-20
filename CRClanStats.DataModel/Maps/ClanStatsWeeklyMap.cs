using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CRClanStats.DataModel.Models;
using JetBrains.Annotations;

namespace CRClanStats.DataModel.Maps
{
    [PublicAPI]
    public class ClanStatsWeeklyMap : EntityTypeConfiguration<ClanStatsWeekly>
    {
        public ClanStatsWeeklyMap()
            : this("dbo")
        {
        }

        public ClanStatsWeeklyMap(string schema)
        {
            ToTable("ClanStatsWeekly", schema);
            HasKey(x => x.RecordDate);

            Property(x => x.RecordDate).HasColumnName(@"RecordDate").HasColumnType("datetime2").IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Trophies).HasColumnName(@"Trophies").HasColumnType("int").IsRequired();
            Property(x => x.Donations).HasColumnName(@"Donations").HasColumnType("int").IsRequired();
            Property(x => x.ClanChestCrowns).HasColumnName(@"ClanChestCrowns").HasColumnType("int").IsRequired();
            Property(x => x.ClanChestsCompleted).HasColumnName(@"ClanChestsCompleted").HasColumnType("int")
                .IsRequired();
            Property(x => x.ClanChestsDoneDate).HasColumnName(@"ClanChestsDoneDate").HasColumnType("datetime2")
                .IsOptional();
        }
    }
}