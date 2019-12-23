using Aspenlaub.Net.GitHub.CSharp.DvinCore.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Aspenlaub.Net.GitHub.CSharp.Tash {
    public class Program {
        public static void Main(string[] args) {
            var builder = CreateWebHostBuilder(args);
            builder.RunHost(args);
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
#if DEBUG
                .UseDvinAndPegh(Constants.TashAppId, false, args)
#else
                .UseDvinAndPegh(Constants.TashAppId, true, args)
#endif
                .UseStartup<Startup>();
    }
}
