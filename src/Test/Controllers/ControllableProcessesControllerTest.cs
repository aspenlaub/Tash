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
            var process = CreateTestProcess();

            using (var client = ControllerTestHelpers.CreateHttpClient()) {
                var response = await PutControllableProcess(process, client);
                Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

                var controllableProcesses = await GetControllableProcesses(client);
                Assert.AreEqual(1, controllableProcesses.Count);
                Assert.IsTrue(process.MemberwiseEquals(controllableProcesses[0]));
            }
        }

        [TestMethod]
        public async Task CanGetControllableProcess() {
            var process = CreateTestProcess();

            using (var client = ControllerTestHelpers.CreateHttpClient()) {
                var response = await PutControllableProcess(process, client);
                Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

                var controllableProcess = await GetControllableProcess(client, process.ProcessId);
                Assert.IsNotNull(controllableProcess);
                Assert.IsTrue(process.MemberwiseEquals(controllableProcess));
            }
        }

        [TestMethod]
        public async Task CanConfirmControllableProcess() {
            var process = CreateTestProcess();

            using (var client = ControllerTestHelpers.CreateHttpClient()) {
                var response = await PutControllableProcess(process, client);
                Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

                var confirmation = new ControllableProcessConfirmation {
                    Busy = true,
                    ConfirmedAt = DateTime.Now.AddSeconds(47)
                };

                response = await PatchControllableProcess(process.ProcessId, confirmation, client);
                Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

                var controllableProcess = await GetControllableProcess(client, process.ProcessId);
                Assert.IsNotNull(controllableProcess);
                Assert.IsFalse(process.MemberwiseEquals(controllableProcess));
                process.Busy = confirmation.Busy;
                process.ConfirmedAt = confirmation.ConfirmedAt;
                Assert.IsTrue(process.MemberwiseEquals(controllableProcess));
            }
        }

        private static async Task<HttpResponseMessage> PutControllableProcess(ControllableProcess process, HttpClient client) {
            var request = new StringContent(JsonConvert.SerializeObject(process), Encoding.UTF8, "application/json");
            request.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PutAsync(BaseUrl, request);
            return response;
        }

        private static async Task<HttpResponseMessage> PatchControllableProcess(int processId, ControllableProcessConfirmation confirmation, HttpClient client) {
            var request = new StringContent(JsonConvert.SerializeObject(confirmation), Encoding.UTF8, "application/json");
            request.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PatchAsync($"{BaseUrl}({processId})", request);
            return response;
        }

        private static ControllableProcess CreateTestProcess() {
            var process = new ControllableProcess {
                ProcessId = 4711,
                Busy = false,
                ConfirmedAt = DateTime.Now,
                LaunchCommand = "StartMeUp.cmd",
                Title = "This is not a title"
            };
            return process;
        }

        private static async Task<List<ControllableProcess>> GetControllableProcesses(HttpClient client) {
            var response = await client.GetAsync(BaseUrl);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var controllableProcesses = JsonConvert.DeserializeObject<ODataResponse<ControllableProcess>>(responseString).Value.ToList();
            return controllableProcesses;
        }

        private static async Task<ControllableProcess> GetControllableProcess(HttpClient client, int processId) {
            var response = await client.GetAsync($"{BaseUrl}({processId})");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var controllableProcess = JsonConvert.DeserializeObject<ControllableProcess>(responseString);
            return controllableProcess;
        }
    }
}
