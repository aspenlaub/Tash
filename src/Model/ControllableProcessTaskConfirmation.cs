using System.Text.Json.Serialization;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Model;

public class ControllableProcessTaskConfirmation {
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ControllableProcessTaskStatus Status { get; init; }
}