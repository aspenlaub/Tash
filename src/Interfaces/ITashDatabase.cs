using System.Collections.Concurrent;
using Aspenlaub.Net.GitHub.CSharp.Tash.Model;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Interfaces {
    public interface ITashDatabase {
        BlockingCollection<ControllableProcess> ControllableProcesses { get; set; }
        BlockingCollection<ControllableProcessTask> ControllableProcessTasks { get; set; }
    }
}
