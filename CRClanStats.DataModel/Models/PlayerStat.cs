using System;
using JetBrains.Annotations;

namespace CRClanStats.DataModel.Models
{
    [PublicAPI]
    public class PlayerStat
    {
        public DateTime RecordDate { get; set; } // RecordDate (Primary key)
        public int PlayerId { get; set; } // PlayerId (Primary key)
        public int Trophies { get; set; } // Trophies
        public int Donations { get; set; } // Donations
        public int DonationsReceived { get; set; } // DonationsReceived
        public int ClanChestCrowns { get; set; } // ClanChestCrowns

        // Foreign keys

        /// <summary>
        ///     Parent Player pointed by [PlayerStats].([PlayerId]) (FK_PlayerStats_ToPlayer)
        /// </summary>
        public virtual Player Player { get; set; } // FK_PlayerStats_ToPlayer
    }
}