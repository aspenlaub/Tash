using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

public class ControllableProcessTasksController(ITashDatabase tashDatabase,
        ISimpleLogger simpleLogger,
        IMethodNamesFromStackFramesExtractor methodNamesFromStackFramesExtractor)
    : ODataController {
    [HttpGet, EnableQuery]
    public IActionResult Get() {
        using (simpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController) + nameof(Get)))) {
            IList<string> methodNamesFromStack = methodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            LogInformationWithCallStack("Returning all controllable process tasks", methodNamesFromStack);

            return Ok(tashDatabase.ControllableProcessTasks);
        }
    }

    [HttpGet, EnableQuery]
    public IActionResult Get(Guid key) {
        using (simpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController) + nameof(Get)))) {
            IList<string> methodNamesFromStack = methodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            LogInformationWithCallStack($"Get controllable process task with id={key}", methodNamesFromStack);
            ControllableProcessTask controllableProcessTaskFromDb = tashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == key);
            if (controllableProcessTaskFromDb == null) {
                LogInformationWithCallStack($"No controllable process task found with id={key}", methodNamesFromStack);
                return NotFound();
            }

            LogInformationWithCallStack($"Returning controllable process task with id={key}", methodNamesFromStack);
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
        using (simpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController) + nameof(Put)))) {
            IList<string> methodNamesFromStack = methodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            LogInformationWithCallStack($"Put controllable process task with id={key}", methodNamesFromStack);
            if (!ModelState.IsValid) {
                LogInformationWithCallStack($"Model is not valid, cannot put controllable process task with id={key}", methodNamesFromStack);
                return BadRequest(ModelState);
            }

            BlockingCollection<ControllableProcessTask> controllableProcessTasksFromDb = tashDatabase.ControllableProcessTasks;
            ControllableProcessTask controllableProcessTaskFromDb = controllableProcessTasksFromDb.FirstOrDefault(p => p.Id == key);
            if (controllableProcessTaskFromDb != null) {
                LogInformationWithCallStack($"Controllable process task deleted for id={key}", methodNamesFromStack);
                controllableProcessTasksFromDb.Remove(controllableProcessTaskFromDb);
            }

            processTask.Id = key;
            controllableProcessTasksFromDb.Add(processTask);
            LogInformationWithCallStack($"Controllable process task inserted for id={key}", methodNamesFromStack);
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
        using (simpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController) + nameof(Patch)))) {
            IList<string> methodNamesFromStack = methodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            LogInformationWithCallStack($"Patch controllable process task with id={key}", methodNamesFromStack);
            if (!ModelState.IsValid) {
                LogInformationWithCallStack($"Model is not valid, cannot patch controllable process task with id={key}", methodNamesFromStack);
                return BadRequest(ModelState);
            }

            ControllableProcessTask controllableProcessTaskFromDb = tashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == key);
            if (controllableProcessTaskFromDb == null) {
                LogInformationWithCallStack($"No controllable process task found with id={key}", methodNamesFromStack);
                return NotFound();
            }

            patch.Patch(controllableProcessTaskFromDb);

            LogInformationWithCallStack($"Controllable process task updated for id={key}", methodNamesFromStack);
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
        using (simpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController) + nameof(Post)))) {
            IList<string> methodNamesFromStack = methodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            LogInformationWithCallStack($"Post controllable process task with id={processTask.Id}", methodNamesFromStack);
            ControllableProcessTask controllableProcessTaskFromDb = tashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == processTask.Id);
            if (controllableProcessTaskFromDb != null) {
                LogInformationWithCallStack($"A controllable process task with id={processTask.Id} already exists", methodNamesFromStack);
                return BadRequest();
            }

            tashDatabase.ControllableProcessTasks.Add(processTask);
            LogInformationWithCallStack($"Controllable process task inserted for id={processTask.Id}", methodNamesFromStack);
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
        using (simpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController) + nameof(Delete)))) {
            IList<string> methodNamesFromStack = methodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            LogInformationWithCallStack($"Delete controllable process task with id={key}", methodNamesFromStack);
            ControllableProcessTask controllableProcessTaskFromDb = tashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == key);
            if (controllableProcessTaskFromDb == null) {
                LogInformationWithCallStack($"No controllable process task found with id={key}", methodNamesFromStack);
                return NotFound();
            }

            tashDatabase.ControllableProcessTasks.Remove(controllableProcessTaskFromDb);
            LogInformationWithCallStack($"Controllable process task deleted for id={key}", methodNamesFromStack);
            return StatusCode((int) HttpStatusCode.NoContent);
        }
    }

    private void LogInformationWithCallStack(string message, IList<string> methodNamesFromStack) {
        try {
            simpleLogger.LogInformationWithCallStack(message, methodNamesFromStack);
            // ReSharper disable once EmptyGeneralCatchClause
        } catch {
        }
    }
}