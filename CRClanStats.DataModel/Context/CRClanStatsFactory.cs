using System.Data.Entity.Infrastructure;
using JetBrains.Annotations;

namespace CRClanStats.DataModel.Context
{
    [PublicAPI]
    public class CRClanStatsFactory : IDbContextFactory<CRClanStats>
    {
        public CRClanStats Create()
        {
            return new CRClanStats();
        }
    }
}