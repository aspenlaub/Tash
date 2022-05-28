using System;
using System.Linq;
using System.Net;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Tash.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Logging;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Controllers;

public class ControllableProcessTasksController : ODataController {
    private readonly ITashDatabase TashDatabase;
    private readonly ISimpleLogger SimpleLogger;
    private readonly string LogId;

    public ControllableProcessTasksController(ITashDatabase tashDatabase, ISimpleLogger simpleLogger, ILogConfigurationFactory logConfigurationFactory) {
        TashDatabase = tashDatabase;
        SimpleLogger = simpleLogger;
        var logConfiguration = logConfigurationFactory.Create("Tash", true);
        SimpleLogger.LogSubFolder = logConfiguration.LogSubFolder;
        LogId = logConfiguration.LogId;
    }

    [HttpGet, EnableQuery]
    public IActionResult Get() {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController), LogId))) {
            SimpleLogger.LogInformation("Returning all controllable process tasks");
            return Ok(TashDatabase.ControllableProcessTasks);
        }
    }

    [HttpGet, EnableQuery]
    public IActionResult Get(Guid key) {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController), LogId))) {
            SimpleLogger.LogInformation($"Get controllable process task with id={key}");
            var controllableProcessTaskFromDb = TashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == key);
            if (controllableProcessTaskFromDb == null) {
                SimpleLogger.LogInformation($"No controllable process task found with id={key}");
                return NotFound();
            }

            SimpleLogger.LogInformation($"Returning controllable process task with id={key}");
            return Ok(controllableProcessTaskFromDb);
        }
    }

    /// <summary>
    /// Used by SaveChangesAsync Update (!) action with SaveChangesOptions.ReplaceOnUpdate
    /// </summary>
    /// <param name="key"></param>
    /// <param name="processTask"></param>
    /// <returns></returns>
    [HttpPut]
    public IActionResult Put(Guid key, [FromBody] ControllableProcessTask processTask) {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController), LogId))) {
            SimpleLogger.LogInformation($"Put controllable process task with id={key}");
            if (!ModelState.IsValid) {
                SimpleLogger.LogInformation($"Model is not valid, cannot put controllable process task with id={key}");
                return BadRequest(ModelState);
            }

            var controllableProcessTasksFromDb = TashDatabase.ControllableProcessTasks;
            var controllableProcessTaskFromDb = controllableProcessTasksFromDb.FirstOrDefault(p => p.Id == key);
            if (controllableProcessTaskFromDb != null) {
                SimpleLogger.LogInformation($"Controllable process task deleted for id={key}");
                controllableProcessTasksFromDb.Remove(controllableProcessTaskFromDb);
            }

            processTask.Id = key;
            controllableProcessTasksFromDb.Add(processTask);
            SimpleLogger.LogInformation($"Controllable process task inserted for id={key}");
            return Updated(processTask);
        }
    }

    /// <summary>
    /// Used by SaveChangesAsync Update action without SaveChangesOptions.ReplaceOnUpdate
    /// </summary>
    /// <param name="key"></param>
    /// <param name="patch"></param>
    /// <returns></returns>
    [HttpPatch]
    public IActionResult Patch(Guid key, Delta<ControllableProcessTask> patch) {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController), LogId))) {
            SimpleLogger.LogInformation($"Patch controllable process task with id={key}");
            if (!ModelState.IsValid) {
                SimpleLogger.LogInformation($"Model is not valid, cannot patch controllable process task with id={key}");
                return BadRequest(ModelState);
            }

            var controllableProcessTaskFromDb = TashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == key);
            if (controllableProcessTaskFromDb == null) {
                SimpleLogger.LogInformation($"No controllable process task found with id={key}");
                return NotFound();
            }

            patch.Patch(controllableProcessTaskFromDb);

            SimpleLogger.LogInformation($"Controllable process task updated for id={key}");
            return Updated(controllableProcessTaskFromDb);
        }
    }

    /// <summary>
    /// Used by SaveChangesAsync Insert action
    /// </summary>
    /// <param name="processTask"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult Post([FromBody] ControllableProcessTask processTask) {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController), LogId))) {
            SimpleLogger.LogInformation($"Post controllable process task with id={processTask.Id}");
            var controllableProcessTaskFromDb = TashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == processTask.Id);
            if (controllableProcessTaskFromDb != null) {
                SimpleLogger.LogInformation($"A controllable process task with id={processTask.Id} already exists");
                return BadRequest();
            }

            TashDatabase.ControllableProcessTasks.Add(processTask);
            SimpleLogger.LogInformation($"Controllable process task inserted for id={processTask.Id}");
            return Created(processTask);
        }
    }

    /// <summary>
    /// Used by SaveChangesAsync Delete action
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [HttpDelete]
    public IActionResult Delete(Guid key) {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController), LogId))) {
            SimpleLogger.LogInformation($"Delete controllable process task with id={key}");
            var controllableProcessTaskFromDb = TashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == key);
            if (controllableProcessTaskFromDb == null) {
                SimpleLogger.LogInformation($"No controllable process task found with id={key}");
                return NotFound();
            }

            TashDatabase.ControllableProcessTasks.Remove(controllableProcessTaskFromDb);
            SimpleLogger.LogInformation($"Controllable process task deleted for id={key}");
            return StatusCode((int) HttpStatusCode.NoContent);
        }
    }
}