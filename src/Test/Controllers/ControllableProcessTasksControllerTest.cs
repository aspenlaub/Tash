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

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Test.Controllers;

[TestClass]
public class ControllableProcessTasksControllerTest {
    private const string BaseUrl = "http://localhost:60404/ControllableProcessTasks";

    [TestMethod]
    public async Task CanGetControllableProcessTasks() {
        using var client = ControllerTestHelpers.CreateHttpClient();
        var controllableProcesses = await GetControllableProcessTasks(client);
        Assert.AreEqual(0, controllableProcesses.Count);
    }

    [TestMethod]
    public async Task CanPutControllableProcessTask() {
        var processTask = CreateTestProcessTask();

        using var client = ControllerTestHelpers.CreateHttpClient();
        var response = await PutControllableProcessTask(client, processTask);
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

        var controllableProcesses = await GetControllableProcessTasks(client);
        Assert.AreEqual(1, controllableProcesses.Count);
        Assert.IsTrue(processTask.MemberwiseEquals(controllableProcesses[0]));
    }

    [TestMethod]
    public async Task CanGetControllableProcessTask() {
        var processTask = CreateTestProcessTask();

        using var client = ControllerTestHelpers.CreateHttpClient();
        var response = await PutControllableProcessTask(client, processTask);
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

        var controllableProcessTask = await GetControllableProcessTask(client, processTask.Id);
        Assert.IsNotNull(controllableProcessTask);
        Assert.IsTrue(processTask.MemberwiseEquals(controllableProcessTask));
    }

    [TestMethod]
    public async Task CanConfirmCompletedProcessTask() {
        var processTask = CreateTestProcessTask();

        using var client = ControllerTestHelpers.CreateHttpClient();
        var response = await PutControllableProcessTask(client, processTask);
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

        var confirmation = new ControllableProcessTaskConfirmation {
            Status = ControllableProcessTaskStatus.Completed
        };

        response = await PatchControllableProcessTask(client, processTask.Id, confirmation);
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

        var controllableProcess = await GetControllableProcessTask(client, processTask.Id);
        Assert.IsNotNull(controllableProcess);
        Assert.IsFalse(processTask.MemberwiseEquals(controllableProcess));
        processTask.Status = confirmation.Status;
        Assert.IsTrue(processTask.MemberwiseEquals(controllableProcess));
    }

    [TestMethod]
    public async Task CanPostControllableProcessTask() {
        var processTask = CreateTestProcessTask();

        using var client = ControllerTestHelpers.CreateHttpClient();
        var response = await PostControllableProcessTask(client, processTask);
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        var controllableProcessTasks = await GetControllableProcessTasks(client);
        Assert.AreEqual(1, controllableProcessTasks.Count);
        Assert.IsTrue(processTask.MemberwiseEquals(controllableProcessTasks[0]));

        response = await PostControllableProcessTask(client, processTask);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task CanDeleteControllableProcessTask() {
        var processTask = CreateTestProcessTask();

        using var client = ControllerTestHelpers.CreateHttpClient();
        var response = await PostControllableProcessTask(client, processTask);
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        var controllableProcessTasks = await GetControllableProcessTasks(client);
        Assert.AreEqual(1, controllableProcessTasks.Count);
        Assert.IsTrue(processTask.MemberwiseEquals(controllableProcessTasks[0]));

        response = await DeleteControllableProcessTask(client, processTask.Id);
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

        controllableProcessTasks = await GetControllableProcessTasks(client);
        Assert.AreEqual(0, controllableProcessTasks.Count);
    }

    private static ControllableProcessTask CreateTestProcessTask() {
        var process = new ControllableProcessTask {
            Id = Guid.NewGuid(),
            ProcessId = 4711,
            Type = "PressButton",
            ControlName = "RunButton",
            Text = "Buttons do not have text",
            Status = ControllableProcessTaskStatus.Requested
        };
        return process;
    }

    private static async Task<List<ControllableProcessTask>> GetControllableProcessTasks(HttpClient client) {
        var response = await client.GetAsync(BaseUrl);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        var controllableProcessTasks = JsonConvert.DeserializeObject<ODataResponse<ControllableProcessTask>>(responseString)?.Value.ToList();
        return controllableProcessTasks;
    }

    private static async Task<ControllableProcessTask> GetControllableProcessTask(HttpClient client, Guid taskId) {
        var response = await client.GetAsync($"{BaseUrl}({taskId})");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        var controllableProcessTask = JsonConvert.DeserializeObject<ControllableProcessTask>(responseString);
        return controllableProcessTask;
    }

    private static async Task<HttpResponseMessage> PutControllableProcessTask(HttpClient client, ControllableProcessTask processTask) {
        var serializedTask = JsonConvert.SerializeObject(processTask);
        var request = new StringContent(serializedTask, Encoding.UTF8, "application/json");
        request.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var response = await client.PutAsync($"{BaseUrl}({processTask.Id})", request);
        return response;
    }

    private static async Task<HttpResponseMessage> PatchControllableProcessTask(HttpClient client, Guid taskId, ControllableProcessTaskConfirmation confirmation) {
        var request = new StringContent(JsonConvert.SerializeObject(confirmation), Encoding.UTF8, "application/json");
        request.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var response = await client.PatchAsync($"{BaseUrl}({taskId})", request);
        return response;
    }

    private static async Task<HttpResponseMessage> PostControllableProcessTask(HttpClient client, ControllableProcessTask processTask) {
        var request = new StringContent(JsonConvert.SerializeObject(processTask), Encoding.UTF8, "application/json");
        request.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var response = await client.PostAsync(BaseUrl, request);
        return response;
    }

    private static async Task<HttpResponseMessage> DeleteControllableProcessTask(HttpClient client, Guid taskId) {
        return await client.DeleteAsync($"{BaseUrl}({taskId})");
    }
}