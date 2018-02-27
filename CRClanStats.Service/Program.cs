using log4net;


namespace CRClanStats.Service
{
    public class Program
    {
        private static ILog Logger => LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public static void Main(string[] args)
        {
            Logger.Debug("Entered Main");

            ConfigureService.Configure();
        }
    }
}