using System.Collections.Concurrent;
using Aspenlaub.Net.GitHub.CSharp.Tash.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash.Model;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.DataAccess {
    public class TashDatabase : ITashDatabase {
        public BlockingCollection<ControllableProcess> ControllableProcesses { get; set; } = new BlockingCollection<ControllableProcess>();
    }
}
