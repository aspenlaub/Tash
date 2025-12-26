using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Tash.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Tash.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControllableProcess = Aspenlaub.Net.GitHub.CSharp.Tash.Model.ControllableProcess;

[assembly: DoNotParallelize]
namespace Aspenlaub.Net.GitHub.CSharp.Tash.Test.Controllers;

[TestClass]
public class ControllableProcessesControllerTest {
    private const string _baseUrl = "http://localhost:60404/ControllableProcesses";

    [TestMethod]
    public async Task CanGetControllableProcesses() {
        using HttpClient client = ControllerTestHelpers.CreateHttpClient();
        List<ControllableProcess> controllableProcesses = await GetControllableProcesses(client);
        Assert.IsEmpty(controllableProcesses);
    }

    [TestMethod]
    public async Task CanPutControllableProcess() {
        ControllableProcess process = CreateTestProcess();

        using HttpClient client = ControllerTestHelpers.CreateHttpClient();
        HttpResponseMessage response = await PutControllableProcess(client, process);
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

        List<ControllableProcess> controllableProcesses = await GetControllableProcesses(client);
        Assert.HasCount(1, controllableProcesses);
        Assert.IsTrue(process.MemberwiseEquals(controllableProcesses[0]));
    }

    [TestMethod]
    public async Task CanGetControllableProcess() {
        ControllableProcess process = CreateTestProcess();

        using HttpClient client = ControllerTestHelpers.CreateHttpClient();
        HttpResponseMessage response = await PutControllableProcess(client, process);
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

        ControllableProcess controllableProcess = await GetControllableProcess(client, process.ProcessId);
        Assert.IsNotNull(controllableProcess);
        Assert.IsTrue(process.MemberwiseEquals(controllableProcess));
    }

    [TestMethod]
    public async Task CanConfirmControllableProcess() {
        ControllableProcess process = CreateTestProcess();

        using HttpClient client = ControllerTestHelpers.CreateHttpClient();
        HttpResponseMessage response = await PutControllableProcess(client, process);
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

        var confirmation = new ControllableProcessConfirmation {
            Status = ControllableProcessStatus.Busy,
            ConfirmedAt = DateTime.Now.AddSeconds(47)
        };

        response = await PatchControllableProcess(client, process.ProcessId, confirmation);
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

        ControllableProcess controllableProcess = await GetControllableProcess(client, process.ProcessId);
        Assert.IsNotNull(controllableProcess);
        Assert.IsFalse(process.MemberwiseEquals(controllableProcess));
        process.Status = confirmation.Status;
        process.ConfirmedAt = confirmation.ConfirmedAt;
        Assert.IsTrue(process.MemberwiseEquals(controllableProcess));
    }

    [TestMethod]
    public async Task CanPostControllableProcess() {
        ControllableProcess process = CreateTestProcess();

        using HttpClient client = ControllerTestHelpers.CreateHttpClient();
        HttpResponseMessage response = await PostControllableProcess(client, process);
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        List<ControllableProcess> controllableProcesses = await GetControllableProcesses(client);
        Assert.HasCount(1, controllableProcesses);
        Assert.IsTrue(process.MemberwiseEquals(controllableProcesses[0]));

        response = await PostControllableProcess(client, process);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task CanDeleteControllableProcess() {
        ControllableProcess process = CreateTestProcess();

        using HttpClient client = ControllerTestHelpers.CreateHttpClient();
        HttpResponseMessage response = await PostControllableProcess(client, process);
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        List<ControllableProcess> controllableProcesses = await GetControllableProcesses(client);
        Assert.HasCount(1, controllableProcesses);
        Assert.IsTrue(process.MemberwiseEquals(controllableProcesses[0]));

        response = await DeleteControllableProcess(client, process.ProcessId);
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

        controllableProcesses = await GetControllableProcesses(client);
        Assert.IsEmpty(controllableProcesses);
    }

    private static async Task<HttpResponseMessage> PutControllableProcess(HttpClient client, ControllableProcess process) {
        var request = new StringContent(JsonSerializer.Serialize(process), Encoding.UTF8, "application/json");
        request.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await client.PutAsync($"{_baseUrl}({process.ProcessId})", request);
        return response;
    }

    private static async Task<HttpResponseMessage> PatchControllableProcess(HttpClient client, int processId, ControllableProcessConfirmation confirmation) {
        var request = new StringContent(JsonSerializer.Serialize(confirmation), Encoding.UTF8, "application/json");
        request.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await client.PatchAsync($"{_baseUrl}({processId})", request);
        return response;
    }

    private static async Task<HttpResponseMessage> PostControllableProcess(HttpClient client, ControllableProcess process) {
        var request = new StringContent(JsonSerializer.Serialize(process), Encoding.UTF8, "application/json");
        request.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await client.PostAsync(_baseUrl, request);
        return response;
    }

    private static ControllableProcess CreateTestProcess() {
        var process = new ControllableProcess {
            ProcessId = 4711,
            Status = ControllableProcessStatus.Idle,
            ConfirmedAt = DateTime.Now,
            LaunchCommand = "StartMeUp.cmd",
            Title = "This is not a title"
        };
        return process;
    }

    private static async Task<List<ControllableProcess>> GetControllableProcesses(HttpClient client) {
        HttpResponseMessage response = await client.GetAsync(_baseUrl);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        string responseString = await response.Content.ReadAsStringAsync();
        var controllableProcesses = JsonSerializer.Deserialize<ODataResponse<ControllableProcess>>(responseString)?.Value.ToList();
        return controllableProcesses;
    }

    private static async Task<ControllableProcess> GetControllableProcess(HttpClient client, int processId) {
        HttpResponseMessage response = await client.GetAsync($"{_baseUrl}({processId})");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        string responseString = await response.Content.ReadAsStringAsync();
        ControllableProcess controllableProcess = JsonSerializer.Deserialize<ControllableProcess>(responseString);
        return controllableProcess;
    }

    private static async Task<HttpResponseMessage> DeleteControllableProcess(HttpClient client, int processId) {
        return await client.DeleteAsync($"{_baseUrl}({processId})");
    }
}