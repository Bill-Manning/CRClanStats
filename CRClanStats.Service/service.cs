using System;
using System.ComponentModel;
using System.Configuration;
using System.Threading;
using log4net;

namespace CRClanStats.Service
{
    public class Service
    {
        private static ILog Logger => LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private BackgroundWorker backgroundWorker;
        private DateTime weeklyResetTimeUTC;
        private DayOfWeek weeklyResetDay;
        private DateTime weeklyResetDateTimeUTC;
        private Timer weeklyResetTimer;
        private Timer shortTimer;

        private DateTime NextScheduledPullTime { get; set; }


        public Service()
        {
            Logger.Debug("New instance of Service");
        }

        public void Start()
        {
            Logger.Info("Starting up");

            //Start the actual job in a separate thread to avoid timeouts on start
            backgroundWorker = new BackgroundWorker {WorkerReportsProgress = false};
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerAsync();
        }

        public void Stop()
        {
            Logger.Info("Shutting down");

            backgroundWorker.Dispose();
            weeklyResetTimer.Dispose();
            shortTimer.Dispose();

            LogManager.Shutdown();
        }


        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Logger.Info("Starting BackgroundWorker thread ");

            StartWeeklyStatsRollUpTimer();
            StartScheduledPullTimer();
        }


        private int GetDaysUntilNextDesiredDayOfWeekUTC(DayOfWeek desiredDayOfWeek)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            return ((int)desiredDayOfWeek - (int)DateTime.UtcNow.DayOfWeek + 7) % 7;
        }

        private void StartWeeklyStatsRollUpTimer()
        {
            //calcuale the delay until the point of starting time
            weeklyResetTimeUTC = DateTime.Parse(ConfigurationManager.AppSettings["WeeklyResetTimeUTC"]);
            weeklyResetDay = (DayOfWeek) Enum.Parse(typeof (DayOfWeek), ConfigurationManager.AppSettings["WeeklyResetDayUTC"]);

            weeklyResetDateTimeUTC = weeklyResetTimeUTC.AddDays(GetDaysUntilNextDesiredDayOfWeekUTC(weeklyResetDay));

            var currentDateTimeUTC = DateTime.UtcNow;
            var currentLocalDateTime = DateTime.Now;


            if (weeklyResetTimeUTC <= currentDateTimeUTC)
            {
                weeklyResetDateTimeUTC = weeklyResetDateTimeUTC.AddDays(1);
            }

            var timespanUntilReset = weeklyResetDateTimeUTC.Subtract(currentDateTimeUTC);
            
            weeklyResetTimer = new Timer(WeeklyRollUpTimer_Elapsed, null, timespanUntilReset, new TimeSpan(0, 0, 0));

            Logger.Info($"First weekly rollup scheduled for {currentLocalDateTime.Add(timespanUntilReset)}");
        }


        private void WeeklyRollUpTimer_Elapsed(object state)
        {
            Logger.Info("Processing weekly rollup}");

            weeklyResetDateTimeUTC = weeklyResetDateTimeUTC.AddDays(7);
            var timespanUntilReset = weeklyResetDateTimeUTC.Subtract(DateTime.UtcNow);
            var currentLocalDateTime = DateTime.Now;

            weeklyResetTimer.Change(timespanUntilReset, Timeout.InfiniteTimeSpan);

            RecordWeeklyClanStats();
            RecordWeeklyMemberStats();

            Logger.Info($"Next weekly rollup scheduled for {currentLocalDateTime.Add(timespanUntilReset)}");
        }

        private void StartScheduledPullTimer()
        {
            Logger.Info("Processing scheduled pull");

            var currentTime = DateTime.Now;
            var scheduledIntervalMinutes = int.Parse(ConfigurationManager.AppSettings["ScheduledPullIntervalInMinutes"]);
            var minutesToAdd = scheduledIntervalMinutes - (currentTime.Minute + scheduledIntervalMinutes) % scheduledIntervalMinutes;
            var currentTimeToTheMinute =  new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, 0);
            NextScheduledPullTime = currentTimeToTheMinute.AddMinutes(minutesToAdd);
            var timespanUntilNextScheduledPull = NextScheduledPullTime - currentTime;
            shortTimer = new Timer(ScheduledTimer_Elapsed, null, timespanUntilNextScheduledPull, new TimeSpan(0, 0, 0));

            Logger.Info($"First scheduled pull at {currentTime.Add(timespanUntilNextScheduledPull).TimeOfDay}");
        }



        private void ScheduledTimer_Elapsed(object state)
        {
            Logger.Info("Processing scheduled pull}");

            var currentTime = DateTime.Now;
            var scheduledIntervalMinutes = int.Parse(ConfigurationManager.AppSettings["ScheduledPullIntervalInMinutes"]);
            NextScheduledPullTime = NextScheduledPullTime.AddMinutes(scheduledIntervalMinutes);
            var timespanUntilNextScheduledPull = NextScheduledPullTime - currentTime;

            shortTimer.Change(timespanUntilNextScheduledPull, Timeout.InfiniteTimeSpan);

            GatherClanStats();

            Logger.Info($"Next scheduled pull at {currentTime.Add(timespanUntilNextScheduledPull).TimeOfDay}");
        }

        private void RecordDailyMemberStats()
        {
            var processor = new StatsProcessor();
            processor.RecordDailyMemberStats();;
        }

        private void RecordDailyClanStats()
        {
            var processor = new StatsProcessor();
            processor.RecordDailyClanStats();
        }

        private void RecordWeeklyMemberStats()
        {
            var processor = new StatsProcessor();
            processor.RecordWeeklyMemberStats();
        }

        private void RecordWeeklyClanStats()
        {
            var processor = new StatsProcessor();
            processor.RecordWeeklyClanStats();
        }

        private void RecordClanChestStats()
        {
            var processor = new StatsProcessor();
            processor.RecordClanChestStats();

        }

        private void GatherClanStats()
        {
            Logger.Info("Pulling clan stats data");

            var processor = new StatsProcessor();
            processor.GatherClanStats();
        }
    }
}