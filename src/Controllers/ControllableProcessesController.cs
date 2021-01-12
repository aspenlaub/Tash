using System.Linq;
using System.Net;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Tash.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash.Model;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Controllers {
    public class ControllableProcessesController : ODataController {
        private readonly ITashDatabase vTashDatabase;
        private readonly ISimpleLogger vSimpleLogger;
        private readonly string vLogId;

        public ControllableProcessesController(ITashDatabase tashDatabase, ISimpleLogger simpleLogger, ILogConfiguration logConfiguration) {
            vTashDatabase = tashDatabase;
            vSimpleLogger = simpleLogger;
            vSimpleLogger.LogSubFolder = logConfiguration.LogSubFolder;
            vLogId = logConfiguration.LogId;
        }

        [HttpGet, ODataRoute("ControllableProcesses"), EnableQuery]
        public IActionResult Get() {
            using (vSimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessesController), vLogId))) {
                vSimpleLogger.LogInformation("Returning all controllable processes");
                return Ok(vTashDatabase.ControllableProcesses);
            }
        }

        [HttpGet, ODataRoute("ControllableProcesses({id})"), EnableQuery]
        public IActionResult Get([FromODataUri] int id) {
            using (vSimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessesController), vLogId))) {
                vSimpleLogger.LogInformation($"Get controllable process with id={id}");
                var controllableProcessFromDb = vTashDatabase.ControllableProcesses.FirstOrDefault(p => p.ProcessId == id);
                if (controllableProcessFromDb == null) {
                    vSimpleLogger.LogInformation($"No controllable process found with id={id}");
                    return NotFound();
                }

                vSimpleLogger.LogInformation($"Returning controllable process with id={id}");
                return Ok(controllableProcessFromDb);
            }
        }

        /// <summary>
        /// Used by SaveChangesAsync Update (!) action with SaveChangesOptions.ReplaceOnUpdate
        /// </summary>
        /// <param name="id"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        [HttpPut, ODataRoute("ControllableProcesses({id})")]
        public IActionResult Put([FromODataUri] int id, [FromBody] ControllableProcess process) {
            using (vSimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessesController), vLogId))) {
                vSimpleLogger.LogInformation($"Put controllable process with id={id}");
                if (!ModelState.IsValid) {
                    vSimpleLogger.LogInformation($"Model is not valid, cannot put controllable process with id={id}");
                    return BadRequest(ModelState);
                }

                var controllableProcessesFromDb = vTashDatabase.ControllableProcesses;
                var controllableProcessFromDb = controllableProcessesFromDb.FirstOrDefault(p => p.ProcessId == id);
                if (controllableProcessFromDb != null) {
                    vSimpleLogger.LogInformation($"Controllable process deleted for id={id}");
                    controllableProcessesFromDb.Remove(controllableProcessFromDb);
                }

                process.ProcessId = id;
                controllableProcessesFromDb.Add(process);
                vSimpleLogger.LogInformation($"Controllable process inserted for id={id}");
                return Updated(process);
            }
        }

        /// <summary>
        /// Used by SaveChangesAsync Update action without SaveChangesOptions.ReplaceOnUpdate
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patch"></param>
        /// <returns></returns>
        [HttpPatch, ODataRoute("ControllableProcesses({id})")]
        public IActionResult Patch([FromODataUri] int id, Delta<ControllableProcess> patch) {
            using (vSimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessesController), vLogId))) {
                vSimpleLogger.LogInformation($"Patch controllable process with id={id}");
                if (!ModelState.IsValid) {
                    vSimpleLogger.LogInformation($"Model is not valid, cannot patch controllable process with id={id}");
                    return BadRequest(ModelState);
                }

                var controllableProcessFromDb = vTashDatabase.ControllableProcesses.FirstOrDefault(p => p.ProcessId == id);
                if (controllableProcessFromDb == null) {
                    vSimpleLogger.LogInformation($"No controllable process found with id={id}");
                    return NotFound();
                }

                patch.Patch(controllableProcessFromDb);

                vSimpleLogger.LogInformation($"Controllable process updated for id={id}");
                return Updated(controllableProcessFromDb);
            }
        }

        /// <summary>
        /// Used by SaveChangesAsync Insert action
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        [HttpPost, ODataRoute("ControllableProcesses")]
        public IActionResult Post([FromBody] ControllableProcess process) {
            using (vSimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessesController), vLogId))) {
                vSimpleLogger.LogInformation($"Post controllable process with id={process.ProcessId}");
                var controllableProcessFromDb = vTashDatabase.ControllableProcesses.FirstOrDefault(p => p.ProcessId == process.ProcessId);
                if (controllableProcessFromDb != null) {
                    vSimpleLogger.LogInformation($"A controllable process with id={process.ProcessId} already exists");
                    return BadRequest();
                }

                vTashDatabase.ControllableProcesses.Add(process);
                vSimpleLogger.LogInformation($"Controllable process inserted for id={process.ProcessId}");
                return Created(process);
            }
        }

        /// <summary>
        /// Used by SaveChangesAsync Delete action
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, ODataRoute("ControllableProcesses({id})")]
        public IActionResult DeleteControllableProcess([FromODataUri] int id) {
            using (vSimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessesController), vLogId))) {
                vSimpleLogger.LogInformation($"Delete controllable process with id={id}");
                var controllableProcessFromDb = vTashDatabase.ControllableProcesses.FirstOrDefault(p => p.ProcessId == id);
                if (controllableProcessFromDb == null) {
                    vSimpleLogger.LogInformation($"No controllable process found with id={id}");
                    return NotFound();
                }

                vTashDatabase.ControllableProcesses.Remove(controllableProcessFromDb);
                vSimpleLogger.LogInformation($"Controllable process deleted for id={id}");
                return StatusCode((int) HttpStatusCode.NoContent);
            }
        }
    }
}
