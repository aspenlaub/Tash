using System.Linq;
using System.Net;
using Aspenlaub.Net.GitHub.CSharp.Tash.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Tash.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash.Model;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Controllers {
    public class ControllableProcessesController : ODataController {
        private readonly ITashDatabase vTashDatabase;

        public ControllableProcessesController(ITashDatabase tashDatabase) {
            vTashDatabase = tashDatabase;
        }

        [HttpGet, ODataRoute("ControllableProcesses")]
        public IActionResult Get() {
            return Ok(vTashDatabase.ControllableProcesses);
        }

        [HttpPut, ODataRoute("ControllableProcesses")]
        public IActionResult Put([FromBody] ControllableProcess controllableProcess) {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var processes = vTashDatabase.ControllableProcesses;
            var process = processes.FirstOrDefault(p => p.ProcessId == controllableProcess.ProcessId);
            if (process != null) {
                processes.Remove(process);
            }
            processes.Add(controllableProcess);
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}
