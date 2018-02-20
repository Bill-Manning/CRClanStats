using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Threading;
using System.Threading.Tasks;
using CRClanStats.DataModel.Models;
using JetBrains.Annotations;

namespace CRClanStats.DataModel.Context
{
    [PublicAPI]
    public class FakeCRClanStats : ICRClanStats
    {
        public FakeCRClanStats()
        {
            ClanStats = new FakeDbSet<ClanStat>("RecordDate");
            ClanStatsWeeklies = new FakeDbSet<ClanStatsWeekly>("RecordDate");
            Players = new FakeDbSet<Player>("PlayerId");
            PlayerStats = new FakeDbSet<PlayerStat>("RecordDate", "PlayerId");
            PlayerStatsWeeklies = new FakeDbSet<PlayerStatsWeekly>("RecordDate", "PlayerId");
        }

        public int SaveChangesCount { get; private set; }
        public DbSet<ClanStat> ClanStats { get; set; }
        public DbSet<ClanStatsWeekly> ClanStatsWeeklies { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerStat> PlayerStats { get; set; }
        public DbSet<PlayerStatsWeekly> PlayerStatsWeeklies { get; set; }

        public int SaveChanges()
        {
            ++SaveChangesCount;
            return 1;
        }

        public Task<int> SaveChangesAsync()
        {
            ++SaveChangesCount;
            return Task<int>.Factory.StartNew(() => 1);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            ++SaveChangesCount;
            return Task<int>.Factory.StartNew(() => 1, cancellationToken);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public DbChangeTracker ChangeTracker { get; }
        public DbContextConfiguration Configuration { get; }
        public Database Database { get; }

        public DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public DbEntityEntry Entry(object entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DbEntityValidationResult> GetValidationErrors()
        {
            throw new NotImplementedException();
        }

        public DbSet Set(Type entityType)
        {
            throw new NotImplementedException();
        }

        public DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}