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

    public class ClanStatsWeekly
    {
        public System.DateTime RecordDate { get; set; }
        public int TotalTrophies { get; set; }
        public int TotalDonations { get; set; }
        public int ClanMemberCount { get; set; }
        public int ClanChestCrownsCount { get; set; }
        public int ClanChestsCompletedCount { get; set; }
        public int ClanChestNonParticipationCount { get; set; }
        public int ClanChestFullParticipationCount { get; set; }
        public int TopDonationsPlayer { get; set; }
        public int TopCrownsPlayer { get; set; }


        public virtual Player Player_TopCrownsPlayer { get; set; }

        public virtual Player Player_TopDonationsPlayer { get; set; }
    }

}
// </auto-generated>
