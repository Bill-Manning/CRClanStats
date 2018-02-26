using log4net;


namespace CRClanStats.Service
{
    public class Program
    {
        public static ILog Logger { get; private set; }

        public static void Main(string[] args)
        {
            Logger = LogManager.GetLogger(typeof(Program));
            Logger.Debug("Entered Main");

            ConfigureService.Configure();
        }
    }
}