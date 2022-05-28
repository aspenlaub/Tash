using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Model;

public class ControllableProcessConfirmation {
    [JsonConverter(typeof(StringEnumConverter))]
    public ControllableProcessStatus Status { get; init; }

    public DateTime ConfirmedAt { get; init; }
}