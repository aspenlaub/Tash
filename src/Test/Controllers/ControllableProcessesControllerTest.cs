using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Tash.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Tash.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Test.Controllers {
    [TestClass]
    public class ControllableProcessesControllerTest {
        private const string BaseUrl = "http://localhost:60404/ControllableProcesses";

        [TestMethod]
        public async Task CanGetControllableProcesses() {
            using (var client = ControllerTestHelpers.CreateHttpClient()) {
                var controllableProcesses = await GetControllableProcesses(client);
                Assert.AreEqual(0, controllableProcesses.Count);
            }
        }

        [TestMethod]
        public async Task CanPutControllableProcess() {
            var process = new ControllableProcess {
                ProcessId = 4711,
                Busy = false,
                ConfirmedAt = DateTime.Now,
                LaunchCommand = "StartMeUp.cmd",
                Title = "This is not a title"
            };

            using (var client = ControllerTestHelpers.CreateHttpClient()) {
                var request = new StringContent(JsonConvert.SerializeObject(process), Encoding.UTF8, "application/json");
                request.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await client.PutAsync(BaseUrl, request);
                Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

                var controllableProcesses = await GetControllableProcesses(client);
                Assert.AreEqual(1, controllableProcesses.Count);
                Assert.IsTrue(process.MemberwiseEquals(controllableProcesses[0]));
            }
        }

        private static async Task<List<ControllableProcess>> GetControllableProcesses(HttpClient client) {
            var response = await client.GetAsync(BaseUrl);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var controllableProcesses = JsonConvert.DeserializeObject<ODataResponse<ControllableProcess>>(responseString).Value.ToList();
            return controllableProcesses;
        }
    }
}
