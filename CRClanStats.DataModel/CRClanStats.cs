// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable EmptyNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.7
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning


namespace CRClanStats.DataModel
{

    using System.Linq;

    public class CRClanStats : System.Data.Entity.DbContext, ICRClanStats
    {
        public System.Data.Entity.DbSet<ClanStat> ClanStats { get; set; }
        public System.Data.Entity.DbSet<ClanStatsWeekly> ClanStatsWeeklies { get; set; }
        public System.Data.Entity.DbSet<Player> Players { get; set; }
        public System.Data.Entity.DbSet<PlayerRole> PlayerRoles { get; set; }
        public System.Data.Entity.DbSet<PlayerStat> PlayerStats { get; set; }
        public System.Data.Entity.DbSet<PlayerStatsWeekly> PlayerStatsWeeklies { get; set; }
        public System.Data.Entity.DbSet<RefactorLog> RefactorLogs { get; set; }

        static CRClanStats()
        {
            System.Data.Entity.Database.SetInitializer<CRClanStats>(null);
        }

        public CRClanStats()
            : base("Name=CRClanStatsLocal")
        {
        }

        public CRClanStats(string connectionString)
            : base(connectionString)
        {
        }

        public CRClanStats(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model)
            : base(connectionString, model)
        {
        }

        public CRClanStats(System.Data.Common.DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
        }

        public CRClanStats(System.Data.Common.DbConnection existingConnection, System.Data.Entity.Infrastructure.DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public bool IsSqlParameterNull(System.Data.SqlClient.SqlParameter param)
        {
            var sqlValue = param.SqlValue;
            var nullableValue = sqlValue as System.Data.SqlTypes.INullable;
            if (nullableValue != null)
                return nullableValue.IsNull;
            return (sqlValue == null || sqlValue == System.DBNull.Value);
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new ClanStatMap());
            modelBuilder.Configurations.Add(new ClanStatsWeeklyMap());
            modelBuilder.Configurations.Add(new PlayerMap());
            modelBuilder.Configurations.Add(new PlayerRoleMap());
            modelBuilder.Configurations.Add(new PlayerStatMap());
            modelBuilder.Configurations.Add(new PlayerStatsWeeklyMap());
            modelBuilder.Configurations.Add(new RefactorLogMap());
        }

        public static System.Data.Entity.DbModelBuilder CreateModel(System.Data.Entity.DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new ClanStatMap(schema));
            modelBuilder.Configurations.Add(new ClanStatsWeeklyMap(schema));
            modelBuilder.Configurations.Add(new PlayerMap(schema));
            modelBuilder.Configurations.Add(new PlayerRoleMap(schema));
            modelBuilder.Configurations.Add(new PlayerStatMap(schema));
            modelBuilder.Configurations.Add(new PlayerStatsWeeklyMap(schema));
            modelBuilder.Configurations.Add(new RefactorLogMap(schema));
            return modelBuilder;
        }
    }
}
// </auto-generated>