using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Model {
    public class ControllableProcess {
        [Key]
        public int ProcessId { get; set; }

        public string Title { get; set; }

        public string LaunchCommand { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ControllableProcessStatus Status { get; set; }

        public DateTime ConfirmedAt { get; set; }
    }
}
