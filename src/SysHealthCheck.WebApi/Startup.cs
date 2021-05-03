using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using SysHealthCheck.WebApi.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace SysHealthCheck.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var hc = services.AddHealthChecks();

            #region Private Memory
            var privateMemory = Configuration.GetSection("PrivateMemory").Get<PrivateMemoryHelper>();
            if (privateMemory != null)
            {
                hc.AddPrivateMemoryHealthCheck(1024 * 1024 * privateMemory.Maximum, name: privateMemory.Name, tags: privateMemory.Tags);
            }
            #endregion

            #region Disk Storage
            var diskStorage = Configuration.GetSection("DiskStorage").Get<List<DiskStorageHelper>>();
            if (diskStorage != null && diskStorage.Count > 0)
            {
                foreach (var ds in diskStorage)
                {
                    hc.AddDiskStorageHealthCheck(s => s.AddDrive(ds.Drive, ds.MinFree),
                        name:  ds.Name, tags: ds.Tags);
                }                
            }
            #endregion

            #region Windows Service
            var windowsService = Configuration.GetSection("WindowsService").Get<List<WindowsServiceHelper>>();
            if (windowsService != null && windowsService.Count > 0)
            {
                foreach (var ws in windowsService)
                {
                    hc.AddWindowsServiceHealthCheck(ws.Service, s => s.Status == ServiceControllerStatus.Running, name: ws.Name, tags: ws.Tags);
                }
            }
            #endregion

            #region Network TCP 
            var networkTCP = Configuration.GetSection("Tcp").Get<List<TcpHelper>>();
            if (networkTCP != null && networkTCP.Count > 0)
            {
                foreach (var tcp in networkTCP)
                {
                    hc.AddTcpHealthCheck(s => s.AddHost(tcp.Host, tcp.Port), timeout: TimeSpan.FromSeconds(tcp.Timeout),name:tcp.Name,tags:tcp.Tags);
                }
            }
            #endregion

            #region SqlServer
            var sqlServer = Configuration.GetSection("SqlServer").Get<List<DatabaseHelper>>();
            if (sqlServer != null && sqlServer.Count > 0)
            {
                foreach (var mssql in sqlServer)
                {
                    hc.AddSqlServer(mssql.ConnectString, timeout: TimeSpan.FromSeconds(mssql.Timeout), name: mssql.Name, tags: mssql.Tags);
                }
            }
            #endregion

            #region MongoDb
            var mongoDb = Configuration.GetSection("MongoDb").Get<List<DatabaseHelper>>();
            if (mongoDb != null && mongoDb.Count > 0)
            {
                foreach (var mongo in mongoDb)
                {
                    hc.AddMongoDb(mongo.ConnectString, timeout: TimeSpan.FromSeconds(mongo.Timeout), name: mongo.Name, tags: mongo.Tags);
                }
            }
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
