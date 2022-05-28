using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Dvin.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace Aspenlaub.Net.GitHub.CSharp.Tash;

public static class Program {
    public static async Task Main(string[] args) {
        var builder = await CreateWebHostBuilderAsync(args);
        builder.RunHost(args);
    }

    public static async Task<IWebHostBuilder> CreateWebHostBuilderAsync(string[] args) {
        // ReSharper disable once RedundantAssignment
        var release = true;
#if DEBUG
        release = false;
#endif
        return
            (await WebHost.CreateDefaultBuilder(args).UseDvinAndPeghAsync(Constants.TashAppId, release, args))
            .UseStartup<Startup>();
    }
}