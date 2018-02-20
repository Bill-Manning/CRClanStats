using System;
using JetBrains.Annotations;

namespace CRClanStats.DataModel.Models
{
    [PublicAPI]
    public class ClanStat
    {
        public DateTime RecordDate { get; set; } // (Primary key)
        public int Trophies { get; set; }
        public int Donations { get; set; }
        public int ClanChestCrowns { get; set; }
        public int ClanChestsCompleted { get; set; }
    }
}