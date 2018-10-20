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

        [HttpGet, ODataRoute("ControllableProcesses"), EnableQuery]
        public IActionResult Get() {
            return Ok(vTashDatabase.ControllableProcesses);
        }

        [HttpGet, ODataRoute("ControllableProcesses({id})"), EnableQuery]
        public IActionResult Get([FromODataUri] int id) {
            var process = vTashDatabase.ControllableProcesses.FirstOrDefault(p => p.ProcessId == id);
            if (process == null) { return NotFound(); }

            return Ok(process);
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

        [HttpPatch, ODataRoute("ControllableProcesses({id})")]
        public IActionResult Patch([FromODataUri] int id, Delta<ControllableProcess> patch) {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var process = vTashDatabase.ControllableProcesses.FirstOrDefault(p => p.ProcessId == id);
            if (process == null) { return NotFound(); }

            patch.Patch(process);

            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}
