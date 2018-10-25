using System;
using System.Linq;
using System.Net;
using Aspenlaub.Net.GitHub.CSharp.Tash.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Tash.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash.Model;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Controllers {
    public class ControllableProcessTasksController : ODataController {
        private readonly ITashDatabase vTashDatabase;

        public ControllableProcessTasksController(ITashDatabase tashDatabase) {
            vTashDatabase = tashDatabase;
        }

        [HttpGet, ODataRoute("ControllableProcessTasks"), EnableQuery]
        public IActionResult Get() {
            return Ok(vTashDatabase.ControllableProcessTasks);
        }

        [HttpGet, ODataRoute("ControllableProcessTasks({id})"), EnableQuery]
        public IActionResult Get([FromODataUri] Guid id) {
            var controllableProcessTaskFromDb = vTashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == id);
            if (controllableProcessTaskFromDb == null) { return NotFound(); }

            return Ok(controllableProcessTaskFromDb);
        }

        /// <summary>
        /// Used by SaveChangesAync Update (!) action with SaveChangesOptions.ReplaceOnUpdate
        /// </summary>
        /// <param name="id"></param>
        /// <param name="controllableProcessTask"></param>
        /// <returns></returns>
        [HttpPut, ODataRoute("ControllableProcessTasks({id})")]
        public IActionResult Put([FromODataUri] Guid id, [FromBody] ControllableProcessTask controllableProcessTask) {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var controllableProcessTasksFromDb = vTashDatabase.ControllableProcessTasks;
            var controllableProcessTaskFromDb = controllableProcessTasksFromDb.FirstOrDefault(p => p.Id == id);
            if (controllableProcessTaskFromDb != null) {
                controllableProcessTasksFromDb.Remove(controllableProcessTaskFromDb);
            }

            controllableProcessTask.Id = id;
            controllableProcessTasksFromDb.Add(controllableProcessTask);
            return Updated(controllableProcessTask);
        }

        /// <summary>
        /// Used by SaveChangesAync Update action without SaveChangesOptions.ReplaceOnUpdate
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patch"></param>
        /// <returns></returns>
        [HttpPatch, ODataRoute("ControllableProcessTasks({id})")]
        public IActionResult Patch([FromODataUri] Guid id, Delta<ControllableProcessTask> patch) {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var controllableProcessTaskFromDb = vTashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == id);
            if (controllableProcessTaskFromDb == null) { return NotFound(); }

            patch.Patch(controllableProcessTaskFromDb);

            return Updated(controllableProcessTaskFromDb);
        }

        /// <summary>
        /// Used by SaveChangesAsync Insert action
        /// </summary>
        /// <param name="controllableProcessTask"></param>
        /// <returns></returns>
        [HttpPost, ODataRoute("ControllableProcessTasks")]
        public IActionResult Post([FromBody] ControllableProcessTask controllableProcessTask) {
            var controllableProcessTaskFromDb = vTashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == controllableProcessTask.Id);
            if (controllableProcessTaskFromDb != null) {
                return BadRequest();
            }

            vTashDatabase.ControllableProcessTasks.Add(controllableProcessTask);
            return Created(controllableProcessTask);
        }

        /// <summary>
        /// Used by SaveChangesAsync Delete action
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, ODataRoute("ControllableProcessTasks({id})")]
        public IActionResult DeleteControllableProcess([FromODataUri] Guid id) {
            var controllableProcessTaskFromDb = vTashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == id);
            if (controllableProcessTaskFromDb == null) {
                return NotFound();
            }

            vTashDatabase.ControllableProcessTasks.Remove(controllableProcessTaskFromDb);
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}
