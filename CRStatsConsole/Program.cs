using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CRAPI;
using CRClanStats.DataModel;
using Player = CRAPI.Player;

namespace CRStatsConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Executing async requests and populating database.");
            DoSomething();
            Console.ReadKey();
            //Console.Clear();


            // Initialize wrapper with your dev key
            //Wrapper wr = new Wrapper("e4f7eb51a7bc43d784adf37b92bdd8661ab84aee65ab42849e5dd2733e94eb8e");

            //string tag = "2C09RR280";

            //Player player = wr.GetPlayer(tag);

            //Console.WriteLine($"{player.name} (XP level {player.stats.level}) ({player.arena.name})");
            //Console.WriteLine($"{player.clan.name} ({player.clan.role})");


            //Card[] cards = player.currentDeck;


            //foreach (Card c in cards)
            //{
            //    Console.WriteLine($"{c.name} {c.elixir} ({c.rarity}) --- level: {c.level}");
            //}

            //Console.WriteLine("Searching for clan with name abc and at least 10 members.");

            // Parameters: name, min score, min members, max members. When is some parameter null, wrapper will not search using the parameter. At least
            // one parameter must not be null.
            //SimplifiedClan[] searchResult = wr.SearchForClans("abc", null, 10, null);

            //Console.WriteLine($"First clan is \"{searchResult[0]}\" with {wr.GetClan(searchResult[0].tag).memberCount} members.");

            //Console.ReadKey();
        }


        private static void DoSomething()
        {
            var wr = new Wrapper("e4f7eb51a7bc43d784adf37b92bdd8661ab84aee65ab42849e5dd2733e94eb8e");

            //string ptag = "2C09RR280";
            var clanTag = "2P082UU9";
            var clan = wr.GetClan(clanTag);

            // Wait untill everything is prepared. You can for example write dots into console
            //while (!clansAsync.IsCompleted)
            //{
            //    Console.Write(".");
            //    Thread.Sleep(50);
            //}

            //var clan = await clansAsync.ConfigureAwait(false);
            var members = clan.members.ToList();

            Console.WriteLine($"\nGot clan data for {clan.name}. Processing players now");

            var membersLookup = members.ToLookup(k => k.tag, v => v);
            var players = wr.GetPlayer(membersLookup?.Select(t => t.Key).ToArray());

            //while (!playersAsync.IsCompleted)
            //{
            //    Console.Write(".");
            //    Thread.Sleep(50);
            //}

            //var playersResult = await playersAsync.ConfigureAwait(false);
            //var players = playersResult.ToList();

            Console.WriteLine($"\nUpdating player stats for {clan.memberCount} members");

            var database = new CRClanStatsFactory().Create();
            
            AddNewMembers(database, players.ToList());
            RemoveMissingMembers(database, members);
            UpdatePlayersStats(database, members);
        }

        private static void UpdatePlayersStats(ICRClanStats database, List<SimplifiedPlayer> members)
        {
            foreach (var member in members)
            {
                var playerRecord = database.Players?.Single(p => p.Tag == member.tag);
                var playerStat = new PlayerStat
                {
                    RecordDate = DateTime.UtcNow,
                    DonationsCount = member.donations ?? 0,
                    DonationsReceivedCount = member.donationsReceived ?? 0,
                    PlayerRole = database.PlayerRoles?.Single(r => r.RoleName == member.role),
                    ClanChestCrownsCount = member.clanChestCrowns ?? 0,
                    Trophies = member.trophies
                };

                playerRecord.PlayerStats.Add(playerStat);

                Console.WriteLine($"\nSome stats for {member.name}");
                Console.WriteLine($"      Tag:                        {member.tag}");
                Console.WriteLine($"      Role:                       {member.role}");
                Console.WriteLine($"      Trophies:                   {member.trophies}");
                Console.WriteLine($"      Arena:                      {member.arena.name}");
                Console.WriteLine($"      Current Donations:          {member.donations}");
                Console.WriteLine($"      Current Donations Delta:    {member.donationsDelta}");
                Console.WriteLine($"      Current Donations Received: {member.donationsReceived}");

                //Console.WriteLine($"      Total Games: {player.games.total}");
                //Console.WriteLine($"      Total Donations: {player.stats.totalDonations}");
            }

            database.SaveChanges();
        }

        private static void AddNewMembers(ICRClanStats database, List<Player> players)
        {
            int totalNewMembersAdded = 0;

            foreach (var player in players)
            {
                var playerRecord = database.Players?.FirstOrDefault(p => p.Tag == player.tag);

                if (playerRecord == null)
                {
                    playerRecord = new CRClanStats.DataModel.Player
                    {
                        Tag = player.tag,
                        Name = player.name
                    };

                    database.Players.Add(playerRecord);

                    Console.WriteLine($"\nAdding new member: {player.name}");

                    totalNewMembersAdded++;
                }
            }

            database.SaveChanges();
            
            Console.WriteLine($"\nAdded {totalNewMembersAdded} new members");
        }


        private static void RemoveMissingMembers(ICRClanStats database, List<SimplifiedPlayer> members)
        {
            var totalMissingMembers = 0;
            var currentMemberPlayerRecords = database.Players?.Where(p => p.IsCurrentMember).ToList();

            foreach (var playerRecord in currentMemberPlayerRecords)
            {
                var memberTags = members?.Select(m => m.tag);

                if (memberTags != null && !memberTags.Contains(playerRecord.Tag))
                {
                    playerRecord.IsCurrentMember = false;

                    Console.WriteLine($"\nFlagging missing member: {playerRecord.Name}");

                    totalMissingMembers++;
                }
            }

            database.SaveChanges();

            Console.WriteLine($"\nFlaged {totalMissingMembers} missing members");

        }

        private static void SyncPlayerBattles(Player player)
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
    }
}