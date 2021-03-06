﻿using System;
using System.Collections.Generic;
using System.Linq;
using CRClanStats.DataModel;
using CRAPI;
using log4net;
using Player = CRAPI.Player;

namespace CRClanStats.Service
{
    public class StatsProcessor
    {
        private static ILog Logger => LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string ApiKey = "e4f7eb51a7bc43d784adf37b92bdd8661ab84aee65ab42849e5dd2733e94eb8e";
        private const string ClanTag = "2P082UU9";

        public StatsProcessor()
        {
            Logger.Debug("New instance of StatsProcessor");
        }

        public void GatherClanStats()
        {
            Logger.Debug("Begin GatherClanStats");

            var clan = GetClan();
            var members = clan.members?.ToList();
            var database = new CRClanStatsFactory().Create();

            AddNewMembers(database, members);
            RemoveMissingMembers(database, members);
            UpdatePlayersStats(database, members);

            Logger.Debug("End GatherClanStats");
        }

        private Clan GetClan()
        {
            var wr = new Wrapper(ApiKey);
            var clan = wr.GetClan(ClanTag);

            return clan;
        }

        private IList<Player> GetPlayers(IList<SimplifiedPlayer> members)
        {
            var wr = new Wrapper(ApiKey);
            var membersLookup = members?.ToLookup(k => k.tag, v => v);
            var players = wr.GetPlayer(membersLookup?.Select(t => t.Key).ToArray());

            return players;
        }

        private void UpdatePlayersStats(ICRClanStats database, List<SimplifiedPlayer> members)
        {
            foreach (var member in members)
            {
                Logger.Debug($"Updating stats for {member.name}");
                Logger.Debug($"      Tag:                        {member.tag}");
                Logger.Debug($"      Role:                       {member.role}");
                Logger.Debug($"      Trophies:                   {member.trophies}");
                Logger.Debug($"      Arena:                      {member.arena.name}");
                Logger.Debug($"      Current Donations:          {member.donations}");
                Logger.Debug($"      Current Donations Delta:    {member.donationsDelta}");
                Logger.Debug($"      Current Donations Received: {member.donationsReceived}");

                try
                {
                    var playerRecord = database.Players?.Single(p => p.Tag == member.tag);
                    var playerStat = new PlayerStat
                    {
                        RecordDate = DateTime.UtcNow,
                        DonationsCount = member.donations ?? 0,
                        DonationsReceivedCount = member.donationsReceived ?? 0,
                        //DonationsDelta = member.donationsDelta ?? 0,
                        PlayerRole = database.PlayerRoles?.Single(r => r.RoleName == member.role),
                        ClanChestCrownsCount = member.clanChestCrowns ?? 0,
                        Trophies = member.trophies
                    };

                    playerRecord.PlayerStats.Add(playerStat);
                }
                catch (Exception ex)
                {
                    Logger.Error("Update failed",ex);
                    throw;
                }
            }

            database.SaveChanges();

            Logger.Info($"Updated stats for {members.Count} clan members");
        }

        private void AddNewMembers(ICRClanStats database, List<SimplifiedPlayer> members)
        {
            var totalNewMembersAdded = 0;

            foreach (var member in members)
            {
                var playerRecord = database.Players?.FirstOrDefault(p => p.Tag == member.tag);

                if (playerRecord == null)
                {
                    playerRecord = new DataModel.Player
                    {
                        Tag = member.tag,
                        Name = member.name
                    };

                    database.Players.Add(playerRecord);
                    totalNewMembersAdded++;

                    Logger.Info($"\nAdding new member: {member.name}");
                }
            }

            database.SaveChanges();

            Logger.Info($"\nAdded {totalNewMembersAdded} new members");
        }

        private void RemoveMissingMembers(ICRClanStats database, List<SimplifiedPlayer> members)
        {
            var totalMissingMembers = 0;
            var currentMemberPlayerRecords = database.Players?.Where(p => p.IsCurrentMember).ToList();

            foreach (var playerRecord in currentMemberPlayerRecords)
            {
                var memberTags = members?.Select(m => m.tag);

                if (memberTags != null && !memberTags.Contains(playerRecord.Tag))
                {
                    playerRecord.IsCurrentMember = false;
                    totalMissingMembers++;

                    Logger.Info($"\nFlagging missing member: {playerRecord.Name}");
                }
            }

            database.SaveChanges();

            Logger.Info($"\nFound {totalMissingMembers} missing members");
        }

        private void SyncPlayerBattles(Player player)
        {
            foreach (var battle in player?.battles)
            {
                Console.WriteLine($"          Crowns: {battle?.teamCrowns}");
                Console.WriteLine($"          Time: {battle?.utcTime}");
                Console.WriteLine($"          Mode: {battle?.mode.name}");
                Console.WriteLine($"          Type: {battle?.type}");
                Console.WriteLine($"          Winner (int???): {battle?.winner}\n");
            }
        }

