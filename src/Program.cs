using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Dvin.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace Aspenlaub.Net.GitHub.CSharp.Tash;

public static class Program {
    public static async Task Main(string[] args) {
        // ReSharper disable once RedundantAssignment
        bool release = true;
        Exception webHostBuilderException = null;
#if DEBUG
        release = false;
#endif
        IHostBuilder hostBuilder = new HostBuilder()
            .ConfigureWebHost(
                async void (webHostBuilder) => {
                    try {
                        (await webHostBuilder
                                .UseDvinAndPeghAsync("Tash", Constants.TashAppId, release, args))
                            .UseStartup<Startup>();
                    } catch (Exception e) {
                        webHostBuilderException = e;
                    }
                });
        if (webHostBuilderException != null) {
            throw webHostBuilderException;
        }
        await hostBuilder.Build().StartAsync();
    }
}