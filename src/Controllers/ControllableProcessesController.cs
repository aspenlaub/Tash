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
            var controllableProcessFromDb = vTashDatabase.ControllableProcesses.FirstOrDefault(p => p.ProcessId == id);
            if (controllableProcessFromDb == null) { return NotFound(); }

            return Ok(controllableProcessFromDb);
        }

        /// <summary>
        /// Used by SaveChangesAync Update (!) action with SaveChangesOptions.ReplaceOnUpdate
        /// </summary>
        /// <param name="id"></param>
        /// <param name="controllableProcess"></param>
        /// <returns></returns>
        [HttpPut, ODataRoute("ControllableProcesses({id})")]
        public IActionResult Put([FromODataUri] int id, [FromBody] ControllableProcess controllableProcess) {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var controllableProcessesFromDb = vTashDatabase.ControllableProcesses;
            var controllableProcessFromDb = controllableProcessesFromDb.FirstOrDefault(p => p.ProcessId == id);
            if (controllableProcessFromDb != null) {
                controllableProcessesFromDb.Remove(controllableProcessFromDb);
            }

            controllableProcess.ProcessId = id;
            controllableProcessesFromDb.Add(controllableProcess);
            return Updated(controllableProcess);
        }

        /// <summary>
        /// Used by SaveChangesAync Update action without SaveChangesOptions.ReplaceOnUpdate
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patch"></param>
        /// <returns></returns>
        [HttpPatch, ODataRoute("ControllableProcesses({id})")]
        public IActionResult Patch([FromODataUri] int id, Delta<ControllableProcess> patch) {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var controllableProcessFromDb = vTashDatabase.ControllableProcesses.FirstOrDefault(p => p.ProcessId == id);
            if (controllableProcessFromDb == null) { return NotFound(); }

            patch.Patch(controllableProcessFromDb);

            return Updated(controllableProcessFromDb);
        }

        /// <summary>
        /// Used by SaveChangesAsync Insert action
        /// </summary>
        /// <param name="controllableProcess"></param>
        /// <returns></returns>
        [HttpPost, ODataRoute("ControllableProcesses")]
        public IActionResult Post([FromBody] ControllableProcess controllableProcess) {
            var controllableProcessFromDb = vTashDatabase.ControllableProcesses.FirstOrDefault(p => p.ProcessId == controllableProcess.ProcessId);
            if (controllableProcessFromDb != null) {
                return BadRequest();
            }

            vTashDatabase.ControllableProcesses.Add(controllableProcess);
            return Created(controllableProcess);
        }

        /// <summary>
        /// Used by SaveChangesAsync Delete action
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, ODataRoute("ControllableProcesses({id})")]
        public IActionResult DeleteControllableProcess([FromODataUri] int id) {
            var controllableProcessFromDb = vTashDatabase.ControllableProcesses.FirstOrDefault(p => p.ProcessId == id);
            if (controllableProcessFromDb == null) {
                return NotFound();
            }

            vTashDatabase.ControllableProcesses.Remove(controllableProcessFromDb);
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}