        public void RecordWeeklyClanStats()
        {
            Logger.Debug("Begin RecordWeeklyStats");

            var clan = GetClan();
            var members = clan.members?.ToList();
            var database = new CRClanStatsFactory().Create();

            foreach (var member in members)
            {
                Logger.Debug($"Updating stats for {member.name}");
                Logger.Debug($"      Clan Donations:              {clan.donations}");
                Logger.Debug($"      Member Count:                {clan.memberCount}");
                Logger.Debug($"      Missing Members Count:       {clan.memberCount}");
                Logger.Debug($"      New Members Count:           {clan.memberCount}");
                Logger.Debug($"      Active Members Count:        {clan.members.Length}");
                Logger.Debug($"      Total Clan Battles:          {clan.clanChest.crowns}");
                Logger.Debug($"      Total Clan Friendly Battles: {member.donationsReceived}");
                Logger.Debug($"      Most Friendly Player:        {member.donationsReceived}");
                Logger.Debug($"      Biggest Ladder Winner:       {member.donationsReceived}");
                Logger.Debug($"      Biggest Ladder Loser:        {member.donationsReceived}");

                try
                {
                    var playerRecord = database.Players?.Single(p => p.Tag == member.tag);
                    var playerStat = new PlayerStat()
                    {
                        RecordDate = DateTime.UtcNow,
                        
                        DonationsCount = member.donations ?? 0,
                        DonationsReceivedCount = member.donationsReceived ?? 0,
                        //DonationsDelta = member.donationsDelta ?? 0,
                        PlayerRole = database.PlayerRoles?.Single(r => r.RoleName == member.role),
                        ClanChestCrownsCount = member.clanChestCrowns ?? 0,
                        Trophies = member.trophies
                    };

                    playerRecord.PlayerStats.Add(playerStat);
                }
                catch (Exception ex)
                {
                    Logger.Error("Update failed", ex);
                    throw;
                }
            }

            database.SaveChanges();

            Logger.Info($"Updated stats for {members.Count} clan members");
            Logger.Debug("End RecordWeeklyStats");
        }

        public void RecordWeeklyMemberStats()
        {
            Logger.Debug("Begin RecordWeeklyMemberStats");

            var clan = GetClan();
            var members = clan.members?.ToList();
            var database = new CRClanStatsFactory().Create();

            foreach (var member in members)
            {
                Logger.Debug($"Recording stats for {member.name}");
               
                try
                {
                    var playerRecord = database.Players?.Single(p => p.Tag == member.tag);
                    var playerStat = new PlayerStat()
                    {
                        RecordDate = DateTime.UtcNow,
                        DonationsCount = member.donations ?? 0,
                        DonationsReceivedCount = member.donationsReceived ?? 0,
                        //DonationsDelta = member.donationsDelta ?? 0,
                        PlayerRole = database.PlayerRoles?.Single(r => r.RoleName == member.role),
                        ClanChestCrownsCount = member.clanChestCrowns ?? 0,
                        Trophies = member.trophies
                    };

                    playerRecord.PlayerStats.Add(playerStat);
                }
                catch (Exception ex)
                {
                    Logger.Error("Update failed", ex);
                    throw;
                }
            }

            database.SaveChanges();

            Logger.Info($"Recorded weekly stats for {members.Count} clan members");

            Logger.Debug("End RecordWeeklyMemberStats");
        }

        public void RecordClanChestStats()
        {
            Logger.Debug("Begin RecordClanChestStats");

            var clan = GetClan();
            var members = clan.members?.ToList();
            var database = new CRClanStatsFactory().Create();

            foreach (var member in members)
            {
                Logger.Debug($"Updating stats for {member.name}");
                Logger.Debug($"      Crowns:                        {clan.clanChest.crowns}");
                Logger.Debug($"      Role:                       {clan.clanChest.crowns}");
                Logger.Debug($"      Trophies:                   {clan.clanChest.crowns}");
                Logger.Debug($"      Arena:                      {clan.clanChest.crowns}");
                Logger.Debug($"      Current Donations:          {clan.clanChest.crowns}");
                Logger.Debug($"      Current Donations Delta:    {clan.clanChest.crowns}");
                Logger.Debug($"      Current Donations Received: {member.donationsReceived}");

                try
                {
                    var playerRecord = database.Players?.Single(p => p.Tag == member.tag);
                    var playerStat = new PlayerStat()
                    {
                        RecordDate = DateTime.UtcNow,

                        DonationsCount = member.donations ?? 0,
                        DonationsReceivedCount = member.donationsReceived ?? 0,
                        //DonationsDelta = member.donationsDelta ?? 0,
                        PlayerRole = database.PlayerRoles?.Single(r => r.RoleName == member.role),
                        ClanChestCrownsCount = member.clanChestCrowns ?? 0,
                        Trophies = member.trophies
                    };

                    playerRecord.PlayerStats.Add(playerStat);
                }
                catch (Exception ex)
                {
                    Logger.Error("Update failed", ex);
                    throw;
                }
            }

            database.SaveChanges();


            Logger.Debug("End RecordClanChestStats");
        }

        public void RecordDailyClanStats()
        {
            
        }

        public void RecordDailyMemberStats()
        {
            
        }
    }
}