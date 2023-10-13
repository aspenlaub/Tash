using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Model;

public class ControllableProcessTask {
    [Key]
    public Guid Id { get; set; }

    public int ProcessId { get; set; }

    public string Type { get; set; }

    public string ControlName { get; init; } = "";

    public string Text { get; init; } = "";

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ControllableProcessTaskStatus Status { get; set; }

    public string ErrorMessage { get; set; } = "";
}