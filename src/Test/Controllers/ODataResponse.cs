using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Test.Controllers;

public class ODataResponse<T> {
    [JsonPropertyName("value")]
    public List<T> Value { get; set; } = new();
}