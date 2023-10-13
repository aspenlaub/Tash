using System;
using System.Text.Json.Serialization;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Model;

public class ControllableProcessConfirmation {
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ControllableProcessStatus Status { get; init; }

    public DateTime ConfirmedAt { get; init; }
}