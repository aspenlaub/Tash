using System;
using System.Linq;
using System.Net;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Tash.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Controllers;

public class ControllableProcessTasksController : ODataController {
    private readonly ITashDatabase TashDatabase;
    private readonly ISimpleLogger SimpleLogger;
    private readonly IMethodNamesFromStackFramesExtractor MethodNamesFromStackFramesExtractor;

    public ControllableProcessTasksController(ITashDatabase tashDatabase, ISimpleLogger simpleLogger, IMethodNamesFromStackFramesExtractor methodNamesFromStackFramesExtractor) {
        TashDatabase = tashDatabase;
        SimpleLogger = simpleLogger;
        MethodNamesFromStackFramesExtractor = methodNamesFromStackFramesExtractor;
    }

    [HttpGet, EnableQuery]
    public IActionResult Get() {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController) + nameof(Get), SimpleLogger.LogId))) {
            var methodNamesFromStack = MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            SimpleLogger.LogInformationWithCallStack("Returning all controllable process tasks", methodNamesFromStack);
            return Ok(TashDatabase.ControllableProcessTasks);
        }
    }

    [HttpGet, EnableQuery]
    public IActionResult Get(Guid key) {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController) + nameof(Get), SimpleLogger.LogId))) {
            var methodNamesFromStack = MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            SimpleLogger.LogInformationWithCallStack($"Get controllable process task with id={key}", methodNamesFromStack);
            var controllableProcessTaskFromDb = TashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == key);
            if (controllableProcessTaskFromDb == null) {
                SimpleLogger.LogInformationWithCallStack($"No controllable process task found with id={key}", methodNamesFromStack);
                return NotFound();
            }

            SimpleLogger.LogInformationWithCallStack($"Returning controllable process task with id={key}", methodNamesFromStack);
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
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController) + nameof(Put), SimpleLogger.LogId))) {
            var methodNamesFromStack = MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            SimpleLogger.LogInformationWithCallStack($"Put controllable process task with id={key}", methodNamesFromStack);
            if (!ModelState.IsValid) {
                SimpleLogger.LogInformationWithCallStack($"Model is not valid, cannot put controllable process task with id={key}", methodNamesFromStack);
                return BadRequest(ModelState);
            }

            var controllableProcessTasksFromDb = TashDatabase.ControllableProcessTasks;
            var controllableProcessTaskFromDb = controllableProcessTasksFromDb.FirstOrDefault(p => p.Id == key);
            if (controllableProcessTaskFromDb != null) {
                SimpleLogger.LogInformationWithCallStack($"Controllable process task deleted for id={key}", methodNamesFromStack);
                controllableProcessTasksFromDb.Remove(controllableProcessTaskFromDb);
            }

            processTask.Id = key;
            controllableProcessTasksFromDb.Add(processTask);
            SimpleLogger.LogInformationWithCallStack($"Controllable process task inserted for id={key}", methodNamesFromStack);
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
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController) + nameof(Patch), SimpleLogger.LogId))) {
            var methodNamesFromStack = MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            SimpleLogger.LogInformationWithCallStack($"Patch controllable process task with id={key}", methodNamesFromStack);
            if (!ModelState.IsValid) {
                SimpleLogger.LogInformationWithCallStack($"Model is not valid, cannot patch controllable process task with id={key}", methodNamesFromStack);
                return BadRequest(ModelState);
            }

            var controllableProcessTaskFromDb = TashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == key);
            if (controllableProcessTaskFromDb == null) {
                SimpleLogger.LogInformationWithCallStack($"No controllable process task found with id={key}", methodNamesFromStack);
                return NotFound();
            }

            patch.Patch(controllableProcessTaskFromDb);

            SimpleLogger.LogInformationWithCallStack($"Controllable process task updated for id={key}", methodNamesFromStack);
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
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController) + nameof(Post), SimpleLogger.LogId))) {
            var methodNamesFromStack = MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            SimpleLogger.LogInformationWithCallStack($"Post controllable process task with id={processTask.Id}", methodNamesFromStack);
            var controllableProcessTaskFromDb = TashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == processTask.Id);
            if (controllableProcessTaskFromDb != null) {
                SimpleLogger.LogInformationWithCallStack($"A controllable process task with id={processTask.Id} already exists", methodNamesFromStack);
                return BadRequest();
            }

            TashDatabase.ControllableProcessTasks.Add(processTask);
            SimpleLogger.LogInformationWithCallStack($"Controllable process task inserted for id={processTask.Id}", methodNamesFromStack);
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
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController) + nameof(Delete), SimpleLogger.LogId))) {
            var methodNamesFromStack = MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            SimpleLogger.LogInformationWithCallStack($"Delete controllable process task with id={key}", methodNamesFromStack);
            var controllableProcessTaskFromDb = TashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == key);
            if (controllableProcessTaskFromDb == null) {
                SimpleLogger.LogInformationWithCallStack($"No controllable process task found with id={key}", methodNamesFromStack);
                return NotFound();
            }

            TashDatabase.ControllableProcessTasks.Remove(controllableProcessTaskFromDb);
            SimpleLogger.LogInformationWithCallStack($"Controllable process task deleted for id={key}", methodNamesFromStack);
            return StatusCode((int) HttpStatusCode.NoContent);
        }
    }
}