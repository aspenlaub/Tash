using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Model;

public class ControllableProcessTaskConfirmation {
    [JsonConverter(typeof(StringEnumConverter))]
    public ControllableProcessTaskStatus Status { get; init; }
}