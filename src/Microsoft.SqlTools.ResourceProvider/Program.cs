//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

#nullable disable

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.SqlTools.ServiceLayer.SqlContext;
using Microsoft.SqlTools.Utility;

namespace Microsoft.SqlTools.ResourceProvider
{
    /// <summary>
    /// Main application class for the executable that supports the resource provider and identity services
    /// </summary>
    internal sealed class Program
    {
        private const string ServiceName = "SqlToolsResourceProviderService.exe";

        /// <summary>
        /// Main entry point into the Credentials Service Host
        /// </summary>
        internal static async Task Main(string[] args)
        {
            try
            {
                // read command-line arguments
                CommandOptions commandOptions = new CommandOptions(args, ServiceName);
                if (commandOptions.ShouldExit)
                {
                    return;
                }

                string logFilePath = commandOptions.LogFilePath;
                if (string.IsNullOrWhiteSpace(logFilePath))
                {
                    logFilePath = Logger.GenerateLogFilePath("SqlToolsResourceProviderService");
                }

                // we need to switch to Information when preparing for public preview
                Logger.Initialize(tracingLevel: commandOptions.TracingLevel, commandOptions.PiiLogging, logFilePath: logFilePath, traceSource: "resourceprovider", commandOptions.AutoFlushLog);
                Logger.Write(TraceEventType.Information, "Starting SqlTools Resource Provider");

                // set up the host details and profile paths 
                var hostDetails = new HostDetails(
                    name: "SqlTools Resource Provider",
                    profileId: "Microsoft.SqlTools.ResourceProvider",
                    version: new Version(1, 0));

                SqlToolsContext sqlToolsContext = new SqlToolsContext(hostDetails);
                UtilityServiceHost serviceHost = ResourceProviderHostLoader.CreateAndStartServiceHost(sqlToolsContext);

                await serviceHost.WaitForExitAsync();
            }
            catch (Exception e)
            {
                Logger.WriteWithCallstack(TraceEventType.Critical, $"An unhandled exception occurred: {e}");
                Environment.Exit(1);
            }
            finally
            {
                Logger.Close();
            }
        }
    }
}
