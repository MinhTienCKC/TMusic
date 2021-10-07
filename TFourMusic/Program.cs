using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFourMusic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //using (ServerManager serverManager = new ServerManager())
            //{
            //    Configuration config = serverManager.GetWebConfiguration("Default Web Site");
            //    Microsoft.Web.Administration.ConfigurationSection requestFilteringSection = config.GetSection("system.webServer/security/requestFiltering");
            //    ConfigurationElement requestLimitsElement = requestFilteringSection.GetChildElement("requestLimits");
            //    ConfigurationElementCollection headerLimitsCollection = requestLimitsElement.GetCollection("headerLimits");

            //    ConfigurationElement addElement = headerLimitsCollection.CreateElement("add");
            //    addElement["header"] = @"Content-type";
            //    addElement["sizeLimit"] = 100;
            //    headerLimitsCollection.Add(addElement);

            //    serverManager.CommitChanges();
            //}
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("../TFourMusic/tfourmusic-1e3ff-firebase-adminsdk-b8byd-fb95ff4dbc.json"),
            });
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
