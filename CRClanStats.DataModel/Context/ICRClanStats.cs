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
    public interface ICRClanStats : IDisposable
    {
        DbSet<ClanStat> ClanStats { get; set; } // ClanStats
        DbSet<ClanStatsWeekly> ClanStatsWeeklies { get; set; } // ClanStatsWeekly
        DbSet<Player> Players { get; set; } // Player
        DbSet<PlayerStat> PlayerStats { get; set; } // PlayerStats
        DbSet<PlayerStatsWeekly> PlayerStatsWeeklies { get; set; } // PlayerStatsWeekly
        DbChangeTracker ChangeTracker { get; }
        DbContextConfiguration Configuration { get; }
        Database Database { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        DbEntityEntry Entry(object entity);
        IEnumerable<DbEntityValidationResult> GetValidationErrors();
        DbSet Set(Type entityType);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        string ToString();
    }
}