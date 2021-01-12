using System;
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
    public class ControllableProcessTasksController : ODataController {
        private readonly ITashDatabase vTashDatabase;
        private readonly ISimpleLogger vSimpleLogger;
        private readonly string vLogId;

        public ControllableProcessTasksController(ITashDatabase tashDatabase, ISimpleLogger simpleLogger, ILogConfiguration logConfiguration) {
            vTashDatabase = tashDatabase;
            vSimpleLogger = simpleLogger;
            vSimpleLogger.LogSubFolder = logConfiguration.LogSubFolder;
            vLogId = logConfiguration.LogId;
        }

        [HttpGet, ODataRoute("ControllableProcessTasks"), EnableQuery]
        public IActionResult Get() {
            using (vSimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController), vLogId))) {
                vSimpleLogger.LogInformation("Returning all controllable process tasks");
                return Ok(vTashDatabase.ControllableProcessTasks);
            }
        }

        [HttpGet, ODataRoute("ControllableProcessTasks({id})"), EnableQuery]
        public IActionResult Get([FromODataUri] Guid id) {
            using (vSimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController), vLogId))) {
                vSimpleLogger.LogInformation($"Get controllable process task with id={id}");
                var controllableProcessTaskFromDb = vTashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == id);
                if (controllableProcessTaskFromDb == null) {
                    vSimpleLogger.LogInformation($"No controllable process task found with id={id}");
                    return NotFound();
                }

                vSimpleLogger.LogInformation($"Returning controllable process task with id={id}");
                return Ok(controllableProcessTaskFromDb);
            }
        }

        /// <summary>
        /// Used by SaveChangesAsync Update (!) action with SaveChangesOptions.ReplaceOnUpdate
        /// </summary>
        /// <param name="id"></param>
        /// <param name="processTask"></param>
        /// <returns></returns>
        [HttpPut, ODataRoute("ControllableProcessTasks({id})")]
        public IActionResult Put([FromODataUri] Guid id, [FromBody] ControllableProcessTask processTask) {
            using (vSimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController), vLogId))) {
                vSimpleLogger.LogInformation($"Put controllable process task with id={id}");
                if (!ModelState.IsValid) {
                    vSimpleLogger.LogInformation($"Model is not valid, cannot put controllable process task with id={id}");
                    return BadRequest(ModelState);
                }

                var controllableProcessTasksFromDb = vTashDatabase.ControllableProcessTasks;
                var controllableProcessTaskFromDb = controllableProcessTasksFromDb.FirstOrDefault(p => p.Id == id);
                if (controllableProcessTaskFromDb != null) {
                    vSimpleLogger.LogInformation($"Controllable process task deleted for id={id}");
                    controllableProcessTasksFromDb.Remove(controllableProcessTaskFromDb);
                }

                processTask.Id = id;
                controllableProcessTasksFromDb.Add(processTask);
                vSimpleLogger.LogInformation($"Controllable process task inserted for id={id}");
                return Updated(processTask);
            }
        }

        /// <summary>
        /// Used by SaveChangesAsync Update action without SaveChangesOptions.ReplaceOnUpdate
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patch"></param>
        /// <returns></returns>
        [HttpPatch, ODataRoute("ControllableProcessTasks({id})")]
        public IActionResult Patch([FromODataUri] Guid id, Delta<ControllableProcessTask> patch) {
            using (vSimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController), vLogId))) {
                vSimpleLogger.LogInformation($"Patch controllable process task with id={id}");
                if (!ModelState.IsValid) {
                    vSimpleLogger.LogInformation($"Model is not valid, cannot patch controllable process task with id={id}");
                    return BadRequest(ModelState);
                }

                var controllableProcessTaskFromDb = vTashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == id);
                if (controllableProcessTaskFromDb == null) {
                    vSimpleLogger.LogInformation($"No controllable process task found with id={id}");
                    return NotFound();
                }

                patch.Patch(controllableProcessTaskFromDb);

                vSimpleLogger.LogInformation($"Controllable process task updated for id={id}");
                return Updated(controllableProcessTaskFromDb);
            }
        }

        /// <summary>
        /// Used by SaveChangesAsync Insert action
        /// </summary>
        /// <param name="processTask"></param>
        /// <returns></returns>
        [HttpPost, ODataRoute("ControllableProcessTasks")]
        public IActionResult Post([FromBody] ControllableProcessTask processTask) {
            using (vSimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController), vLogId))) {
                vSimpleLogger.LogInformation($"Post controllable process task with id={processTask.Id}");
                var controllableProcessTaskFromDb = vTashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == processTask.Id);
                if (controllableProcessTaskFromDb != null) {
                    vSimpleLogger.LogInformation($"A controllable process task with id={processTask.Id} already exists");
                    return BadRequest();
                }

                vTashDatabase.ControllableProcessTasks.Add(processTask);
                vSimpleLogger.LogInformation($"Controllable process task inserted for id={processTask.Id}");
                return Created(processTask);
            }
        }

        /// <summary>
        /// Used by SaveChangesAsync Delete action
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, ODataRoute("ControllableProcessTasks({id})")]
        public IActionResult DeleteControllableProcess([FromODataUri] Guid id) {
            using (vSimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController), vLogId))) {
                vSimpleLogger.LogInformation($"Delete controllable process task with id={id}");
                var controllableProcessTaskFromDb = vTashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == id);
                if (controllableProcessTaskFromDb == null) {
                    vSimpleLogger.LogInformation($"No controllable process task found with id={id}");
                    return NotFound();
                }

                vTashDatabase.ControllableProcessTasks.Remove(controllableProcessTaskFromDb);
                vSimpleLogger.LogInformation($"Controllable process task deleted for id={id}");
                return StatusCode((int) HttpStatusCode.NoContent);
            }
        }
    }
}
