using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CRClanStats.DataModel.Models;
using JetBrains.Annotations;

namespace CRClanStats.DataModel.Maps
{
    [PublicAPI]
    public class PlayerStatMap : EntityTypeConfiguration<PlayerStat>
    {
        public PlayerStatMap()
            : this("dbo")
        {
        }

        public PlayerStatMap(string schema)
        {
            ToTable("PlayerStats", schema);
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
            HasRequired(a => a.Player).WithMany(b => b.PlayerStats).HasForeignKey(c => c.PlayerId)
                .WillCascadeOnDelete(false); // FK_PlayerStats_ToPlayer
        }
    }
}