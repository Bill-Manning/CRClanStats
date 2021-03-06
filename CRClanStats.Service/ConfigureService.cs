﻿using log4net;
using Topshelf;

namespace CRClanStats.Service
{
    public static class ConfigureService
    {
        //private static ILog Logger => LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public static void Configure()
        {
            
           // Logger.Debug("Entered Configure");

            HostFactory.Run(configure =>
            {
                configure.Service<Service>(service =>
                {
                    service.ConstructUsing(s => new Service());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });

                //Setup Account that window service use to run.  
                configure.RunAsNetworkService();
                configure.SetServiceName("CRClanStats");
                configure.SetDisplayName("CRClanStats");
                configure.SetDescription(
                    "Clash Royale Clan Stats Service - Gathers statistics for configured clan(s) on a scheduled basis.");
            });
        }
    }
}