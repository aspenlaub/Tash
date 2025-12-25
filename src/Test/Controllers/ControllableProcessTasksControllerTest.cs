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

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Test.Controllers;

[TestClass]
public class ControllableProcessTasksControllerTest {
    private const string _baseUrl = "http://localhost:60404/ControllableProcessTasks";

    [TestMethod]
    public async Task CanGetControllableProcessTasks() {
        using HttpClient client = await ControllerTestHelpers.CreateHttpClientAsync();
        List<ControllableProcessTask> controllableProcesses = await GetControllableProcessTasks(client);
        Assert.IsEmpty(controllableProcesses);
    }

    [TestMethod]
    public async Task CanPutControllableProcessTask() {
        ControllableProcessTask processTask = CreateTestProcessTask();

        using HttpClient client = await ControllerTestHelpers.CreateHttpClientAsync();
        HttpResponseMessage response = await PutControllableProcessTask(client, processTask);
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

        List<ControllableProcessTask> controllableProcesses = await GetControllableProcessTasks(client);
        Assert.HasCount(1, controllableProcesses);
        Assert.IsTrue(processTask.MemberwiseEquals(controllableProcesses[0]));
    }

    [TestMethod]
    public async Task CanGetControllableProcessTask() {
        ControllableProcessTask processTask = CreateTestProcessTask();

        using HttpClient client = await ControllerTestHelpers.CreateHttpClientAsync();
        HttpResponseMessage response = await PutControllableProcessTask(client, processTask);
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

        ControllableProcessTask controllableProcessTask = await GetControllableProcessTask(client, processTask.Id);
        Assert.IsNotNull(controllableProcessTask);
        Assert.IsTrue(processTask.MemberwiseEquals(controllableProcessTask));
    }

    [TestMethod]
    public async Task CanConfirmCompletedProcessTask() {
        ControllableProcessTask processTask = CreateTestProcessTask();

        using HttpClient client = await ControllerTestHelpers.CreateHttpClientAsync();
        HttpResponseMessage response = await PutControllableProcessTask(client, processTask);
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

        var confirmation = new ControllableProcessTaskConfirmation {
            Status = ControllableProcessTaskStatus.Completed
        };

        response = await PatchControllableProcessTask(client, processTask.Id, confirmation);
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

        ControllableProcessTask controllableProcess = await GetControllableProcessTask(client, processTask.Id);
        Assert.IsNotNull(controllableProcess);
        Assert.IsFalse(processTask.MemberwiseEquals(controllableProcess));
        processTask.Status = confirmation.Status;
        Assert.IsTrue(processTask.MemberwiseEquals(controllableProcess));
    }

    [TestMethod]
    public async Task CanPostControllableProcessTask() {
        ControllableProcessTask processTask = CreateTestProcessTask();

        using HttpClient client = await ControllerTestHelpers.CreateHttpClientAsync();
        HttpResponseMessage response = await PostControllableProcessTask(client, processTask);
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        List<ControllableProcessTask> controllableProcessTasks = await GetControllableProcessTasks(client);
        Assert.HasCount(1, controllableProcessTasks);
        Assert.IsTrue(processTask.MemberwiseEquals(controllableProcessTasks[0]));

        response = await PostControllableProcessTask(client, processTask);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task CanDeleteControllableProcessTask() {
        ControllableProcessTask processTask = CreateTestProcessTask();

        using HttpClient client = await ControllerTestHelpers.CreateHttpClientAsync();
        HttpResponseMessage response = await PostControllableProcessTask(client, processTask);
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        List<ControllableProcessTask> controllableProcessTasks = await GetControllableProcessTasks(client);
        Assert.HasCount(1, controllableProcessTasks);
        Assert.IsTrue(processTask.MemberwiseEquals(controllableProcessTasks[0]));

        response = await DeleteControllableProcessTask(client, processTask.Id);
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

        controllableProcessTasks = await GetControllableProcessTasks(client);
        Assert.IsEmpty(controllableProcessTasks);
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
        HttpResponseMessage response = await client.GetAsync(_baseUrl);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        string responseString = await response.Content.ReadAsStringAsync();
        var controllableProcessTasks = JsonSerializer.Deserialize<ODataResponse<ControllableProcessTask>>(responseString)?.Value.ToList();
        return controllableProcessTasks;
    }

    private static async Task<ControllableProcessTask> GetControllableProcessTask(HttpClient client, Guid taskId) {
        HttpResponseMessage response = await client.GetAsync($"{_baseUrl}({taskId})");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        string responseString = await response.Content.ReadAsStringAsync();
        ControllableProcessTask controllableProcessTask = JsonSerializer.Deserialize<ControllableProcessTask>(responseString);
        return controllableProcessTask;
    }

    private static async Task<HttpResponseMessage> PutControllableProcessTask(HttpClient client, ControllableProcessTask processTask) {
        string serializedTask = JsonSerializer.Serialize(processTask);
        var request = new StringContent(serializedTask, Encoding.UTF8, "application/json");
        request.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await client.PutAsync($"{_baseUrl}({processTask.Id})", request);
        return response;
    }

    private static async Task<HttpResponseMessage> PatchControllableProcessTask(HttpClient client, Guid taskId, ControllableProcessTaskConfirmation confirmation) {
        var request = new StringContent(JsonSerializer.Serialize(confirmation), Encoding.UTF8, "application/json");
        request.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await client.PatchAsync($"{_baseUrl}({taskId})", request);
        return response;
    }

    private static async Task<HttpResponseMessage> PostControllableProcessTask(HttpClient client, ControllableProcessTask processTask) {
        var request = new StringContent(JsonSerializer.Serialize(processTask), Encoding.UTF8, "application/json");
        request.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await client.PostAsync(_baseUrl, request);
        return response;
    }

    private static async Task<HttpResponseMessage> DeleteControllableProcessTask(HttpClient client, Guid taskId) {
        return await client.DeleteAsync($"{_baseUrl}({taskId})");
    }
}