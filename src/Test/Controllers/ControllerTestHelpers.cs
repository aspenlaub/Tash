using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Test.Controllers;

public static class ControllerTestHelpers {
    public static HttpClient CreateHttpClient() {
        IWebHostBuilder builder = new WebHostBuilder().UseStartup<Startup>();
        var server = new TestServer(builder);
        return server.CreateClient();
    }
}