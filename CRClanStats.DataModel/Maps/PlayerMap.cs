using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CRClanStats.DataModel.Models;
using JetBrains.Annotations;

namespace CRClanStats.DataModel.Maps
{
    [PublicAPI]
    public class PlayerMap : EntityTypeConfiguration<Player>
    {
        public PlayerMap()
            : this("dbo")
        {
        }

        public PlayerMap(string schema)
        {
            ToTable("Player", schema);
            HasKey(x => x.PlayerId);

            Property(x => x.PlayerId).HasColumnName(@"PlayerId").HasColumnType("int").IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Tag).HasColumnName(@"Tag").HasColumnType("nchar").IsRequired().IsFixedLength()
                .HasMaxLength(10);
            Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nchar").IsRequired().IsFixedLength()
                .HasMaxLength(10);
            Property(x => x.FormerName).HasColumnName(@"FormerName").HasColumnType("nchar").IsOptional().IsFixedLength()
                .HasMaxLength(10);
            Property(x => x.JoinCount).HasColumnName(@"JoinCount").HasColumnType("int").IsRequired();
            Property(x => x.IsCurrentMember).HasColumnName(@"IsCurrentMember").HasColumnType("bit").IsRequired();
        }
    }
}