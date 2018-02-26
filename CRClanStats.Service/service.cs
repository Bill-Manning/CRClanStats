using System;
using System.ComponentModel;
using System.Configuration;
using System.Threading;
using log4net;

namespace CRClanStats.Service
{
    public class Service
    {
        private BackgroundWorker backgroundWorker;
        private DateTime dailyPullTimeUTC;
        private Timer dailyTimer;
        private Timer shortTimer;

        public ILog Logger { get; private set; }

        public Service()
        {
            Logger = LogManager.GetLogger(typeof(Service));
            Logger.Debug("New instance of Service");
        }

        public void Start()
        {
            Logger.Info("Service.Start()");

            //Start the actual job in a separate thread to avoid timeouts on start
            backgroundWorker = new BackgroundWorker {WorkerReportsProgress = false};
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerAsync();
        }

        public void Stop()
        {
            Logger.Info("Service.Stop()");

            backgroundWorker.Dispose();
            dailyTimer.Dispose();
            shortTimer.Dispose();

            LogManager.Shutdown();
        }


        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Logger.Info("Starting BackgroundWorker thread ");

            // get stats once initially then let the scheduled events take over
            GatherClanStats();

            StartDailyPullTimer();
            StartScheduledPullTimer();
        }


        private void StartDailyPullTimer()
        {
            //calcuale the delay until the point of starting time
            dailyPullTimeUTC = DateTime.Parse(ConfigurationManager.AppSettings["DailyPullTimeUTC"]);
            var currentTimeUTC = DateTime.UtcNow;
            TimeSpan timespanUntilDailyPull;

            if (dailyPullTimeUTC <= currentTimeUTC)
            {
                dailyPullTimeUTC = dailyPullTimeUTC.AddDays(1);
            }

            timespanUntilDailyPull = dailyPullTimeUTC.Subtract(currentTimeUTC);


            dailyTimer = new Timer(DailyTimer_Elapsed, null, timespanUntilDailyPull, new TimeSpan(0, 0, 0));

            Logger.Info($"First Daily Pull in {timespanUntilDailyPull}");
        }


        private void DailyTimer_Elapsed(object state)
        {
            Logger.Info("Starting Daily Pull}");

            dailyPullTimeUTC = dailyPullTimeUTC.AddDays(1);
            var timespanUntilDailyPull = dailyPullTimeUTC.Subtract(DateTime.UtcNow);

            dailyTimer.Change(timespanUntilDailyPull, Timeout.InfiniteTimeSpan);

            GatherClanStats();

            Logger.Info($"Next Scheduled Pull in {timespanUntilDailyPull}");
        }

        private void StartScheduledPullTimer()
        {
            var timerInterval = int.Parse(ConfigurationManager.AppSettings["ScheduledPullIntervalInMinutes"]) * 1000 * 60;
            shortTimer = new Timer(ScheduledTimer_Elapsed, null, timerInterval, Timeout.Infinite);

            Logger.Info($"First Scheduled Pull in {timerInterval} minutes");
        }



        private void ScheduledTimer_Elapsed(object state)
        {
            Logger.Info("Starting Scheduled Pull}");
            
            var timerInterval = int.Parse(ConfigurationManager.AppSettings["ScheduledPullIntervalInMinutes"]) * 1000 * 60;
            shortTimer.Change(timerInterval, Timeout.Infinite);

            GatherClanStats();

            Logger.Info($"Next Scheduled Pull in {timerInterval} minutes");
        }

        private void GatherClanStats()
        {
            Logger.Info("Pulling clan stats data");

            var processor = new StatsProcessor();
            processor.GatherClanStats();
        }
    }
}