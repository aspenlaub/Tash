using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Dvin.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace Aspenlaub.Net.GitHub.CSharp.Tash;

public static class Program {
    public static async Task Main(string[] args) {
        IWebHostBuilder builder = await CreateWebHostBuilderAsync(args);
        builder.RunHost();
    }

    public static async Task<IWebHostBuilder> CreateWebHostBuilderAsync(string[] args) {
        // ReSharper disable once RedundantAssignment
        return
            (await WebHost.CreateDefaultBuilder(args).UseDvinAndPeghAsync("Tash", Constants.TashAppId))
            .UseStartup<Startup>();
    }
}