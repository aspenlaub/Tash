using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Model {
    public class ControllableProcessTask {
        [Key]
        public Guid Id { get; set; }

        public int ProcessId { get; set; }

        public string Type { get; set; }

        public string ControlName { get; init; } = "";

        public string Text { get; init; } = "";

        [JsonConverter(typeof(StringEnumConverter))]
        public ControllableProcessTaskStatus Status { get; set; }

        public string ErrorMessage { get; set; } = "";
    }
}
