﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace raupjc_projekt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog((contex, logger) =>
                {
                    var connectionString = contex.Configuration.GetConnectionString("DefaultConnection");

                    logger.MinimumLevel.Error().Enrich.FromLogContext()
                        .WriteTo.MSSqlServer(
                            connectionString: connectionString,
                            tableName: "Errors",
                            autoCreateSqlTable: true);
                })
                .Build();
    }
}
