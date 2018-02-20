using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CRClanStats.DataModel.Models;
using JetBrains.Annotations;

namespace CRClanStats.DataModel.Maps
{
    public class PlayerStatsWeeklyMap : EntityTypeConfiguration<PlayerStatsWeekly>
    {
        [PublicAPI]
        public PlayerStatsWeeklyMap()
            : this("dbo")
        {
        }

        public PlayerStatsWeeklyMap(string schema)
        {
            ToTable("PlayerStatsWeekly", schema);
            HasKey(x => new {x.RecordDate, x.PlayerId});

            Property(x => x.RecordDate).HasColumnName(@"RecordDate").HasColumnType("datetime2").IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.PlayerId).HasColumnName(@"PlayerId").HasColumnType("int").IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Trophies).HasColumnName(@"Trophies").HasColumnType("int").IsRequired();
            Property(x => x.Donations).HasColumnName(@"Donations").HasColumnType("int").IsRequired();
            Property(x => x.DonationsReceived).HasColumnName(@"DonationsReceived").HasColumnType("int").IsRequired();
            Property(x => x.ClanChestCrowns).HasColumnName(@"ClanChestCrowns").HasColumnType("int").IsRequired();

            // Foreign keys
            HasRequired(a => a.Player).WithMany(b => b.PlayerStatsWeeklies).HasForeignKey(c => c.PlayerId)
                .WillCascadeOnDelete(false); // FK_PlayerStatsWeekly_ToPlayer
        }
    }
}