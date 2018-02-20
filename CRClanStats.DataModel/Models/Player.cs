using System.Collections.Generic;
using JetBrains.Annotations;

namespace CRClanStats.DataModel.Models
{
    [PublicAPI]
    public class Player
    {
        public int PlayerId { get; set; } // PlayerId (Primary key)
        public string Tag { get; set; } // Tag (length: 10)
        public string Name { get; set; } // Name (length: 10)
        public string FormerName { get; set; } // FormerName (length: 10)
        public int JoinCount { get; set; } // JoinCount
        public bool IsCurrentMember { get; set; } // IsCurrentMember

        // Reverse navigation

        /// <summary>
        ///     Child PlayerStats where [PlayerStats].[PlayerId] point to this entity (FK_PlayerStats_ToPlayer)
        /// </summary>
        public virtual ICollection<PlayerStat> PlayerStats { get; set; } =
            new List<PlayerStat>(); // PlayerStats.FK_PlayerStats_ToPlayer

        /// <summary>
        ///     Child PlayerStatsWeeklies where [PlayerStatsWeekly].[PlayerId] point to this entity (FK_PlayerStatsWeekly_ToPlayer)
        /// </summary>
        public virtual ICollection<PlayerStatsWeekly> PlayerStatsWeeklies { get; set; } =
            new List<PlayerStatsWeekly>(); // PlayerStatsWeekly.FK_PlayerStatsWeekly_ToPlayer
    }
}