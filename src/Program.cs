using Aspenlaub.Net.GitHub.CSharp.Dvin.Components;
using Aspenlaub.Net.GitHub.CSharp.Tash.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Aspenlaub.Net.GitHub.CSharp.Tash;

public class Program {
    public static void Main(string[] args) {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) {
        return Host.CreateDefaultBuilder(args)
           .ConfigureWebHostDefaults(builder => {
               builder.ConfigureUrl(Constants.TashApplicationName, Constants.TashAppId);
               builder.ConfigureServices(Configurator.ConfigureServices);
               builder.Configure(Configurator.Configure);
           });
    }
}