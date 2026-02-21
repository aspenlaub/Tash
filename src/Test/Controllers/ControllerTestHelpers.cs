using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: DoNotParallelize]

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Test.Controllers;

public static class ControllerTestHelpers {

    public static HttpClient CreateHttpClient() {
        var factory = new WebApplicationFactory<Program>();
        HttpClient client = factory.CreateClient();
        Assert.IsNotNull(client);
        return client;
    }
}