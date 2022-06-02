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
    private readonly ITashDatabase _TashDatabase;
    private readonly ISimpleLogger _SimpleLogger;
    private readonly IMethodNamesFromStackFramesExtractor _MethodNamesFromStackFramesExtractor;

    public ControllableProcessTasksController(ITashDatabase tashDatabase, ISimpleLogger simpleLogger, IMethodNamesFromStackFramesExtractor methodNamesFromStackFramesExtractor) {
        _TashDatabase = tashDatabase;
        _SimpleLogger = simpleLogger;
        _MethodNamesFromStackFramesExtractor = methodNamesFromStackFramesExtractor;
    }

    [HttpGet, EnableQuery]
    public IActionResult Get() {
        using (_SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController) + nameof(Get), _SimpleLogger.LogId))) {
            var methodNamesFromStack = _MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            _SimpleLogger.LogInformationWithCallStack("Returning all controllable process tasks", methodNamesFromStack);
            return Ok(_TashDatabase.ControllableProcessTasks);
        }
    }

    [HttpGet, EnableQuery]
    public IActionResult Get(Guid key) {
        using (_SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController) + nameof(Get), _SimpleLogger.LogId))) {
            var methodNamesFromStack = _MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            _SimpleLogger.LogInformationWithCallStack($"Get controllable process task with id={key}", methodNamesFromStack);
            var controllableProcessTaskFromDb = _TashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == key);
            if (controllableProcessTaskFromDb == null) {
                _SimpleLogger.LogInformationWithCallStack($"No controllable process task found with id={key}", methodNamesFromStack);
                return NotFound();
            }

            _SimpleLogger.LogInformationWithCallStack($"Returning controllable process task with id={key}", methodNamesFromStack);
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
        using (_SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController) + nameof(Put), _SimpleLogger.LogId))) {
            var methodNamesFromStack = _MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            _SimpleLogger.LogInformationWithCallStack($"Put controllable process task with id={key}", methodNamesFromStack);
            if (!ModelState.IsValid) {
                _SimpleLogger.LogInformationWithCallStack($"Model is not valid, cannot put controllable process task with id={key}", methodNamesFromStack);
                return BadRequest(ModelState);
            }

            var controllableProcessTasksFromDb = _TashDatabase.ControllableProcessTasks;
            var controllableProcessTaskFromDb = controllableProcessTasksFromDb.FirstOrDefault(p => p.Id == key);
            if (controllableProcessTaskFromDb != null) {
                _SimpleLogger.LogInformationWithCallStack($"Controllable process task deleted for id={key}", methodNamesFromStack);
                controllableProcessTasksFromDb.Remove(controllableProcessTaskFromDb);
            }

            processTask.Id = key;
            controllableProcessTasksFromDb.Add(processTask);
            _SimpleLogger.LogInformationWithCallStack($"Controllable process task inserted for id={key}", methodNamesFromStack);
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
        using (_SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController) + nameof(Patch), _SimpleLogger.LogId))) {
            var methodNamesFromStack = _MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            _SimpleLogger.LogInformationWithCallStack($"Patch controllable process task with id={key}", methodNamesFromStack);
            if (!ModelState.IsValid) {
                _SimpleLogger.LogInformationWithCallStack($"Model is not valid, cannot patch controllable process task with id={key}", methodNamesFromStack);
                return BadRequest(ModelState);
            }

            var controllableProcessTaskFromDb = _TashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == key);
            if (controllableProcessTaskFromDb == null) {
                _SimpleLogger.LogInformationWithCallStack($"No controllable process task found with id={key}", methodNamesFromStack);
                return NotFound();
            }

            patch.Patch(controllableProcessTaskFromDb);

            _SimpleLogger.LogInformationWithCallStack($"Controllable process task updated for id={key}", methodNamesFromStack);
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
        using (_SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController) + nameof(Post), _SimpleLogger.LogId))) {
            var methodNamesFromStack = _MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            _SimpleLogger.LogInformationWithCallStack($"Post controllable process task with id={processTask.Id}", methodNamesFromStack);
            var controllableProcessTaskFromDb = _TashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == processTask.Id);
            if (controllableProcessTaskFromDb != null) {
                _SimpleLogger.LogInformationWithCallStack($"A controllable process task with id={processTask.Id} already exists", methodNamesFromStack);
                return BadRequest();
            }

            _TashDatabase.ControllableProcessTasks.Add(processTask);
            _SimpleLogger.LogInformationWithCallStack($"Controllable process task inserted for id={processTask.Id}", methodNamesFromStack);
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
        using (_SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessTasksController) + nameof(Delete), _SimpleLogger.LogId))) {
            var methodNamesFromStack = _MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            _SimpleLogger.LogInformationWithCallStack($"Delete controllable process task with id={key}", methodNamesFromStack);
            var controllableProcessTaskFromDb = _TashDatabase.ControllableProcessTasks.FirstOrDefault(p => p.Id == key);
            if (controllableProcessTaskFromDb == null) {
                _SimpleLogger.LogInformationWithCallStack($"No controllable process task found with id={key}", methodNamesFromStack);
                return NotFound();
            }

            _TashDatabase.ControllableProcessTasks.Remove(controllableProcessTaskFromDb);
            _SimpleLogger.LogInformationWithCallStack($"Controllable process task deleted for id={key}", methodNamesFromStack);
            return StatusCode((int) HttpStatusCode.NoContent);
        }
    }
}