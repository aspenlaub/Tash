using System.Net.Http;
using Microsoft.AspNetCore.TestHost;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Test.Controllers {
    public class ControllerTestHelpers {
        public static HttpClient CreateHttpClient() {
            var builder = Program.CreateWebHostBuilder(new string[] { });
            var server = new TestServer(builder);
            return server.CreateClient();
        }
    }
}
