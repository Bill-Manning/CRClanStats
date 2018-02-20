using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using CRClanStats.DataModel.Maps;
using CRClanStats.DataModel.Models;
using JetBrains.Annotations;

namespace CRClanStats.DataModel.Context
{
    [PublicAPI]
    public class CRClanStats : DbContext, ICRClanStats
    {
        static CRClanStats()
        {
            Database.SetInitializer<CRClanStats>(null);
        }

        public CRClanStats()
            : base("Name=CRClanStats")
        {
        }

        //public CRClanStats(string connectionString)
        //    : base(connectionString)
        //{
        //}

        //public CRClanStats(string connectionString, DbCompiledModel model)
        //    : base(connectionString, model)
        //{
        //}

        //public CRClanStats(DbConnection existingConnection, bool contextOwnsConnection)
        //    : base(existingConnection, contextOwnsConnection)
        //{
        //}

        //public CRClanStats(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
        //    : base(existingConnection, model, contextOwnsConnection)
        //{
        //}

        public DbSet<ClanStat> ClanStats { get; set; } // ClanStats
        public DbSet<ClanStatsWeekly> ClanStatsWeeklies { get; set; } // ClanStatsWeekly
        public DbSet<Player> Players { get; set; } // Player
        public DbSet<PlayerStat> PlayerStats { get; set; } // PlayerStats
        public DbSet<PlayerStatsWeekly> PlayerStatsWeeklies { get; set; } // PlayerStatsWeekly


        public bool IsSqlParameterNull(SqlParameter param)
        {
            var sqlValue = param.SqlValue;
            var nullableValue = sqlValue as INullable;
            if (nullableValue != null)
                return nullableValue.IsNull;
            return sqlValue == null || sqlValue == DBNull.Value;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new ClanStatMap());
            modelBuilder.Configurations.Add(new ClanStatsWeeklyMap());
            modelBuilder.Configurations.Add(new PlayerMap());
            modelBuilder.Configurations.Add(new PlayerStatMap());
            modelBuilder.Configurations.Add(new PlayerStatsWeeklyMap());
        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new ClanStatMap(schema));
            modelBuilder.Configurations.Add(new ClanStatsWeeklyMap(schema));
            modelBuilder.Configurations.Add(new PlayerMap(schema));
            modelBuilder.Configurations.Add(new PlayerStatMap(schema));
            modelBuilder.Configurations.Add(new PlayerStatsWeeklyMap(schema));
            return modelBuilder;
        }
    }
}