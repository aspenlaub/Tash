using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Test.Controllers;

public static class ControllerTestHelpers {
    public static async Task<HttpClient> CreateHttpClientAsync() {
        IHostBuilder hostBuilder = new HostBuilder()
            .ConfigureWebHost(
                webHostBuilder =>
                    webHostBuilder
                        .UseTestServer()
                        .UseStartup<Startup>());
        IHost host = hostBuilder.Build();
        await host.StartAsync();
        TestServer server = host.GetTestServer();
        return server.CreateClient();
    }
}