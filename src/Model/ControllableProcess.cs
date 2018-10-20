using System;
using System.ComponentModel.DataAnnotations;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Model {
    public class ControllableProcess {
        [Key]
        public int ProcessId { get; set; }

        public string Title { get; set; }

        public string LaunchCommand { get; set; }

        public bool Busy { get; set; }

        public DateTime ConfirmedAt { get; set; }
    }
}
